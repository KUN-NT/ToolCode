using PrintDemo.Common;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace PrintDemo.Setting
{
    public static partial class clsPrinter
    {

        #region 1、设定 默认 打印机

        /// <summary>
        /// 打印合格证\一致证前设定默认打印机
        /// clsPrinter.SetDefaultPrinter(Global.Hgz_printer);
        /// clsPrinter.SetDefaultPrinter(Global.Aecl_printer);
        /// </summary>
        /// <param name="strPrinter_Name"></param>
        /// <returns></returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string strPrinter_Name);

        #endregion

        #region 2、判断打印机设置是否正常

        /// <summary>
        /// 2、判断打印机设置是否正常
        /// </summary>
        /// <param name="strPrinter_Name"></param>
        /// <returns></returns>
        public static bool CheckPrinter(string strPrinter_Name)
        {
            if (strPrinter_Name.IsNullOrWhiteSpace())
            {
                return false;
            }

            var settings = new PrinterSettings();
            settings.PrinterName = strPrinter_Name;

            return settings.IsValid;
        }

        #endregion

        #region 3、获取默认打印机名称

        /// <summary>
        /// 3、获取默认打印机名称
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPrinter()
        {
            var m_PrintDocument = new PrintDocument();    //获取默认打印机的方法

            string strPrinterName = m_PrintDocument.PrinterSettings.PrinterName;

            return strPrinterName;
        }

        #endregion
    }
}
