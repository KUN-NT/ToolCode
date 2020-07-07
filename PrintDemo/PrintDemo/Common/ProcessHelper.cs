using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PrintDemo.Common
{
    public partial class ProcessHelper
    {
        #region GetProcessName

        /// <summary>
        /// 获取 真实的 ProcessName
        /// </summary>
        public static string GetProcessName()
        {
            string ProcessName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);  // 获取主进程的有效名称
            ProcessName = ProcessName.Replace(".vshost", ""); // 避免 debug 进程带有 vshost 引发和实际运行进程的差别。
            return ProcessName;
        }

        #endregion

        #region CheckProcessExist

        /// <summary>
        /// 限制进程开启判断：
        /// 检测自己，Length 不能大于1；检测其他程序，Length == 0
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <returns></returns>
        public static int ProcessCount(string ProcessName)
        {
            Process[] processes = Process.GetProcessesByName(ProcessName);
            if (processes != null)
            {
                return processes.Length;
            }

            return 0;
        }

        #endregion

        #region start process

        /// <summary>
        /// 确保带配置文件的exe程序可以顺利执行，路径不能带空格
        /// </summary>
        /// <param name="proc_fullPath">F:\Program Files (x86)\Notepad++\notepad++.exe</param>
        public static void Run_Proc(string proc_fullPath)
        {
            using (Process proc = new Process())
            {
                proc.StartInfo.FileName = "cmd.exe";//打开cmd程序
                proc.StartInfo.UseShellExecute = false;//不使用shell启动程序
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true; // true表示不显示黑框，false表示显示dos界面

                try
                {
                    proc.Start();//启动

                    proc.StandardInput.WriteLine(proc_fullPath);//执行程序具体位置

                    proc.WaitForExit();
                }
                catch (Exception ex)
                {
                    NLogHelper.Error(ex);
                }
            }

        }

        #endregion

        #region End Process

        /// <summary>
        /// 确保 exe 进程完全关闭
        /// </summary>
        /// <param name="strProc_Name">不含.exe</param>
        public static void End_Proc(string strProc_Name)
        {
            strProc_Name = strProc_Name.SafeTrim().TrimEnd(".exe").Safe2Lower();
            if (strProc_Name.IsNullOrWhiteSpace())
            {
                return;
            }

            Process[] prs = Process.GetProcesses();

            foreach (Process pr in prs)
            {
                if (pr.ProcessName.Safe2Lower() == strProc_Name)
                {
                    pr.Kill();
                }

            }

        }

        public static void End_Proc(List<string> proc_Names)
        {
            using (Process proc = new Process())
            {
                proc.StartInfo.FileName = "cmd.exe";//打开cmd程序
                proc.StartInfo.UseShellExecute = false;//不使用shell启动程序
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true; // true表示不显示黑框，false表示显示dos界面
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                string m_proc_Name = "";
                try
                {
                    proc.Start();//启动

                    for (int i = 0; i < proc_Names.Count; i++)
                    {
                        if (proc_Names[i].EndsWith(".exe"))
                        {
                            m_proc_Name = proc_Names[i];
                        }
                        else
                        {
                            m_proc_Name = proc_Names[i] + ".exe";
                        }
                        proc.StandardInput.WriteLine(string.Format("taskkill /f /im {0}", m_proc_Name));// 关闭进程
                    }

                    proc.StandardInput.WriteLine("exit");//退出
                    proc.Close();//关闭
                }
                catch (Exception ex)
                {
                    NLogHelper.Error(ex);
                }
            }
        }

        #endregion

        #region Restart Process

        public static void Create_RestartBat(string processName, string batName = "Restart.bat")
        {
            string dir = Directory.GetCurrentDirectory() + "\\";
            processName = processName.TrimEnd(".exe");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("taskkill /f /im {0}.exe", processName));
            sb.AppendLine(string.Format("{0}{1}.exe", dir, processName)); // D:\mesline\WpfApplication1\bin\x86\Debug\
            string batFilePath = string.Format("{0}{1}", dir, batName);

            #region 写入 bat 文件

            try
            {
                // Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
                File.WriteAllText(batFilePath, sb.ToString());

            }
            catch (Exception ex)
            {
                SimpleLogHelper.Error(ex);
                return;
            }

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processPath">进程路径</param>
        /// <param name="processName"></param>
        /// <param name="batName"></param>
        /// <param name="CreateBat">CreateBat = true 有时会出现access denied 错误，建议一般用 false</param>
        public static void Restart(string processPath, string processName, string batName = "Restart.bat", bool CreateBat = false)
        {
            if (CreateBat)
            {
                string dir = Directory.GetCurrentDirectory() + "\\";

                processName = processName.TrimEnd(".exe");

                StringBuilder sb = new StringBuilder();

                sb.AppendLine(string.Format("taskkill /f /im {0}.exe", processName));

                sb.AppendLine(processPath);

                string batFilePath = string.Format("{0}{1}", dir, batName);

                #region 写入 bat 文件

                try
                {
                    // Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
                    File.WriteAllText(batFilePath, sb.ToString());

                }
                catch (Exception ex)
                {
                    SimpleLogHelper.Error(ex);
                    return;
                }

                #endregion
            }

            BatHelper.RunBat_sameDir(batName);

        }

        #endregion

    }
}
