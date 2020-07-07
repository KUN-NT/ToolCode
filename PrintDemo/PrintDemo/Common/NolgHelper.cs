using NLog;
using PrintDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintDemo.Common
{
    public class NLogHelper // : IGeneralLogger
	{
		private static Logger nlogger = LogManager.GetCurrentClassLogger();

		#region Debug
		public static void Debug(string msg)
		{
			try
			{
				nlogger.Log(LogLevel.Debug, msg);
			}
			catch
			{
				SimpleLogHelper.Error(msg);
			}
		}
		#endregion
		
		#region Info
		public static void Info(string msg)
		{
			try
			{
				nlogger.Log(LogLevel.Info, msg);
			}
			catch
			{
				SimpleLogHelper.Error(msg);
			}
		}

		/// <summary>
		/// 用于小老化室温度显示，跟踪采集到的参数数值
		/// </summary>
		/// <param name="dic"></param>
		public static void Info<T1, T2>(Dictionary<T1, T2> dic)
		{
			int dic_Count = dic.Count;
			if (dic_Count == 0)
			{
				return;
			}

			StringBuilder sb = new StringBuilder(0);
			string temp = "";
			int signal_FirstRow = 0;

			// 第一行之外附加"     "，以便对齐nlog样式
			foreach (var dic_item in dic)
			{
				signal_FirstRow++;
				if (signal_FirstRow == 1)
				{
					temp = string.Format("{0}:{1}", dic_item.Key, dic_item.Value);
				}
				else
				{
					temp = string.Format("{0}{1}:{2}", "     ", dic_item.Key, dic_item.Value);
				}

				if (signal_FirstRow < dic_Count)
				{
					sb.AppendLine(temp);
				}
				else
				{
					sb.Append(temp); //去除 nlog 多余回行
				}
			}

			temp = sb.ToString();
			if (temp.IsNullOrEmpty())
			{
				return;
			}

			try
			{
				if (temp.ContainsEx(" not ")) // 显示错误型日志
				{
					nlogger.Log(LogLevel.Error, temp);
					return;
				}
				nlogger.Log(LogLevel.Info, temp);
			}
			catch
			{
				SimpleLogHelper.Error(temp);
			}
		}

		/// <summary>
		/// 用于跟踪 plc 采集前插入的数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parameterValues"></param>
		public static void Info<T>(List<T> parameterValues)
		{
			int list_Count = parameterValues.Count;
			if (list_Count == 0)
			{
				return;
			}

			StringBuilder sb = new StringBuilder(0);
			string temp = "";
			int signal_FirstRow = 0;

			// 第一行之外附加"     "，以便对齐nlog样式
			foreach (var listItem in parameterValues)
			{
				signal_FirstRow++;
				if (signal_FirstRow == 1)
				{
					temp = StringHelper.SafeToString(listItem);
				}
				else
				{
					temp = string.Format("{0}{1}", "     ", StringHelper.SafeToString(listItem));
				}

				if (signal_FirstRow < list_Count)
				{
					sb.AppendLine(temp);
				}
				else
				{
					sb.Append(temp); //去除多余回行
				}
			}

			temp = sb.ToString();
			if (temp.IsNullOrEmpty())
			{
				return;
			}

			try
			{
				if (temp.ContainsEx(" not ")) // 显示错误型日志
				{
					nlogger.Log(LogLevel.Error, temp);
					return;
				}
				nlogger.Log(LogLevel.Info, temp);
			}
			catch
			{
				SimpleLogHelper.Error(temp);
			}
		}


		#endregion

		public static void Warn(string msg)
		{

			try
			{
				nlogger.Log(LogLevel.Warn, msg);
			}
			catch
			{
				SimpleLogHelper.Error(msg);
			}
		}

		#region Error
		public static void Error(string msg)
		{
			try
			{
				nlogger.Log(LogLevel.Error, msg);
			}
			catch
			{
				SimpleLogHelper.Error(msg);
			}
		}

		public static void Error<T1, T2>(Dictionary<T1, T2> dic)
		{
			int dic_Count = dic.Count;
			if (dic_Count == 0)
			{
				return;
			}

			StringBuilder sb = new StringBuilder(0);
			string temp = "";
			int signal_FirstRow = 0;

			// 第一行之外附加"     "，以便对齐nlog样式
			foreach (var dic_item in dic)
			{
				signal_FirstRow++;
				if (signal_FirstRow == 1)
				{
					temp = string.Format("{0}:{1}", dic_item.Key, dic_item.Value);
				}
				else
				{
					temp = string.Format("{0}{1}:{2}", "     ", dic_item.Key, dic_item.Value);
				}

				if (signal_FirstRow < dic_Count)
				{
					sb.AppendLine(temp);
				}
				else
				{
					sb.Append(temp); //去除多余回行
				}
			}

			temp = sb.ToString();
			if (temp.IsNullOrEmpty())
			{
				return;
			}

			try
			{
				if (temp.ContainsEx(" not ")) // 显示错误型日志
				{
					nlogger.Log(LogLevel.Error, temp);
					return;
				}
				nlogger.Log(LogLevel.Info, temp);
			}
			catch
			{
				SimpleLogHelper.Error(temp);
			}
		}

		public static void Error<T>(List<T> parameterValues)
		{
			int list_Count = parameterValues.Count;
			if (list_Count == 0)
			{
				return;
			}

			StringBuilder sb = new StringBuilder(0);
			string temp = "";
			int signal_FirstRow = 0;

			// 第一行之外附加"     "，以便对齐nlog样式
			foreach (var listItem in parameterValues)
			{
				signal_FirstRow++;
				if (signal_FirstRow == 1)
				{
					temp = StringHelper.SafeToString(listItem);
				}
				else
				{
					temp = string.Format("{0}{1}", "     ", StringHelper.SafeToString(listItem));
				}

				if (signal_FirstRow < list_Count)
				{
					sb.AppendLine(temp);
				}
				else
				{
					sb.Append(temp); //去除多余回行
				}
			}

			temp = sb.ToString();
			if (temp.IsNullOrEmpty())
			{
				return;
			}

			try
			{
				if (temp.ContainsEx(" not ")) // 显示错误型日志
				{
					nlogger.Log(LogLevel.Error, temp);
					return;
				}
				nlogger.Log(LogLevel.Error, temp);
			}
			catch
			{
				SimpleLogHelper.Error(temp);
			}
		}

		public static void Error(Exception ex)
		{
			string msg = SimpleLogHelper.BeautyErrorMsg(ex);
			Error(msg);
		}

		public static void Error(Exception ex, string title)
		{
			string msg = SimpleLogHelper.BeautyErrorMsg(ex, title);
			Error(msg);
		}
		#endregion

		public static void SetLogLevel(LogLevel lv)
		{
			var lc = LogManager.Configuration;
			var lr = lc.LoggingRules.FirstOrDefault(
				r => r.Targets.Any(
					t => "AllFile" == t.Name
					)
			); // 查找 AllFile 所用的 LoggingRu
			var tag = lc.AllTargets.FirstOrDefault(t => t.Name == "AllFile");
			//lr.Levels = lv;
			if (null != lr)
			{
				lc.LoggingRules.Remove(lr);    // 删除旧的 LoggingRule .
			}

			lc.LoggingRules.Add(new NLog.Config.LoggingRule("*", lv, tag));
			LogManager.ReconfigExistingLoggers();
			nlogger.Info("修改日志等级至" + lv.ToString());
		}

		public static void SetLogLevel(string lvs)
		{
			LogLevel lv = LogLevel.Info;
			try
			{
				lv = LogLevel.FromString(lvs);
			}
			catch (Exception ex)
			{
				if (null == nlogger) return;
				nlogger.Info("LogLevel.FromString fail!");
			}
			SetLogLevel(lv);
		}
	}
}
