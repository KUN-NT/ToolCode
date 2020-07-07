using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrintDemo
{
    public partial class MainWindow
    {
        public PM_WORK_ORDER GetMasterItem4Ir_Report(string wipId)
        {
            var masterItem = new PM_WORK_ORDER();
            masterItem.WIP_ID = wipId;

            return masterItem;
        }
    }
}
