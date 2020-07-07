using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace PrintDemoSingle
{
    /// <summary>
    /// 打印机设置
    /// </summary>
    public partial class Print
    {
        #region 设置默认打印机
        /// <summary>
        /// 设置默认打印机
        /// </summary>
        /// <param name="strPrinter_Name"></param>
        /// <returns></returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string strPrinter_Name);
        #endregion

        #region 获取默认打印机名称
        /// <summary>
        /// 获取默认打印机名称
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
