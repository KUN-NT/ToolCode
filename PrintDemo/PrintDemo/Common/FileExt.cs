using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintDemo.Common
{
    public class FileExt
    {

        #region 判断文件是否正在被占用

        /// <summary>
        /// 返回指示文件是否已被其它程序使用的布尔值
        /// </summary>
        /// <param name="fileFullName">文件的完全限定名，例如：“C:\MyFile.txt”。</param>
        /// <returns>如果文件已被其它程序使用，则为 true；否则为 false。</returns>
        public static File_Status Check_Status(String fileFullName)
        {
            //判断文件是否存在，如果不存在，直接返回 NonExisting
            if (!System.IO.File.Exists(fileFullName))
            {
                return File_Status.NonExisting;

            }
            else
            {//如果文件存在，则继续判断文件是否已被其它程序使用

                //逻辑：尝试执行打开文件的操作，如果文件已经被其它程序使用，则打开失败，抛出异常，根据此类异常可以判断文件是否已被其它程序使用。
                System.IO.FileStream fileStream = null;
                try
                {
                    fileStream = System.IO.File.Open(fileFullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

                    return File_Status.Unlocked;
                }
                catch (System.IO.IOException ioEx)
                {
                    return File_Status.Locked;
                }
                catch (System.Exception ex)
                {
                    return File_Status.Locked;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }

            }

        }

        #endregion

        #region 执行程序

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="file_path"></param>
        /// <param name="file_name"></param>
        public static void RunFile(string file_path, string file_name)
        {
            using (Process pro = new Process())
            {
                pro.StartInfo.FileName = file_name;

                pro.StartInfo.WorkingDirectory = file_path;

                pro.StartInfo.CreateNoWindow = true;

                pro.StartInfo.RedirectStandardError = false;

                pro.Start();

                //pro.WaitForExit();
            }
        }

        #endregion

        #region 创建目录，文件名时，过滤特殊字符串、非法字符

        /// <summary>
        /// 创建目录，文件名时，过滤特殊字符串、非法字符
        /// </summary>
        /// <param name="strOriginal_FileName">过滤前的文件名，可能含有非法字符</param>
        public static string Replace_BadChars(string strOriginal_FileName)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                strOriginal_FileName = strOriginal_FileName.Replace(c.ToString(), "");
            }

            return strOriginal_FileName;
        }

        #endregion

    }
}
