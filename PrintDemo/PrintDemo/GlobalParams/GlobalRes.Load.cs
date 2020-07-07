using PrintDemo.Common;
using PrintDemo.DataBase;
using PrintDemo.Print;
using PrintDemo.Setting;
using System;
using System.Diagnostics;
using System.Windows;

namespace PrintDemo.GlobalParams
{
    public partial class GlobalRes
	{
		#region all LoadParams for GlobalRes
		public static void Initial()
		{
			string Error_msg = string.Empty;   // Error Message

			if (ProcessName_Xml4app.IsNullOrWhiteSpace())
			{
				SetProcessName();
			}

			Initial_Printer();

			Initial_DbCon();

			Check_DB();

			GlobalRes.MessageBox_Type = XmlHelper.GetValueLikeAppSettings("MessageBox_Type", GlobalRes.ProcessName_Xml4app);

			SpireExt.Lic();
			
			// 清空临时文件
			string dir4tmp_doc = AppDomain.CurrentDomain.BaseDirectory + clsGenerate_Cert.output_folderPrefix;

			FileTxt.Del_Files4Dir(dir4tmp_doc); // 清空实时数据文件夹中的所有文件

		}

		/// <summary>
		/// 统一初始化 ProcessName；动态获取；去除 vshost
		/// </summary>
		public static void SetProcessName()
		{
			ProcessName = ProcessHelper.GetProcessName();  // 获取主进程的有效名称

			ProcessName_Xml4app = string.Format("{0}.app.xml", ProcessName); // ap 对应的配置文件名称

			ProcessName_Xml4db = string.Format("{0}.db.xml", ProcessName);

			ProcessName_Xml4printer = string.Format("{0}.printer.xml", ProcessName); // 打印机 对应的配置文件名称

			ProgTitle = XmlHelper.GetValueLikeAppSettings("ProgTitle", ProcessName_Xml4app);

		}

		public static void Initial_Printer()
		{
			GlobalRes.Printer_Name = XmlHelper.GetValueLikeAppSettings("Printer_Name", ProcessName_Xml4printer);
		}

		private static void Initial_DbCon()
		{

			GlobalRes.ConStr = XmlHelper.GetValueLikeAppSettings("ConStr", GlobalRes.ProcessName_Xml4db).SafeTrim();

			GlobalRes.m_OraConnection = new OraConnection(GlobalRes.ConStr);

		}

		/// <summary>
		/// 检查 数据库连接
		/// </summary>
		/// <returns></returns>
		private static bool Check_DB()
		{
			// 启动 listener 服务
			WinSrvHelper.Start_OracleTNSListener();

			bool db_Error_Msg = GlobalRes.m_OraConnection.CheckConnection();  // 数据库能否连接
			if (!db_Error_Msg)
			{
				MessageBox.Show("数据库连接失败，请检查数据库后启动“打印程序”！");

				Process.GetCurrentProcess().Kill();

				return false;
			}

			return true;
		}
		#endregion
	}
}
