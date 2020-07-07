using System.Collections.Generic;
using System.Data;

namespace PrintDemo.DataBase
{
    public static class DataSetExtension
    {
        public static bool HasRows(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return false;
            }

            return ds.Tables[0].Rows.Count > 0;
        }

        #region 复制属性

        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="target">目前数据</param>
        /// <param name="columns">目标字段</param>
        public static void CopyProperties(this DataRow source, DataRow target, List<string> columns)
        {
            if (columns == null || columns.Count == 0)
            {
                return;
            }

            string col_Name = "";

            for (int i = 0; i < columns.Count; i++)
            {
                col_Name = columns[i];

                if (string.IsNullOrWhiteSpace(col_Name))
                {
                    continue;
                }

                col_Name = col_Name.Trim().ToUpper();

                if (source.Table.Columns.Contains(col_Name) && target.Table.Columns.Contains(col_Name))
                {
                    if (source[col_Name] != null)
                    {
                        target[col_Name] = source[col_Name];
                    }
                    else
                    {
                        target[col_Name] = "";
                    }
                }
            }

        }

        /// <summary>
        /// 全表复制
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyProperties(this DataRow source, DataRow target)
        {

            string col_Name = "";

            for (int i = 0; i < source.Table.Columns.Count; i++)
            {
                col_Name = source.Table.Columns[i].ColumnName;

                if (string.IsNullOrWhiteSpace(col_Name))
                {
                    continue;
                }

                col_Name = col_Name.Trim().ToUpper();

                if (target.Table.Columns.Contains(col_Name))
                {
                    if (source[col_Name] != null)
                    {
                        target[col_Name] = source[col_Name];
                    }
                    else
                    {
                        target[col_Name] = "";
                    }
                }
            }
        }

        /// <summary>
        /// 根据 source 复制到 target，复制 目标数据包含的字段
        /// </summary>
        /// <param name="source">源数据</param>
        /// <param name="target">目前数据</param>
		/// <param name="avoid_columns">不复制的字段</param>
        public static void CopyProperties4Avoids(this DataRow source, DataRow target, List<string> avoid_columns)
        {

            string col_Name = "";

            for (int i = 0; i < source.Table.Columns.Count; i++)
            {
                col_Name = source.Table.Columns[i].ColumnName;

                if (string.IsNullOrWhiteSpace(col_Name))
                {
                    continue;
                }

                col_Name = col_Name.Trim().ToUpper();

                if (target.Table.Columns.Contains(col_Name) && !avoid_columns.Contains(col_Name))
                {
                    if (source[col_Name] != null)
                    {
                        target[col_Name] = source[col_Name];
                    }
                    else
                    {
                        target[col_Name] = "";
                    }
                }
            }

        }


        /// <summary>
        /// 复制时间属性
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyLogProperties(this DataRow source, DataRow target)
        {
            var columns = new List<string>() { "CREATED_BY", "CREATION_DATE", "UPDATED_BY", "LAST_UPDATE_DATE" };

            CopyProperties(source, target, columns);
        }

        #endregion

    }
}
