using Aspose.Cells;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoutingEquipImp
{
    public partial class MainWindow
    {
        public List<string> NoEquipItem = new List<string>();
        public Task DataCheck(WorksheetCollection sheet)
        {
            return Task.Run(() =>
            {
                int startEquipIndex = 3;
                int startDetailIndex = 4;
                bool endEquip = false;
                List<string> equipList = new List<string>();
                while (!endEquip)
                {
                    string equipCode = sheet[0].Cells["A" + startEquipIndex.ToString()].StringValue.Trim();
                    if (string.IsNullOrWhiteSpace(equipCode))
                    {
                        endEquip = true;
                        continue;
                    }
                    equipList.Add(equipCode);
                    startEquipIndex++;
                }
                while (startDetailIndex < 55244)
                {
                    string equipCode = sheet[1].Cells["G" + startDetailIndex.ToString()].StringValue.Trim();
                    string itemCode = sheet[1].Cells["B" + startDetailIndex.ToString()].StringValue.Trim();
                    if (string.IsNullOrWhiteSpace(equipCode))
                    {
                        startDetailIndex++;
                        continue;
                    }
                    if (!equipList.Contains(equipCode))
                    {
                        NoEquipItem.Add(itemCode);
                        string msg= startDetailIndex.ToString() + "\t" + itemCode + "\t" + equipCode;
                        txtMessage.Dispatcher.Invoke(new RefleshUI(SetMessage), true, msg);
                    }
                    startDetailIndex++;
                }
            });
        }
    }
}
