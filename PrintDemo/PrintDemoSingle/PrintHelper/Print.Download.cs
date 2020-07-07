using HD.Common.Utils.FileUtils;
using System;
using System.IO;
using System.Net;

namespace PrintDemoSingle
{
    /// <summary>
    /// 获取打印模板
    /// </summary>
    public partial class Print
    {
        //static string tmpServerPath = AppDomain.CurrentDomain.GetData("webRes").ToString() + "WordTemplate/";
        static string tmpLocalPath = AppDomain.CurrentDomain.BaseDirectory + @"Download\WordTemplate\";
        static string expLocalPath = AppDomain.CurrentDomain.BaseDirectory + @"Download\ExpFiles\";

        //private static void DownloadTemplate(string templateName)
        //{
        //    using (WebClient client = new WebClient())
        //    {
        //        client.Safe_DownloadFile(tmpServerPath + templateName, tmpLocalPath + templateName);
        //    }
        //    if (!Directory.Exists(expLocalPath))
        //    {
        //        Directory.CreateDirectory(expLocalPath);
        //    }
        //}
    }
}
