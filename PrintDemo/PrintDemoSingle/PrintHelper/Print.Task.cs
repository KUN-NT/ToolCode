using HD.Common.Utils.StringUtils;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PrintDemoSingle
{
    /// <summary>
    /// 打印热轧计划信息
    /// </summary>
    public partial class Print
    {
        public static void Print_Task<T>(List<T> taskItems, string templateName, ref string errorMsg)
        {
            //DownloadTemplate(templateName);
            string expName = Export<T>(taskItems, templateName, ref errorMsg);

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

        private static string Export<T>(List<T> taskItems, string templateName, ref string errorMsg)
        {
            errorMsg = "";
            string expName = "QR_" + Guid.NewGuid().ToString().Replace("-", "") + ".docx";

            var full_doc = new Document();
            var doc_tmp = Single_Exp(taskItems, templateName, expName, ref errorMsg);
            if (doc_tmp == null || doc_tmp.Sections.Count == 0)
            {
                return "";
            }

            foreach (Section sec in doc_tmp.Sections)
            {
                full_doc.Sections.Add(sec.Clone());
            }

            try
            {
                full_doc.SaveToFile(expLocalPath + expName, FileFormat.Docx);

                return expLocalPath + expName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static Document Single_Exp<T>(List<T> taskItems, string templateName, string expName, ref string errorMsg)
        {
            errorMsg = "";

            if (taskItems == null)
            {
                errorMsg = "未找到对应数据，请检查后重试！";
                return null;
            }

            var doc_Exp = new Document();

            File.Copy(tmpLocalPath + templateName, expLocalPath + expName);

            doc_Exp.LoadFromFile(expLocalPath + expName);

            //var dic_Master = new Dictionary<string, string>(0);

            //dic_Master.Add("docx_title", titleName);

            //foreach (var dicItem in dic_Master)
            //{
            //    if (dicItem.Value.IsNullOrWhiteSpace())
            //    {
            //        doc_Exp.Replace("$" + dicItem.Key + "$", "xxx", false, true);
            //    }
            //    else
            //    {
            //        doc_Exp.Replace("$" + dicItem.Key + "$", dicItem.Value, false, true);
            //    }
            //}

            Fill_TableData<T>(doc_Exp, taskItems);

            return doc_Exp;
        }

        private static void Fill_TableData<T>(Document doc_Exp, List<T> taskItems)
        {
            int rowCount = taskItems.Count;
            if (rowCount == 0 || doc_Exp.Sections == null || doc_Exp.Sections.Count == 0)
            {
                return;
            }

            var m_Section = doc_Exp.Sections[0]; 
            if (m_Section.Tables == null || m_Section.Tables.Count == 0)
            {
                return;
            }

            var table = m_Section.Tables[0] as Table;

            // 保存模板行
            var tmp_Row = table.Rows[1].Clone();

            for (int i = 0; i < rowCount; i++)
            {
                var detailItem = taskItems[i];

                var task_Dic = GetTaskDic(detailItem);

                foreach (var dicItem in task_Dic)
                {
                    if (dicItem.Value.IsNullOrWhiteSpace())
                    {
                        doc_Exp.Replace("$" + dicItem.Key + "$", "xxx", false, true);
                    }
                    else
                    {
                        doc_Exp.Replace("$" + dicItem.Key + "$", dicItem.Value, false, true);
                    }
                }

                if (i + 1 < rowCount)
                {
                    table.Rows.Insert(i+2, tmp_Row);
                    tmp_Row = table.Rows[i + 2].Clone();
                }
            }
        }

        private static Dictionary<string, string> GetTaskDic<T>(T detailItem)
        {
            Dictionary<string, string> taskDic = new Dictionary<string, string>();

            Type type = detailItem.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();

            for (int i = 0; i < propertyInfos.Length; i++)
            {
                var tempValue = propertyInfos[i].GetValue(detailItem, null);
                var itemValue = tempValue == null ? "" : tempValue.ToString();
                taskDic.Add(propertyInfos[i].Name, itemValue);
            }

            return taskDic;
        }
    }
}
