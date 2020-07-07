using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PrintDemo.Common
{
    public class FileHelper
    {

        #region 安全删除

        public static void Safe_Del(string strFile)
        {
            if (!File.Exists(strFile))
            {
                return;
            }

            try
            {
                File.Delete(strFile);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        #endregion


        #region 严格要求 文件存在 且 文件大小 > 0 字节

        /// <summary>
        /// 严格要求 文件存在 且 文件大小 > 0 字节
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool Safe_Exists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            var fileInfo = new FileInfo(filePath);

            var fileSize = fileInfo.Length; // 字节

            if (fileSize == 0)
            {
                return false;
            }

            return true;
        }

        #endregion


        public static void MoveToSpecialFilePath(string sourceHttpFilePath, string targetFilePath)
        {
            if (string.IsNullOrEmpty(targetFilePath))
            {
                NLogHelper.Error(string.Format("目标文件路径为空；源文件路径为：{0}。"));
                return;
            }

            if (!CheckDirectory(GetPathDirectory(targetFilePath, '\\'))) return;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sourceHttpFilePath);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();

                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding(936)))
                {
                    File.WriteAllText(targetFilePath, reader.ReadToEnd().Replace("\r", "\r\n").Replace("\r\n\n", "\r\n"), Encoding.GetEncoding(936));
                }

                responseStream.Close();
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
                return;
            }
        }

        /// <summary>
        /// 非强制删除
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFileBySpecialPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            File.Delete(filePath);
        }

        #region 删除文件夹所有文件

        public static bool DeleteFilesBySpecialPath(string path)
        {
            if (Directory.Exists(path) == false)
            {
                NLogHelper.Equals("文件夹{0}不存在", path);
                return false;
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();
            try
            {
                foreach (var item in files)
                {
                    File.Delete(item.FullName);
                }
                if (dir.GetDirectories().Length != 0)
                {
                    foreach (var item in dir.GetDirectories())
                    {
                        if (!item.ToString().Contains("$") && (!item.ToString().Contains("Boot")))
                        {
                            DeleteFilesBySpecialPath(dir.ToString() + "\\" + item.ToString());
                        }
                    }
                }
                //Directory.Delete(path);

                return true;
            }
            catch
            {
                NLogHelper.Equals("{0}删除异常", path);
                return false;
            }
        }

        #endregion

        public static string GetPathDirectory(string path, Char spitChar)
        {
            return path.Substring(0, path.LastIndexOf(spitChar));
        }

        public static bool CheckDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void CreateText4Bat(List<string> contentBuilder, string FileNamePath)
        {
            #region 写入 bat 文件

            try
            {
                // Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
                var Filepath_Split = FileNamePath.Split(new char[] { '\\' });
                string ExeFullName = Filepath_Split[Filepath_Split.Length - 1];
                string ExeName = ExeFullName.Replace(".exe", "");
                string FileFolder = FileNamePath.TrimEnd(ExeFullName);

                if (!Directory.Exists(FileFolder))
                {
                    Directory.CreateDirectory(FileFolder);
                }

                File.WriteAllText(FileNamePath, String.Empty);
                //Create Stream to write to file.
                StreamWriter sWriter = new StreamWriter(FileNamePath);
                foreach (string cmd in contentBuilder)
                {
                    sWriter.WriteLine(cmd);
                }
                sWriter.Close();
            }
            catch (Exception ex)
            {
                SimpleLogHelper.Error(ex);
                return;
            }

            #endregion
        }

        #region 写主程序
        public static void CreateMainSPF(List<string> contentBuilder, string FileNamePath)
        {
            try
            {
                // Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.
                var Filepath_Split = FileNamePath.Split(new char[] { '\\' });
                string ExeFullName = Filepath_Split[Filepath_Split.Length - 1];
                string ExeName = ExeFullName.Replace(".SPF", "");
                string FileFolder = FileNamePath.TrimEnd(ExeFullName);

                if (!Directory.Exists(FileFolder))
                {
                    Directory.CreateDirectory(FileFolder);
                }

                File.WriteAllText(FileNamePath, String.Empty);
                //Create Stream to write to file.
                StreamWriter sWriter = new StreamWriter(FileNamePath);
                foreach (string cmd in contentBuilder)
                {
                    sWriter.WriteLine(cmd);
                }
                sWriter.Close();
            }
            catch (Exception ex)
            {
                SimpleLogHelper.Error(ex);
                return;
            }
        }
        #endregion

        public static string GetParentPath_byFileName(string path)
        {
            int lastIndex = path.LastIndexOf('\\');
            if (lastIndex != -1)
            {
                return path.Substring(0, lastIndex);
            }
            return "";
        }

    }
}
