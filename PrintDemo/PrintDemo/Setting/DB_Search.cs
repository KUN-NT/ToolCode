using PrintDemo.DataBase;
using PrintDemo.GlobalParams;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintDemo
{
    public partial class MainWindow
    {
        public void GetAllTask_Page(int pageIndex)
        {
            int startIndex = pageIndex * 10 + 1;

            int endIndex = (pageIndex + 1) * 10;

            var result = GetIrCert_Page(startIndex, endIndex);
            List<PM_WORK_ORDER> vs = new List<PM_WORK_ORDER>();
            foreach(DataRow row in result.Tables[0].Rows)
            {
                PM_WORK_ORDER pM_WORK_ORDER = new PM_WORK_ORDER();
                pM_WORK_ORDER.WIP_ID = row.ItemArray[2].ToString();
                vs.Add(pM_WORK_ORDER);
            }
            this.all_taskGridView.ItemsSource = vs;
        }

        public static DataSet GetIrCert_Page(int startIndex, int endIndex)
        {
            string strSql = "select * from KAIAN_MES_PPM.PM_WORK_ORDER ";
            var m_OraAccess = new OraAccess(GlobalRes.ConStr);
            var task_Items = m_OraAccess.SelectBySQL(strSql);
            if (!task_Items.HasRows())
            {
                return null;
            }
            return task_Items;
        }

    }

    public class PM_WORK_ORDER
    {
        public string WIP_ID { get; set; }
    }
}
