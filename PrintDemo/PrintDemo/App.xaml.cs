using PrintDemo.Common;
using PrintDemo.GlobalParams;
using System;
using System.IO;
using System.Security.Permissions;
using System.Windows;

namespace PrintDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public App()
        {
            // StartUp
            Application.Current.Startup += Application_Startup;
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            #region 单线程运行，避免通信接口被占用

            Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            GlobalRes.SetProcessName();
            if (ProcessHelper.ProcessCount(GlobalRes.ProcessName) > 1)
            {
                return;
            }

            #endregion

            GlobalRes.Initial();

            Application.Current.StartupUri = new Uri(@"MainWindow.xaml", UriKind.RelativeOrAbsolute);  // MainFrm  TestCode  TestFrm.xaml Window1
        }
    }
}
