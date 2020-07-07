using Microsoft.Win32;
using System;
using System.Management;
using System.ServiceProcess;

namespace PrintDemo.Common
{
	public partial class WinSrvHelper
	{
		public static string GetDescription(string ServiceName)
		{
			//construct the management path
			string path = "Win32_Service.Name='" + ServiceName + "'";
			ManagementPath p = new ManagementPath(path);
			//construct the management object
			ManagementObject ManagementObj = new ManagementObject(p);
			if (ManagementObj["Description"] != null)
			{
				return ManagementObj["Description"].SafeToString();
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// 获取 服务 StartupType/Mode
		/// </summary>
		/// <param name="service"></param>
		/// <returns></returns>
		public static string GetStartupType(ServiceController service)
		{
			if (service == null)
			{
				return "";
			}
			return GetStartupType(service.ServiceName);
		}

		public static string GetStartupType(string ServiceName)
		{
			const String basepathStr = @"System\CurrentControlSet\services\";
			String subKeyStr = basepathStr + ServiceName;
			string startupType = string.Empty;

			using (RegistryKey reg = Registry.LocalMachine.OpenSubKey(subKeyStr))
			{
				int startupTypeValue = reg.GetValue("Start").SafeToInt32();
				switch (startupTypeValue)
				{
					case 0:
						startupType = "BOOT";
						break;

					case 1:
						startupType = "SYSTEM";
						break;

					case 2:
						startupType = "AUTOMATIC";
						break;

					case 3:
						startupType = "MANUAL";
						break;

					case 4:
						startupType = "DISABLED";
						break;

					default:
						startupType = "UNKNOWN";
						break;

				}
			}

			return startupType;
		}

		#region 获取服务状态

		/// <summary>
		/// 获取服务状态
		/// </summary>
		/// <param name="serviceName"></param>
		/// <param name="status_chn">中文状态</param>
		/// <param name="status_eng">英文状态</param>
		public static void GetStatus(string serviceName, ref string status_chn, ref string status_eng)
		{
			ServiceController sc = WinSrvHelper.GetService(serviceName);
			if (sc == null)
			{
				status_eng = "Non exist";
				status_chn = "服务不存在";
				return;
			}

			sc.Refresh();
			switch (sc.Status)
			{
				case ServiceControllerStatus.Running:
					status_eng = "Running";
					status_chn = "已启动";
					break;
				case ServiceControllerStatus.Stopped:
					status_eng = "Stopped";
					status_chn = "已停止";
					break;
				case ServiceControllerStatus.Paused:
					status_eng = "Paused";
					status_chn = "已暂停";
					break;
				case ServiceControllerStatus.StopPending:
					status_eng = "Stopping";
					status_chn = "停止中...";
					break;
				case ServiceControllerStatus.StartPending:
					status_eng = "Starting";
					status_chn = "启动中...";
					break;
				default:
					status_eng = "Status Changing";
					status_chn = "获取中...";
					break;
			}

		}
		#endregion
	}
}
