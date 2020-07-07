using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PrintDemo.Common
{
    public partial class BatHelper
    {
        #region Suicide

        /// <summary>
        /// 程序运行结束后自我关闭，保证只有一个进程在执行。
        /// exe 文件修改后，需对应修改 bat 文件中的文件名
        /// CreateBat = true 有时会出现access denied 错误
        /// </summary>
        public static void Suicide(string ProcessName, string SuicideBatName = "Suicide.bat", bool CreateBat = false)
        {
            string dir = Directory.GetCurrentDirectory() + "\\";
            string BatFilePath = string.Format("{0}{1}", dir, SuicideBatName);

            if (CreateBat)
            {
                ProcessName = ProcessName.TrimEnd(".exe");
                string Suicide_cmd = string.Format("taskkill /f /im {0}.exe", ProcessName);

                #region 写入 bat 文件

                try
                {
                    // Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
                    File.WriteAllText(BatFilePath, Suicide_cmd);

                }
                catch (Exception ex)
                {
                    SimpleLogHelper.Error(ex);
                    return;
                }

                #endregion
            }

            RunBat_byPath(BatFilePath);

        }

        /// <summary>
        /// 执行bat文件，也可用于打开帮助文件
        /// </summary>
        /// <param name="BatFilePath"></param>
        public static void RunBat_byPath(string BatFilePath)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = BatFilePath;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }

        public static void RunBat(string batPath)
        {
            Process pro = new Process();

            FileInfo file = new FileInfo(batPath);
            pro.StartInfo.WorkingDirectory = file.Directory.FullName;
            pro.StartInfo.FileName = batPath;
            pro.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pro.StartInfo.UseShellExecute = false;
            pro.Start();
            pro.WaitForExit();
        }

        /// <summary>
        /// 执行在同一目录下的bat文件
        /// </summary>
        /// <param name="startBatName"></param>
        public static void RunBat_sameDir(string startBatName)
        {
            string dir = Directory.GetCurrentDirectory() + "\\" + startBatName;
            using (Process proc = new Process())
            {
                FileInfo file = new FileInfo(dir);
                proc.StartInfo.WorkingDirectory = file.Directory.FullName;
                proc.StartInfo.FileName = dir;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
            }
        }

        /// <summary>
        /*动态生成 多行 cmd，并执行*/
        /// start "" "D:\Program Files\Tencent\QQ.exe"
        /// start D:\CrossFire\CrossFire.exe
        /// 1、路径中有空格的按第一排写，加上引号（注意除了路径要引起来外，前面还有一对引号）；
        /// 2、路径没有空格的按第二排写。
        /// 3、每个需要启动的程序写一行，就可以同时启动多个程序
        ///  <param name="XmlFilePath">Xml 配置文件</param>
        /// </summary>
        public static void StartExe(string XmlFilePath = "Bat.xml")
        {
            #region 构造 cmd 内容

            List<string> cmdBuilder = new List<string>(0);

            string dir = Directory.GetCurrentDirectory() + "\\";
            string disk_navi = "";
            if (!string.IsNullOrWhiteSpace(dir))
            {
                disk_navi = dir[0].ToString() + ":";
            }

            string startBatName = XmlHelper.GetSingleValueFromChildNode1("startBatName", XmlFilePath);
            string ExtExeName = XmlHelper.GetSingleValueFromChildNode1("ExtExeName", XmlFilePath);
            string BatFilePath = string.Format("{0}{1}", dir, startBatName);
            string ExtExePath = string.Format("{0}{1}", dir, ExtExeName);

            string start_cmd = "";
            if (BatFilePath.ContainsEx(" "))
            {
                start_cmd = string.Format("start \"\" \"{0}\"", ExtExePath);
            }
            else
            {
                start_cmd = string.Format("start {0}", ExtExePath);
            }

            cmdBuilder.Add(disk_navi);
            cmdBuilder.Add(start_cmd);

            #endregion

            FileHelper.CreateText4Bat(cmdBuilder, BatFilePath);

            //Run your Batch File & Remove it when finished.
            RunBat_byPath(BatFilePath);

        }

        #endregion
    }
}
