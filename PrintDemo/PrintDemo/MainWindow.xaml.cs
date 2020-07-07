using PrintDemo.Common;
using PrintDemo.GlobalParams;
using PrintDemo.Setting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrintDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Dfg_Printers();
            GetAllTask_Page(0);
        }

        public void BtnPrint_IrCert_Click(object sender, RoutedEventArgs e)
        {

            var seleItems = this.all_taskGridView.SelectedItems;

            if (seleItems==null)
            {
                MessageBox.Show("请至少选中一条数据");
                return;
            }

            clsPrinter.SetDefaultPrinter(GlobalRes.Printer_Name);

            string strSTERILIZE_CODE = ""; // 灭菌批号

            string strCUSTOMER_ORDER_ID = ""; // 客户订单号

            string errorMsg = "";

            for (int i = 0; i < seleItems.Count; i++)
            {
                var currentItem = seleItems[i] as PM_WORK_ORDER;
                if (currentItem == null)
                {
                    continue;
                }

                var masterItem = GetMasterItem4Ir_Report(currentItem.WIP_ID);
                if (masterItem == null)
                {
                    continue;
                }

                clsGenerate_Cert.Print(masterItem, ref errorMsg);

                if (!errorMsg.IsNullOrWhiteSpace())
                {
                    MessageBox.Show(errorMsg);

                    break;
                }
            }


            this.all_taskGridView.SelectedItems.Clear();
        }
    }
}
