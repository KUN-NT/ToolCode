using System;
using System.Collections.Generic;
using System.IO;

namespace PrintDemo.Common
{
    public class FileTxt
    {

        #region 从 strSrc_File 文件 安全加载“实时信息”，位于文本文件第一行

        /// <summary>
        /// 从 strSrc_File 文件 安全加载“实时信息”，位于文本文件第一行
        /// </summary>
        /// <param name="strSrc_File"></param>
        /// <param name="strTarget_File"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static string GetLine0_4Txt(string strSrc_File, string strTarget_File, ref string errorMsg)
        {
            errorMsg = "";

            try
            {
                var file_status = FileExt.Check_Status(strSrc_File);
                switch (file_status)
                {
                    case File_Status.NonExisting:

                        errorMsg = "数据文件不存在";

                        return "";

                    default:
                        break;
                }

                File.Copy(strSrc_File, strTarget_File, true);

                var lines = File.ReadAllLines(strTarget_File);
                if (lines == null || lines.Length == 0)
                {
                    return "";
                }

                string strRtn = lines[0];

                return strRtn;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return "";
        }

        #endregion

        #region 创建文件夹

        public static void Safe_CreateDirectory(string strDir)
        {
            if (string.IsNullOrWhiteSpace(strDir))
            {
                return;
            }

            if (!Directory.Exists(strDir))
            {
                Directory.CreateDirectory(strDir);
            }
        }

        /// <summary>
        /// 批量创建文件夹
        /// </summary>
        /// <param name="list_Dirs"></param>
        public static void Safe_CreateDirectories(List<string> list_Dirs)
        {
            if (list_Dirs == null || list_Dirs.Count == 0)
            {
                return;
            }

            foreach (var strDir in list_Dirs)
            {
                Safe_CreateDirectory(strDir);
            }
        }

        #endregion

        #region 递归删除文件夹下所有文件

        /// <summary>
        /// 递归删除文件夹下所有文件
        /// </summary>
        /// <param name="strDir"></param>
        public static void Del_Files4Dir(string strDir)
        {
            if (strDir.IsNullOrWhiteSpace())
            {
                return;
            }

            if (!Directory.Exists(strDir))
            {
                return;
            }

            var full_Names = GetAll_Files4Dir(strDir);
            for (int i = 0; i < full_Names.Length; i++)
            {
                try
                {
                    var m_File_Status = FileExt.Check_Status(full_Names[i]);
                    if (m_File_Status == File_Status.Unlocked)
                    {
                        File.Delete(full_Names[i]);
                    }
                }
                catch (Exception ex)
                {
                    NLogHelper.Error(ex);
                }
            }
        }

        #endregion

        #region Safe_WriteAllText

        /// <summary>
        /// Safe_WriteAllText
        /// </summary>
        /// <param name="file_path"></param>
        /// <param name="file_contents"></param>
        public static void Safe_WriteAllText(string file_path, string file_contents)
        {
            string strParent_Dir = FileHelper.GetPathDirectory(file_path, '\\');

            Safe_CreateDirectory(strParent_Dir);

            if (!File.Exists(file_path))
            {
                File.WriteAllText(file_path, file_contents);
                return;
            }

            // 只有文件内容不同，才写入文件
            string old_Str = File.ReadAllText(file_path);

            if (old_Str != file_contents)
            {
                File.WriteAllText(file_path, file_contents);
            }
        }

        #endregion

        #region Safe_ReadAllText

        /// <summary>
        /// Safe_ReadAllText，先复制再读取信息
        /// </summary>
        /// <param name="strSrc_File"></param>
        /// <param name="strTarget_File"></param>
        public static string Safe_ReadAllText(string strSrc_File, string strTarget_File)
        {
            try
            {
                var file_status = FileExt.Check_Status(strSrc_File);
                switch (file_status)
                {
                    case File_Status.NonExisting:
                        return "";

                    default:
                        break;
                }

                File.Copy(strSrc_File, strTarget_File, true);

                string strRtn = File.ReadAllText(strTarget_File);

                return strRtn;
            }
            catch (Exception ex)
            {

            }

            return "";
        }

        #endregion

        #region 递归获取“目录下的所有文件”

        /// <summary>
        /// 递归获取“目录下的所有文件”
        /// </summary>
        /// <param name="strDir"></param>
        /// <returns></returns>
        public static string[] GetAll_Files4Dir(string strDir)
        {
            if (strDir.IsNullOrWhiteSpace())
            {
                return new string[] { };
            }

            var file_Names = Directory.GetFiles(strDir, "*.*", SearchOption.AllDirectories);

            return file_Names;
        }

        #endregion

    }
}
