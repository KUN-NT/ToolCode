using PrintDemo.Common;
using PrintDemo.GlobalParams;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows;

namespace PrintDemo
{
    public partial class MainWindow
    {
        #region 加载上次的打印机设置

        /// <summary>
        /// 加载上次的打印机设置
        /// </summary>
        private void Load_Dfg_Printers()
        {
            // 系统未安装打印机，给出提示
            if (PrinterSettings.InstalledPrinters.Count == 0)
            {
                MessageBox.Show("当前系统未检测到打印机，请先安装并启动打印机！");
                return;
            }

            // 所有安装的打印机列表
            var list_InstalledPrinters = new List<string>(0);

            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                var m_InstalledPrinter = PrinterSettings.InstalledPrinters[i];
                if (m_InstalledPrinter.IsNullOrWhiteSpace())
                {
                    continue;
                }

                list_InstalledPrinters.Add(m_InstalledPrinter);

                this.cb_Printer.Items.Add(m_InstalledPrinter);
            }

            if (list_InstalledPrinters == null || list_InstalledPrinters.Count == 0)
            {
                return;
            }

            // 设定“条码打印机”
            if (list_InstalledPrinters.Contains(GlobalRes.Printer_Name))
            {
                this.cb_Printer.LocationComboBoxByText(GlobalRes.Printer_Name);
            }
        }

        #endregion

        #region 保存打印机设置

        private void BtnSave_Printer_Click(object sender, RoutedEventArgs e)
        {

            #region 1、输入验证

            string str_printer = cb_Printer.Text;


            if (str_printer.IsNullOrWhiteSpace())
            {
                MessageBox.Show("未选择其他打印机，请先设置！");
                return;
            }

            #endregion

            #region 2、保存 打印机 设置

            XmlHelper.WriteValueLikeAppSettings("Printer_Name", str_printer, GlobalRes.ProcessName_Xml4printer);

            GlobalRes.Printer_Name = str_printer;

            MessageBox.Show("“打印机设置”保存成功！");

            #endregion

        }

        #endregion
    }
}
