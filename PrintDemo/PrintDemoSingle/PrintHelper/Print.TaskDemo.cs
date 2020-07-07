//using HD.Common.Utils.StringUtils;
//using Spire.Doc;
//using Spire.Doc.Documents;
//using Spire.Doc.Fields;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using System.Windows.Forms;

//namespace HD.WPF.PPM.Plan
//{
//    /// <summary>
//    /// 打印热轧计划信息
//    /// </summary>
//    public partial class Print
//    {
//        public static void Print_Task<T>(List<T> taskItems, string titleName, string templateName, ref string errorMsg)
//        {
//            DownloadTemplate(templateName);
//            string expName = Export<T>(taskItems, titleName, templateName, ref errorMsg);

//            Document document = new Document();
//            document.LoadFromFile(expName);

//            //Print doc file.
//            var dialog = new PrintDialog();
//            dialog.AllowCurrentPage = true;
//            dialog.AllowSomePages = true;
//            dialog.UseEXDialog = true;
//            try
//            {
//                document.PrintDialog = dialog;
//                dialog.Document = document.PrintDocument;
//                dialog.Document.Print();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private static string Export<T>(List<T> taskItems, string titleName, string templateName, ref string errorMsg)
//        {
//            errorMsg = "";
//            string expName = "QR_" + Guid.NewGuid().ToString().Replace("-", "") + ".docx";

//            var full_doc = new Document();
//            var doc_tmp = Single_Exp(taskItems, titleName, templateName, expName, ref errorMsg);
//            if (doc_tmp == null || doc_tmp.Sections.Count == 0)
//            {
//                return "";
//            }

//            foreach (Section sec in doc_tmp.Sections)
//            {
//                full_doc.Sections.Add(sec.Clone());
//            }

//            try
//            {
//                full_doc.SaveToFile(expLocalPath + expName, FileFormat.Docx);

//                return expLocalPath + expName;
//            }
//            catch (Exception ex)
//            {
//                return "";
//            }
//        }

//        private static Document Single_Exp<T>(List<T> taskItems, string titleName, string templateName, string expName, ref string errorMsg)
//        {
//            errorMsg = "";

//            if (taskItems == null)
//            {
//                errorMsg = "未找到对应数据，请检查后重试！";
//                return null;
//            }

//            var doc_Exp = new Document();

//            File.Copy(tmpLocalPath + templateName, expLocalPath + expName);

//            doc_Exp.LoadFromFile(expLocalPath + expName);

//            var dic_Master = new Dictionary<string, string>(0);

//            dic_Master.Add("docx_title", titleName);

//            foreach (var dicItem in dic_Master)
//            {
//                if (dicItem.Value.IsNullOrWhiteSpace())
//                {
//                    doc_Exp.Replace("$" + dicItem.Key + "$", "xxx", false, true);
//                }
//                else
//                {
//                    doc_Exp.Replace("$" + dicItem.Key + "$", dicItem.Value, false, true);
//                }
//            }

//            Fill_TableData<T>(doc_Exp, taskItems);

//            return doc_Exp;
//        }

//        private static void Fill_TableData<T>(Document doc_Exp, List<T> taskItems)
//        {
//            int rowCount = taskItems.Count;
//            if (rowCount == 0 || doc_Exp.Sections == null || doc_Exp.Sections.Count == 0)
//            {
//                return;
//            }

//            var m_Section = doc_Exp.Sections[0]; // 模板只有1个 section
//            if (m_Section.Tables == null || m_Section.Tables.Count == 0)
//            {
//                return;
//            }

//            var table = m_Section.Tables[0] as Table;

//            // (1)默认只有一行记录，大于一行需先插入缺失行
//            for (int i = 1; i < rowCount; i++)
//            {
//                var m_Row = table.Rows[2].Clone(); // 注意 row 没有 cell 时，table 不会添加行

//                table.Rows.Insert(3, m_Row);
//            }

//            // (2)按行循环添加 Cell、对 Cell 填入数值
//            for (int i = 0; i < rowCount; i++)
//            {
//                var detailItem = taskItems[i];

//                var m_Row = table.Rows[i + 2];  //  table.Rows[2] 为 表头行，table.Rows[3] 为 货物明细数据

//                var task_Dic = GetTaskDic(detailItem);

//                for (int j = 0; j < m_Row.Cells.Count; j++)
//                {
//                    var m_Cell = m_Row.Cells[j];

//                    Paragraph m_Paragraph = null;

//                    if (m_Cell.Paragraphs.Count == 0)
//                    {
//                        m_Paragraph = m_Cell.AddParagraph();
//                    }

//                    if (m_Cell.Paragraphs.Count > 0)
//                    {
//                        m_Paragraph = m_Cell.Paragraphs[0];
//                    }

//                    m_Paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;

//                    TextRange m_TextRange = null;

//                    m_TextRange = m_Paragraph.AppendText(task_Dic[j]);

//                    if (m_TextRange != null)
//                    {
//                        m_TextRange.CharacterFormat.FontSize = 9;
//                    }
//                }
//            }
//        }

//        //获取属性值
//        //打印模板列的排列顺序需要和视图字段顺序对应
//        //数字和日期数据在创建视图的时候做格式化处理
//        private static Dictionary<int, string> GetTaskDic<T>(T detailItem)
//        {
//            Dictionary<int, string> taskDic = new Dictionary<int, string>();

//            Type type = detailItem.GetType();
//            PropertyInfo[] propertyInfos = type.GetProperties();

//            for (int i = 1; i < propertyInfos.Length; i++)
//            {
//                var tempValue = propertyInfos[i].GetValue(detailItem, null);
//                var itemValue = tempValue == null ? "" : tempValue.ToString();
//                taskDic.Add(i - 1, itemValue);
//            }

//            return taskDic;
//        }
//    }
//}
