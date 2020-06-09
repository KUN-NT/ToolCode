using Aspose.Cells;
using HD.SVR.PPS.RSM.DataAccess.DataModels;
using HD.SVR.PPS.RSM.DataAccess.DataServices;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoutingEquipImp
{
    public partial class MainWindow
    {
        private List<RS_WORK_CENTER> centerItems;
        private List<RS_EQUIP_MASTER> machItems;
        public Task ReadEquipData(Worksheet sheet, int lastIndex)
        {
            InitialEquipData();
            return Task.Run(() =>
            {
                service = new DBService();
                int rowIndex = 3;

                while (rowIndex < lastIndex)
                {
                    string equipCode = sheet.Cells["A" + rowIndex.ToString()].StringValue.Trim();

                    if (string.IsNullOrWhiteSpace(equipCode))
                    {
                        CreateWorkCenters();
                        SaveEquipData();
                        rowIndex++;
                        continue;
                    }
                    string equipName = sheet.Cells["B" + rowIndex.ToString()].StringValue.Trim();
                    string equipType = sheet.Cells["C" + rowIndex.ToString()].StringValue.Trim();
                    string equipNorm = sheet.Cells["D" + rowIndex.ToString()].StringValue.Trim();
                    string workCenter = sheet.Cells["I" + rowIndex.ToString()].StringValue.Trim();
                    string startUse = sheet.Cells["K" + rowIndex.ToString()].StringValue.Trim();
                    string equipStatus = sheet.Cells["M" + rowIndex.ToString()].StringValue.Trim();

                    if (!machItems.Any(p => p.EQUIP_CODE == equipCode))
                    {
                        RS_EQUIP_MASTER equipMaster= new RS_EQUIP_MASTER();
                        DateTime efDate1 = DateTime.Now;
                        DateTime.TryParse(startUse, out efDate1);
                        //equipMaster.EF_DATE1 = efDate1;
                        equipMaster.EQUIP_CODE = equipCode;
                        equipMaster.EQUIP_NAME = equipName;
                        equipMaster.EQUIP_TYPE = equipType;
                        equipMaster.EQUIP_NORM = equipNorm;
                        equipMaster.EF_CHAR2 = workCenter;
                        equipMaster.EQUIP_STATUS = equipStatus=="Y"?"运行":"未采集";
                        machItems.Add(equipMaster);
                    }
                    
                    rowIndex++;
                }
            });
        }
        
        private void InitialEquipData()
        {
            centerItems = new List<RS_WORK_CENTER>();
            machItems = new List<RS_EQUIP_MASTER>();
        }
        private void CreateWorkCenters()
        {
            var workCenter = machItems.GroupBy(p => p.EF_CHAR2);
            int no = 1;
            foreach (var workEquip in workCenter)
            {
                string wcCode = service.HaveWork(workEquip.Key);
                RS_WORK_CENTER work = new RS_WORK_CENTER();
                work.WC_CODE = string.IsNullOrEmpty(wcCode)?'W'+ no.ToString("000"):wcCode;
                work.WC_NAME = workEquip.Key;
                work.EQUIP_QTY = workEquip.Count();
                foreach (var equip in workEquip)
                {
                    equip.WC_CODE = work.WC_CODE;
                }
                centerItems.Add(work);
                no++;
            }
        }
        private void SaveEquipData()
        {
            if (centerItems.Count > 0)
            {
                var result = service.ImportEquipMasters(centerItems,machItems);
                if (result == "Y")
                {
                    txtMessage.Dispatcher.Invoke(new RefleshUI(SetMessage),true,"处理完成");
                }
                else
                {
                    txtMessage.Dispatcher.Invoke(new RefleshUI(SetMessage),false, result);
                }
                centerItems.Clear();
                machItems.Clear();
            }
        }
    }
}
