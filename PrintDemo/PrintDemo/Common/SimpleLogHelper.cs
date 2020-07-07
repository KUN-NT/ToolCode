using System;
using System.IO;
using System.Text;

namespace PrintDemo.Common
{
    public class SimpleLogHelper
    {

        public static string BeautyErrorMsg(Exception ex, bool ShowInnerException = true)
        {
            StringBuilder sb = new StringBuilder(0);
            sb.AppendLine(string.Format("【异常类型】：{0};", ex.GetType().Name));
            sb.AppendLine(string.Format("【异常信息】：{0};", ex.Message));
            sb.AppendLine(string.Format("【堆栈调用】：{0};", ex.StackTrace));

            if (ShowInnerException && ex.InnerException != null)
            {
                sb.AppendLine(string.Format("【InnerException 异常类型】：{0};", ex.InnerException.GetType().Name));
                sb.AppendLine(string.Format("【InnerException 异常信息】：{0};", ex.InnerException.Message));
                sb.AppendLine(string.Format("【InnerException 堆栈调用】：{0};", ex.StackTrace));
            }

            return sb.ToString().TrimEnd(Environment.NewLine);
        }

        public static string BeautyErrorMsg(Exception ex, string title, bool ShowInnerException = true)
        {
            StringBuilder sb = new StringBuilder(0);
            sb.AppendLine(string.Format("【异常名称】：{0};", title));
            sb.AppendLine(string.Format("【异常类型】：{0};", ex.GetType().Name));
            sb.AppendLine(string.Format("【异常信息】：{0};", ex.Message));
            sb.AppendLine(string.Format("【堆栈调用】：{0};", ex.StackTrace));

            if (ShowInnerException && ex.InnerException != null)
            {
                sb.AppendLine(string.Format("【InnerException 异常类型】：{0};", ex.InnerException.GetType().Name));
                sb.AppendLine(string.Format("【InnerException 异常信息】：{0};", ex.InnerException.Message));
                sb.AppendLine(string.Format("【InnerException 堆栈调用】：{0};", ex.StackTrace));
            }

            return sb.ToString().TrimEnd(Environment.NewLine);
        }

        /// <summary>
        /// write log to baseDir/Logs/日期/UnhandledException.log
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg, string logFileName = "Unhandled_Exception.slog")
        {
            StringBuilder sb = new StringBuilder(0);

            sb.AppendLine(string.Format("【异常时间】：{0};", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")));
            sb.AppendLine(msg);
            sb.AppendLine();
            sb.AppendLine("***************************************************************************");

            string path = string.Format(@"{0}\Logs\{1}", Directory.GetCurrentDirectory(), DateTime.Now.ToString("yyyy-MM-dd"));

            Directory.CreateDirectory(path);

            string filename = string.Format(@"{0}\{1}", path, logFileName);
            File.AppendAllText(filename, sb.ToString());
        }

        /// <summary>
        /// 在程序目录生成简易log。其他log可能失效时使用。
        /// 不足：不支持自动归档
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            StringBuilder sb = new StringBuilder(0);
            sb.Append(BeautyErrorMsg(ex));

            Error(sb.ToString());
        }

    }
}
