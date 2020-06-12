using Aspose.Cells;
using HD.SVR.PPS.RSM.DataAccess.DataModels;
using HD.SVR.PPS.RSM.DataAccess.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoutingEquipImp
{
    public partial class MainWindow
    {
        private RS_ROUTING_HEAD headItem;
        private List<RS_ROUTING_DETAIL> detailItems;
        private List<RS_ROUTING_EQUIP> equipItems;
        private DBService service;
        private string currentRouting;
        public Task ReadRoutingData(Worksheet sheet, int lastIndex)
        {
            return Task.Run(() =>
            {
                currentRouting = "";
                service = new DBService();
                int rowIndex = 4;

                while (rowIndex < lastIndex)
                {
                    string opName = sheet.Cells["E" + rowIndex.ToString()].StringValue;
                    string opNoValue = sheet.Cells["F" + rowIndex.ToString()].StringValue;
                    if (string.IsNullOrWhiteSpace(opName))
                    {
                        SaveData();
                        currentRouting = "";
                        rowIndex++;
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(opNoValue))
                    {
                        rowIndex++;
                        continue;
                    }
                    string itemCode = sheet.Cells["B" + rowIndex.ToString()].StringValue.Trim();
                    string itemName = sheet.Cells["C" + rowIndex.ToString()].StringValue.Trim();
                    string itemUnit = sheet.Cells["D" + rowIndex.ToString()].StringValue.Trim();
                    string equipCode = sheet.Cells["G" + rowIndex.ToString()].StringValue.Trim();
                    string rhythmStr = sheet.Cells["H" + rowIndex.ToString()].StringValue.Trim();
                    string shiftStr = sheet.Cells["I" + rowIndex.ToString()].StringValue.Trim();
                    string dispatchStr = sheet.Cells["J" + rowIndex.ToString()].StringValue.Trim();
                    string stationName = sheet.Cells["K" + rowIndex.ToString()].StringValue.Trim();
                    string processId = sheet.Cells["L" + rowIndex.ToString()].StringValue.Trim();
                    string stationCode = sheet.Cells["M" + rowIndex.ToString()].StringValue.Trim();
                    if (NoEquipItem.Contains(itemCode))
                    {
                        rowIndex++;
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(itemCode) && currentRouting == "")
                    {
                        InitialData();
                        CreateNewHead(itemCode);
                    }
                    if (!string.IsNullOrWhiteSpace(itemCode) && currentRouting != itemCode)
                    {
                        SaveData();
                        InitialData();
                        CreateNewHead(itemCode);
                    }

                    decimal opNo = -1;
                    decimal.TryParse(opNoValue, out opNo);
                    if (opNo == -1)
                    {
                        Console.WriteLine("第" + rowIndex.ToString() + "行,工序号格式错误");
                        rowIndex++;
                        continue;
                    }
                    var detailItem = detailItems.FirstOrDefault(p => p.PROCESS_ID == processId);
                    if (detailItem == null)
                    {
                        detailItem = new RS_ROUTING_DETAIL();
                        detailItem.PROCESS_ID = string.IsNullOrWhiteSpace(processId)
                            ? Guid.NewGuid().ToString().Replace("-", "")
                            : processId;
                        detailItem.OP_NO = opNo;
                        detailItem.OP_NAME = opName;
                        detailItem.STATION_ID = stationCode;
                        detailItem.STATION_NAME = stationName;
                        detailItem.ROUTING_NO = headItem.ROUTING_NO;
                        detailItems.Add(detailItem);
                    }

                    decimal rhythmValue = 0;
                    decimal.TryParse(rhythmStr, out rhythmValue);

                    decimal shiftValue = 0;
                    decimal.TryParse(shiftStr, out shiftValue);

                    decimal dispatchSeq = 0;
                    decimal.TryParse(dispatchStr, out dispatchSeq);
                    if (!equipItems.Any(p => p.PROCESS_ID == detailItem.PROCESS_ID && p.EQUIP_CODE == equipCode))
                    {
                        if (!string.IsNullOrEmpty(equipCode.Trim()))
                        {
                            var equipItem = new RS_ROUTING_EQUIP();
                            equipItem.PROCESS_ID = detailItem.PROCESS_ID;
                            equipItem.EQUIP_CODE = equipCode;
                            equipItem.RHYTHM_VALUE = rhythmValue;
                            equipItem.SHIFT_VALUE = shiftValue;
                            equipItem.DISPATCH_SEQ = dispatchSeq;
                            equipItems.Add(equipItem);
                        }
                    }
                    rowIndex++;
                }
            });
        }

        private void CreateNewHead(string strHead)
        {
            headItem.ROUTING_NO = strHead;
            currentRouting = strHead;
        }

        private void InitialData()
        {
            headItem = new RS_ROUTING_HEAD();
            detailItems = new List<RS_ROUTING_DETAIL>();
            equipItems = new List<RS_ROUTING_EQUIP>();
        }
        public delegate void RefleshUI(bool isSuccess,string s);
        private void SaveData()
        {
            if (detailItems.Count > 0)
            {
                var result = service.ImportRoutingDetails(headItem, detailItems, equipItems);
                if (result == "Y")
                {
                    txtMessage.Dispatcher.Invoke(new RefleshUI(SetMessage),true,headItem.ROUTING_NO + "处理完成");
                }
                else
                {
                    txtMessage.Dispatcher.Invoke(new RefleshUI(SetMessage), false,headItem.ROUTING_NO + ":" + result);
                }
                detailItems.Clear();
            }
        }
    }
}
