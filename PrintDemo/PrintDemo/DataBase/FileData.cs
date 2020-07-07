using PrintDemo.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PrintDemo.DataBase
{
    [Serializable]
    public class FilterData
    {
        public FilterData()
        {
            this._filterExpression = "1=1";

            this._parameterNames = new List<string>(0);

            this._parameterValues = new List<object>(0);

            this._compareTypes = new List<CompareType>(0);
        }

        public FilterData(List<string> parameterNames, List<object> parameterValues)        //--页面启动
        {
            this._filterExpression = "1=1";

            this._parameterNames = parameterNames;
            this._parameterValues = parameterValues;

        }

        public FilterData(string objBase64String)  //--序列化启动参数
        {
            DeSerialData(objBase64String);
        }

        private string _filterExpression;
        public string FilterExpression
        {
            set
            {
                this._filterExpression = value;
            }
            get
            {
                return this._filterExpression;
            }
        }

        private List<string> _parameterNames;
        public List<string> ParameterNames
        {
            set
            {
                this._parameterNames = value;
            }
            get
            {
                return this._parameterNames;
            }
        }

        private List<object> _parameterValues;
        public List<object> ParameterValues
        {
            set
            {
                this._parameterValues = value;
            }
            get
            {
                return this._parameterValues;
            }
        }

        private List<CompareType> _compareTypes;
        public List<CompareType> CompareTypes
        {
            set
            {
                this._compareTypes = value;
            }
            get
            {
                return this._compareTypes;
            }
        }

        public static void CheckFilterValue(ref string strValue)
        {
            if (strValue.IndexOf("'") > -1)
            {
                strValue = strValue.Replace("'", "''");
            }
        }

        #region 附加筛选条件

        /// <summary>
        /// 不适合附加 isnotnull isnull
        /// </summary>
        /// <param name="columnName">字段名</param>
        /// <param name="paramName">参数名</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="filterCompare"></param>
        /// <param name="filterLink"></param>
        public void AppendFilter(string columnName, string paramName, object paramValue, CompareType filterCompare, LinkType filterLink)
        {
            if (paramValue.SafeToString().IsNullOrWhiteSpace())
            {
                return;
            }

            string linkString = "";
            switch (filterLink)
            {
                case LinkType.AND:
                    linkString = " AND ";
                    break;
                case LinkType.OR:
                    linkString = " OR ";
                    break;
                default:
                    break;
            }
            ParameterNames.Add(paramName);

            if (this._filterExpression == "1=1")
            {
                linkString = "";
                this._filterExpression = "";
            }

            switch (filterCompare)
            {
                case CompareType.IsEqual:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} = :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(paramValue);
                    break;
                case CompareType.IsNull:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} is null", linkString, paramName);
                    ParameterValues.Add(paramValue);
                    break;
                case CompareType.IsNotNull:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} is not null", linkString, paramName);
                    ParameterValues.Add(paramValue);
                    break;
                case CompareType.StartWith:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} like :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(paramValue.ToString() + "%");
                    break;
                case CompareType.EndWith:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} like :{2}", linkString, columnName, paramName);
                    ParameterValues.Add("%" + paramValue.ToString());
                    break;
                case CompareType.MoreThan:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} > :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(paramValue);
                    break;
                case CompareType.LessThan:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} < :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(paramValue);
                    break;
                case CompareType.NoLessThan:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} >= :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(paramValue);
                    break;
                case CompareType.NoMoreThan:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} <= :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(paramValue);
                    break;
                case CompareType.Contain:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} like :{2}", linkString, columnName, paramName);
                    ParameterValues.Add("%" + paramValue.ToString() + "%");
                    break;
                case CompareType.UnEqual:
                    this._filterExpression = this._filterExpression + string.Format("{0}{1} <> :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(paramValue);
                    break;
                default:
                    break;
            }

            CompareTypes.Add(filterCompare);

        }

        #endregion

        /// <summary>
        /// 连接 filter1 和 filter2；不能用于 inFilter
        /// </summary>
        /// <param name="filter1"></param>
        /// <param name="filter2"></param>
        /// <param name="filterCompare"></param>
        /// <param name="filterLink"></param>
        public static void ConcatNoneDateFilter(ref FilterData filter1, FilterData filter2, LinkType filterLink)
        {
            for (int i = 0; i < filter2.ParameterNames.Count; i++)
            {
                filter1.AppendFilter(filter2.ParameterNames[i], filter2.ParameterNames[i], filter2.ParameterValues[i], filter2.CompareTypes[i], filterLink);
            }
        }

        #region 附加日期类型查询条件

        public void AppendDateFilter(string columnName, string paramName, object paramValue, CompareType filterCompare, LinkType filterLink)
        {
            if (paramValue.SafeToString().IsNullOrWhiteSpace())
            {
                return;
            }

            string linkString = "";
            switch (filterLink)
            {
                case LinkType.AND:
                    linkString = " AND ";
                    break;
                case LinkType.OR:
                    linkString = " OR ";
                    break;
                default:
                    break;
            }
            ParameterNames.Add(paramName);

            if (this._filterExpression == "1=1")
            {
                linkString = "";
                this._filterExpression = "";
            }

            string strDateTime = Convert.ToDateTime(paramValue).ToString("yyyy-MM-dd"); // Convert.ToDateTime(paramValue).AddDays(1).ToString();

            switch (filterCompare)
            {
                case CompareType.MoreThan: // >
                    this._filterExpression = this._filterExpression + string.Format("{0}TO_CHAR({1},'yyyy-MM-dd') > :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(strDateTime);
                    break;

                case CompareType.NoLessThan: // >=

                    // _filterExpression = string.Format("TO_DATE({0}{1},'yyyy-MM-dd')", ":", paramName);

                    this._filterExpression = this._filterExpression + string.Format("{0}TO_CHAR({1},'yyyy-MM-dd') >= :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(paramValue);
                    break;

                case CompareType.IsEqual: // =
                    this._filterExpression = this._filterExpression + string.Format("{0}TO_CHAR({1},'yyyy-MM-dd') >= :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(paramValue);

                    ParameterNames.Add("To" + paramName);
                    this._filterExpression = this._filterExpression + " AND " + string.Format("{0}TO_CHAR({1},'yyyy-MM-dd') = :{2}", linkString, columnName, "To" + paramName);
                    ParameterValues.Add(strDateTime);
                    break;

                case CompareType.LessThan: // <
                    this._filterExpression = this._filterExpression + string.Format("{0}TO_CHAR({1},'yyyy-MM-dd') < :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(paramValue);
                    break;

                case CompareType.NoMoreThan: // <=
                    this._filterExpression = this._filterExpression + string.Format("{0}TO_CHAR({1},'yyyy-MM-dd') <= :{2}", linkString, columnName, paramName);
                    ParameterValues.Add(strDateTime);
                    break;

                default:
                    break;
            }

            CompareTypes.Add(filterCompare);

        }

        #endregion

        #region 获取多项信息查询表达式
        public void GetMultiSelectData(string[] keyNames, List<Hashtable> tbDataKey)
        {
            this._filterExpression = "";
            this.ParameterNames = new List<string>(0);
            this.ParameterValues = new List<object>(0);

            for (int i = 0; i < tbDataKey.Count; i++)
            {
                string temp = "(";
                for (int j = 0; j < keyNames.Length; j++)
                {
                    string colName = keyNames[j];
                    string paramName = keyNames[j] + i.ToString();
                    this.ParameterNames.Add(paramName);
                    this.ParameterValues.Add(tbDataKey[i][colName]);
                    if (j == keyNames.Length - 1)
                    {
                        temp = temp + string.Format("{0} = :{1}", colName, paramName);
                    }
                    else
                    {
                        temp = temp + string.Format("{0} = :{1} and ", colName, paramName);
                    }
                }
                temp = temp + ")";
                if (i == tbDataKey.Count - 1)
                {
                    this._filterExpression = this._filterExpression + temp;
                }
                else
                {
                    this._filterExpression = this._filterExpression + temp + " or ";
                }
            }
            if (this._filterExpression == "")
            {
                this._filterExpression = "1=0";
            }
        }
        #endregion

        #region 为外键字段赋值
        public void SetForeignKey(string keyCode, object dataRow)
        {
            this.DeSerialData(keyCode);
            for (int i = 0; i < this._parameterNames.Count; i++)
            {
                PropertyInfo property = dataRow.GetType().GetProperty(this._parameterNames[i]);
                if (property != null)
                {
                    property.SetValue(dataRow, this._parameterValues[i], null);
                }
            }
        }
        #endregion

        #region 序列化信息

        /// <summary>
        /// encode filter data
        /// </summary>
        /// <returns></returns>
        public string GetBase64String()
        {
            using (var m_MemoryStream = new MemoryStream())
            {
                BinaryFormatter m_BinaryFormatter = new BinaryFormatter();
                m_BinaryFormatter.Serialize(m_MemoryStream, this);
                m_MemoryStream.Position = 0;
                byte[] content = new byte[m_MemoryStream.Length];
                m_MemoryStream.Read(content, 0, content.Length);
                m_MemoryStream.Close();

                return Convert.ToBase64String(content);
            }
        }

        /// <summary>
        /// encode filter data
        /// </summary>
        /// <returns></returns>
        public byte[] SerializeData()
        {
            using (var m_MemoryStream = new MemoryStream())
            {
                BinaryFormatter m_BinaryFormatter = new BinaryFormatter();
                m_BinaryFormatter.Serialize(m_MemoryStream, this);
                m_MemoryStream.Position = 0;
                byte[] content = new byte[m_MemoryStream.Length];
                m_MemoryStream.Read(content, 0, content.Length);
                m_MemoryStream.Close();

                return content;
            }
        }

        #endregion

        #region 反序列化查询信息

        /// <summary>
        /// 反序列化查询信息
        /// </summary>
        /// <param name="objBase64String"></param>
        public void DeSerialData(string objBase64String)
        {

            byte[] Content = Convert.FromBase64String(objBase64String);
            MemoryStream ms = new MemoryStream();
            try
            {
                ms.Write(Content, 0, Content.Length);
                ms.Position = 0;

                BinaryFormatter m_BinaryFormatter = new BinaryFormatter();
                FilterData obj = (FilterData)m_BinaryFormatter.Deserialize(ms);
                ms.Close();

                this._filterExpression = obj.FilterExpression;
                this._parameterNames = obj.ParameterNames;
                this._parameterValues = obj.ParameterValues;
                this._compareTypes = obj.CompareTypes;
            }
            catch (Exception ex)
            {
                ms.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 反序列化查询信息
        /// </summary>
        /// <param name="Content"></param>
        public void DeSerialData(byte[] Content)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                ms.Write(Content, 0, Content.Length);
                ms.Position = 0;

                BinaryFormatter m_BinaryFormatter = new BinaryFormatter();
                FilterData obj = (FilterData)m_BinaryFormatter.Deserialize(ms);
                ms.Close();

                this._filterExpression = obj.FilterExpression;
                this._parameterNames = obj.ParameterNames;
                this._parameterValues = obj.ParameterValues;
                this._compareTypes = obj.CompareTypes;
            }
            catch (Exception ex)
            {
                ms.Close();
                throw ex;
            }
        }
        #endregion

        #region 根据主键信息获取查询字符串

        /// <summary>
        /// 根据主键信息获取查询字符串
        /// </summary>
        /// <returns></returns>
        public string GetSqlQueryString()
        {
            string sqlQuery = "";
            for (int i = 0; i < this.ParameterNames.Count; i++)
            {
                string columnName = this.ParameterNames[i];
                sqlQuery = sqlQuery + string.Format("{0}='{1}'", columnName, this.ParameterValues[i].ToString());
                if (i < this.ParameterNames.Count - 1)
                {
                    sqlQuery = sqlQuery + " and ";
                }
            }
            return sqlQuery;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetFilterValueString()
        {
            StringBuilder sb = new StringBuilder("");
            foreach (var item in this.ParameterValues)
            {
                sb.Append(item + ",");
            }
            return sb.ToString();
        }


        #region 获取双字段 filter string，未考虑 sql 注入

        /// <summary>
        /// 获取双字段 filter string，未考虑 sql 注入
        /// " (TASK_ID='LF0HGCT21H0051479' and ORDER_ID='N170429910') or (TASK_ID='LF0HGCT28H0051480' and ORDER_ID='N170429908') "
        /// </summary>
        /// <param name="colName1"></param>
        /// <param name="Values1"></param>
        /// <param name="colName2"></param>
        /// <param name="Values2"></param>
        /// <returns></returns>
        public static string Get2ColsFilterStr(string colName1, List<string> Values1, string colName2, List<string> Values2)
        {
            if (Values1.Count == 0 || Values2.Count == 0)
            {
                return " 1=0 ";
            }

            var sb = new StringBuilder(" ");
            string sqlQuery = "";
            for (int i = 0; i < Values1.Count; i++)
            {
                sqlQuery = sqlQuery + string.Format(" ({0}='{1}' and {2}='{3}') ", colName1, Values1[i].ToString(), colName2, Values2[i].ToString());
                if (i < Values1.Count - 1)
                {
                    sqlQuery = sqlQuery + " or ";
                }
            }

            return sqlQuery;
        }


        #endregion

        #region 为单字段构造 status in ("a","b","c")

        /// <summary>
        /// 为单字段构造 status in ("a","b","c")
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="inValues"></param>
        /// <param name="strInOrNot">" not in ",  " in "</param>
        /// <returns></returns>
        public static string GetInFilterString4singleColName(string colName, List<string> inValues, string strInOrNot = " in ")
        {
            if (inValues.Count == 0)
            {
                return "";
            }

            var sb = new StringBuilder(" " + colName + strInOrNot + " ( ");

            for (int i = 0; i < inValues.Count; i++)
            {
                sb.Append("'" + inValues[i] + "',");
            }

            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.ToString().Length - 1, 1);  // 去除多余的,
            }

            sb.Append(" ) ");
            return sb.ToString();
        }

        /// <summary>
        /// 为单字段构造 colName in ("a","b","c")
        /// </summary>
        /// <param name="colName">用于筛选的字段</param>
        /// <returns></returns>
        public static FilterData GetInFilter4singleColName(string colName, string[] inValues)
        {
            var filter = new FilterData();
            if (inValues.Length == 0)
            {
                return filter;
            }

            StringBuilder sb = new StringBuilder(colName + " IN (");
            for (int i = 0; i < inValues.Length; i++)
            {
                sb.Append(":" + colName + i.ToString() + ",");
                filter.ParameterNames.Add(colName + i.ToString());
                filter.ParameterValues.Add(inValues[i]);
            }

            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.ToString().Length - 1, 1);  // 去除多余的,
            }

            sb.Append(") ");
            filter.FilterExpression = sb.ToString();
            return filter;
        }

        /// <summary>
        /// 注意 inValues.Count == 0 时，filter.FilterExpression 为 1=1，直接使用会把所有行查询出来
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="inValues"></param>
        /// <returns></returns>
        public static FilterData GetInFilter4singleColName(string colName, List<string> inValues)
        {
            FilterData filter = new FilterData();
            if (inValues.Count == 0)
            {
                return filter;
            }

            var sb = new StringBuilder(colName + " IN (");
            for (int i = 0; i < inValues.Count; i++)
            {
                sb.Append(":" + colName + i.ToString() + ",");
                filter.ParameterNames.Add(colName + i.ToString());
                filter.ParameterValues.Add(inValues[i]);
            }

            if (sb.ToString().EndsWith(","))
            {
                sb.Remove(sb.ToString().Length - 1, 1);  // 去除多余的,
            }

            sb.Append(") ");
            filter.FilterExpression = sb.ToString();
            return filter;
        }

        #endregion


        public static FilterData GetOrgFilter(List<string> orgItems)
        {
            FilterData filter = new FilterData();
            StringBuilder sb = new StringBuilder("ORG_ID IN (");

            for (int i = 0; i < orgItems.Count; i++)
            {
                sb.Append(":ORG_ID" + i.ToString() + ",");
                filter.ParameterNames.Add("ORG_ID" + i.ToString());
                filter.ParameterValues.Add(orgItems[i]);
            }
            sb.Append("' ') OR ORG_ID IS NULL");
            filter.FilterExpression = sb.ToString();
            return filter;
        }

        /// <summary>
        /// 用于批量删除
        /// </summary>
        /// <param name="listFilter"></param>
        /// <param name="m_LinkType"></param>
        /// <returns></returns>
        public static FilterData List2Filter(List<FilterData> listFilter, LinkType m_LinkType)
        {
            StringBuilder sb = new StringBuilder();
            FilterData filter = new FilterData();
            string str_LinkType = "";

            if (listFilter.Count == 0)
            {
                return filter;
            }

            if (listFilter.Count == 1)
            {
                return listFilter[0];
            }

            if (m_LinkType == LinkType.AND)
            {
                str_LinkType = " and ";
            }
            if (m_LinkType == LinkType.OR)
            {
                str_LinkType = " or ";
            }

            for (int i = 0; i < listFilter.Count; i++)
            {
                sb.Append(listFilter[i].FilterExpression);
                sb.Append(str_LinkType);

                for (int j = 0; j < listFilter[i].ParameterNames.Count; j++)
                {
                    filter.ParameterNames.Add(listFilter[i].ParameterNames[j]);
                    filter.ParameterValues.Add(listFilter[i].ParameterValues[j]);
                    filter.CompareTypes.Add(listFilter[i].CompareTypes[j]);
                }
            }

            if (sb.ToString().EndsWith(str_LinkType))
            {
                sb.Remove(sb.ToString().Length - 1 - str_LinkType.Length, str_LinkType.Length);
            }

            filter.FilterExpression = sb.ToString();

            return filter;
        }

    }

    public enum LinkType
    {
        AND,
        OR
    }

    [ComVisibleAttribute(true)]
    public enum CompareType
    {
        IsEqual = 1,
        MoreThan = 2,
        NoLessThan = 3,
        LessThan = 4,
        NoMoreThan = 5,
        IsNull,
        IsNotNull,
        StartWith,
        EndWith,
        Contain,
        UnEqual,
        Default // 如 in filter
    }
}
