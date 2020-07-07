using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace PrintDemo.Common
{
    public partial class WinSrvHelper
    {
        public static ServiceController GetService(string serviceName, bool exactMatch = true)
        {
            ServiceController[] services = ServiceController.GetServices();
            if (exactMatch)
            {
                return services.FirstOrDefault(p => p.ServiceName == serviceName);
            }
            else
            {
                return services.FirstOrDefault(p => p.ServiceName.ToLower().Contains(serviceName.Trim().ToLower()));
            }
        }

        /// <summary>
        /// 根据模糊匹配，启动 Oracle 服务及listener 服务
        /// </summary>
        public static void Start_OracleServices()  // OracleOraDb10g_home1TNSListener OracleServiceJUSTINOR
        {
            Start_OracleTNSListener();

            var OracleService_service = GetService("OracleService", false);
            StartService(OracleService_service);
        }

        public static void Start_OracleTNSListener()  // OracleOraDb10g_home1TNSListener OracleServiceJUSTINOR
        {
            var TNSListener_service = GetService("TNSListener", false);
            if (TNSListener_service == null)
            {
                return;
            }

            StartService(TNSListener_service);
        }

        public static bool IsServiceRunning(string serviceName)
        {
            ServiceControllerStatus status;
            uint counter = 0;
            do
            {
                ServiceController service = GetService(serviceName);
                if (service == null)
                {
                    return false;
                }

                Thread.Sleep(100);         // 等待100ms 获取服务状态
                status = service.Status;
            } while (!(status == ServiceControllerStatus.Stopped ||
                       status == ServiceControllerStatus.Running) &&
                     (++counter < 30));
            return status == ServiceControllerStatus.Running;
        }

        public static bool IsServiceInstalled(string serviceName)
        {
            return GetService(serviceName) != null;
        }

        public static void StartService(List<string> serviceNames)
        {
            for (int i = 0; i < serviceNames.Count; i++)
            {
                StartService(serviceNames[i]);
            }
        }

        /// <summary>
        /// 将服务设置为 自动启动，并启动未启动的服务
        /// </summary>
        /// <param name="service"></param>
        public static void StartService(ServiceController service)
        {
            if (service == null)
            {
                return;
            }

            try
            {
                if (WinSrvHelper.GetStartupType(service) != "AUTOMATIC")
                {
                    ChangeStartMode(service, ServiceStartMode.Automatic);  // 将服务设置为 自动启动
                }

                if (service.Status == ServiceControllerStatus.Running) // 已启动的服务不能再次启动
                {
                    return;
                }

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);

            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        public static void StartService(string serviceName)
        {
            ServiceController service = GetService(serviceName);
            if (service == null)
            {
                return;
            }
            StartService(service);
        }

        public static void StopService(string serviceName)
        {
            ServiceController controller = GetService(serviceName);
            if (controller == null)
            {
                return;
            }

            try
            {
                if (controller.Status == ServiceControllerStatus.Running)
                {
                    controller.Stop();
                    controller.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        public static void SafeChangeStartMode(string serviceName, ServiceStartMode mode)
        {
            ServiceController service = GetService(serviceName);
            if (service == null)
            {
                return;
            }

            string strMode = mode.ToString().ToUpper().TrimStart("SERVICESTARTMODE.");

            try
            {
                if (WinSrvHelper.GetStartupType(service) != strMode)
                {
                    ChangeStartMode(service, mode);  // 将服务设置为 自动启动
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
    }
}
