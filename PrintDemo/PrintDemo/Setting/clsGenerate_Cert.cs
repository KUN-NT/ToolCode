using PrintDemo.Common;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PrintDemo.Setting
{
    public static class clsGenerate_Cert
    {
        public static string wordTemplate_folderPrefix = @"DownLoad\WordTemplate\"; // word 模板根目录
        public static string templateDoc_Name = "Irradiation_Cert.docx"; // 辐照证明模板
        public static string output_folderPrefix = @"DownLoad\ExpFiles\"; // pdf 生成根目录

        public static void Print(PM_WORK_ORDER masterItem, ref string errorMsg)
        {
            string expName = Export(masterItem, ref errorMsg);

            Document document = new Document();
            document.LoadFromFile(expName);

            //Print doc file.
            var dialog = new PrintDialog();
            dialog.AllowCurrentPage = true;
            dialog.AllowSomePages = true;
            dialog.UseEXDialog = true;
            try
            {
                document.PrintDialog = dialog;
                dialog.Document = document.PrintDocument;
                dialog.Document.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region 生成多页《辐照证明书》，单文件
        public static string Export(PM_WORK_ORDER masterItem, ref string errorMsg)
        {
            List<PM_WORK_ORDER> list_masterItem = new List<PM_WORK_ORDER>();
            list_masterItem.Add(masterItem);
            errorMsg = "";

            var full_doc = new Document();

            for (int i = 0; i < list_masterItem.Count; i++)
            {
                var doc_tmp = Single_Exp(list_masterItem[i], ref errorMsg);
                if (doc_tmp == null || doc_tmp.Sections.Count == 0)
                {
                    continue;
                }

                foreach (Section sec in doc_tmp.Sections)
                {
                    full_doc.Sections.Add(sec.Clone());
                }
            }

            string expName = AppDomain.CurrentDomain.BaseDirectory + output_folderPrefix + "QR_" + GuidHelper.NewPureGuid() + ".docx";

            // (5)文件导出为 docx
            try
            {
                full_doc.SaveToFile(expName, FileFormat.Docx);

                return expName;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);

                return "";
            }
        }

        #endregion

        #region 生成单页《辐照证明书》，一次生成并打开一个

        /// <summary>
        /// 生成《辐照证明书》，一次生成一个。
        /// (1)子项默认一种，故只有大于1种才需要表格动态加行
        /// 单页生成方法
        /// </summary>
        /// <param name="m_Irradiation_CertMaster"></param>
        /// <returns></returns>
        private static Document Single_Exp(PM_WORK_ORDER masterItem, ref string errorMsg)
        {
            errorMsg = "";

            var dic_Master = new Dictionary<string, string>(0);

            dic_Master.Add("wip_id", masterItem.WIP_ID);

            string templateDoc_Path = AppDomain.CurrentDomain.BaseDirectory + wordTemplate_folderPrefix + templateDoc_Name;

            // (1)读取 docx 模板
            var doc_Exp = new Document();

            string expName = AppDomain.CurrentDomain.BaseDirectory + output_folderPrefix + "QR_" + GuidHelper.NewPureGuid() + ".docx";
            File.Copy(templateDoc_Path, expName);

            doc_Exp.LoadFromFile(expName);

            // (2)文字替换
            foreach (var dicItem in dic_Master)
            {
                if (dicItem.Value.IsNullOrWhiteSpace())
                {
                    doc_Exp.Replace("$" + dicItem.Key + "$", "", false, true);
                }
                else
                {
                    doc_Exp.Replace("$" + dicItem.Key + "$", dicItem.Value, false, true);
                }
            }

            return doc_Exp;
        }

        #endregion
    }
}
