using PrintDemo.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintDemo.DataBase
{
    public partial class OraAccess
    {

        #region 执行参数化查询

        /// <summary>
        /// 执行参数化查询
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DataSet SelectByPara_SQL(FilterData filter, bool isNvarChar = true)
        {
            string strSQL = filter.FilterExpression;

            this.oraCmd.CommandText = strSQL;

            this.oraCmd.CommandType = CommandType.Text;

            var ds = new DataSet();

            var parameterNames = filter.ParameterNames;

            var parameterValues = filter.ParameterValues;

            this.oraCmd.Parameters.Clear();
            for (int i = 0; i < parameterNames.Count; i++)
            {
                var parameter = new OracleParameter(parameterNames[i], parameterValues[i]);

                if (isNvarChar)
                {
                    parameter.OracleType = OracleType.NVarChar;
                }

                this.oraCmd.Parameters.Add(parameter);
            }

            this.oraAdapter.SelectCommand = this.oraCmd;

            try
            {
                this.oraAdapter.Fill(ds);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);

                ds = null;
            }
            return ds;
        }

        #endregion

        #region 执行查询 sql

        /// <summary>
        /// 执行查询 sql
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public override DataSet SelectBySQL(string strSQL)
        {
            this.oraCmd.CommandText = strSQL;

            this.oraCmd.CommandType = CommandType.Text;

            var ds = new DataSet();
            this.oraCmd.Parameters.Clear();
            this.oraAdapter.SelectCommand = this.oraCmd;
            try
            {
                this.oraAdapter.Fill(ds);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);

                ds = null;
            }
            return ds;
        }

        /// <summary>
        /// 参数化查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where_filter"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public override DataSet SelectBySQL(string tableName, string where_filter, List<string> parameterNames, List<object> parameterValues)
        {
            DataSet ds = new DataSet();
            this.oraAdapter = new OracleDataAdapter();

            string strSQL = "";
            if (where_filter != "")
            {
                strSQL = "select * from " + tableName + " where " + where_filter;
            }
            else
            {
                strSQL = "select * from " + tableName;
            }

            this.oraCmd.CommandType = CommandType.Text;

            this.oraCmd.CommandText = strSQL;
            this.oraCmd.Parameters.Clear();
            for (int i = 0; i < parameterNames.Count; i++)
            {
                var parameter = new OracleParameter(parameterNames[i], parameterValues[i]);
                this.oraCmd.Parameters.Add(parameter);
            }

            this.oraAdapter.SelectCommand = this.oraCmd;

            try
            {
                this.oraAdapter.Fill(ds);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
            return ds;
        }


        /// <summary>
        /// 执行带参数的查询语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public override DataSet SelectBySQL(string tableName, FilterData filter)
        {
            return SelectBySQL(tableName, filter.FilterExpression, filter.ParameterNames, filter.ParameterValues);
        }

        #endregion

        public bool IsExist(string tableName, string where_filter, List<string> parameterNames, List<object> parameterValues)
        {
            var ds = new DataSet();
            this.oraAdapter = new OracleDataAdapter();

            string strSQL = "";
            if (where_filter != "")
            {
                strSQL = "select count(*) from " + tableName + " where " + where_filter;
            }
            else
            {
                strSQL = "select count(*) from " + tableName;
            }

            this.oraCmd.CommandType = CommandType.Text;

            this.oraCmd.CommandText = strSQL;
            this.oraCmd.Parameters.Clear();
            for (int i = 0; i < parameterNames.Count; i++)
            {
                var parameter = new OracleParameter(parameterNames[i], parameterValues[i]);
                this.oraCmd.Parameters.Add(parameter);
            }

            this.oraAdapter.SelectCommand = this.oraCmd;

            try
            {
                this.oraAdapter.Fill(ds);
                int nCount = 0;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (Int32.TryParse(StringHelper.SafeToString(ds.Tables[0].Rows[0][0]), out nCount))
                    {
                        if (nCount > 0)
                        {
                            return true;  // 该表已存在重复记录
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }

            return false;
        }


    }
}
