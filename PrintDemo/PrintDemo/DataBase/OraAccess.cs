using PrintDemo.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Text;

namespace PrintDemo.DataBase
{
    [Serializable]
	public partial class OraAccess : GeneralAccess
	{

		public OracleConnection oraCon;
		private OracleCommand oraCmd;
		private OracleDataAdapter oraAdapter;
		private OracleCommandBuilder builder;

		public OraAccess(string ConStr)
		{
			oraCon = new OracleConnection(ConStr);
			oraCmd = new OracleCommand();
			oraCmd.Connection = oraCon;
			oraAdapter = new OracleDataAdapter();
			oraAdapter.SelectCommand = oraCmd;
		}

		public OraAccess(OraConnection Con)
		{
			oraCon = new OracleConnection(Con.ConStr);
			oraCmd = new OracleCommand();
			oraCmd.Connection = oraCon;
			oraAdapter = new OracleDataAdapter();
			oraAdapter.SelectCommand = oraCmd;
		}
		#region 获取 Insert/Update/Delete Command

		public override Hashtable GetCommand()
		{
			this.builder = new OracleCommandBuilder(this.oraAdapter);
			this.oraCmd.CommandText = "select * from " + this.TableName;
			this.builder = new OracleCommandBuilder(this.oraAdapter);
			Hashtable tb_OraCmd = new Hashtable(0);
			this.builder.ConflictOption = this.Conflict;

			OracleCommand selectcmd = new OracleCommand();
			selectcmd.CommandText = "select * from " + this.TableName;
			tb_OraCmd.Add("Select", selectcmd);
			tb_OraCmd.Add("Insert", this.builder.GetInsertCommand());
			tb_OraCmd.Add("Update", this.builder.GetUpdateCommand());
			tb_OraCmd.Add("Delete", this.builder.GetDeleteCommand());
			return tb_OraCmd;
		}

		#endregion

		#region 获取当前数据库下的所有表信息

		public override DataSet GetAllTables()
		{
			var ds = new DataSet();
			try
			{
				this.oraCmd.CommandText = "SELECT A.table_name as name, B.comments FROM user_tables A left join user_tab_comments B on A.table_name = B.table_name";
				this.oraAdapter.Fill(ds);
				return ds;
			}
			catch (Exception ex)
			{
				NLogHelper.Error(ex);
			}
			return ds;
		}

		#endregion

		#region 获取当前数据库下的所有视图信息

		public override DataSet GetAllViews()
		{
			var ds = new DataSet();
			try
			{
				this.oraCmd.CommandText = "SELECT view_name as name,view_name as comments FROM user_views";
				this.oraAdapter.Fill(ds);
				return ds;
			}
			catch (Exception ex)
			{
				NLogHelper.Error(ex);
			}
			return ds;
		}

		#endregion

		#region 获取表的字段信息

		public override DataSet GetColumns()
		{
			DataSet ds = new DataSet();
			try
			{
				this.oraCmd.CommandText = "SELECT A.column_name,A.data_type,A.char_length,A.nullable,B.comments FROM " +
					"user_tab_columns A left join user_col_comments B on A.column_name=B.column_name and A.table_name=B.table_name where A.table_name='" + this.TableName + "' order by column_name";

				this.oraAdapter.Fill(ds);
				return ds;
			}
			catch (Exception ex)
			{
				NLogHelper.Error(ex);
			}
			return ds;
		}

		#endregion

		#region 获取主键字段信息

		public override DataSet GetPrimaryKeys()
		{
			var ds = new DataSet();
			try
			{
				string strSQL = string.Format("SELECT COLUMN_NAME FROM USER_CONS_COLUMNS where Table_Name='{0}' ", this.TableName);
				strSQL = strSQL + string.Format("AND CONSTRAINT_NAME = (SELECT CONSTRAINT_NAME FROM USER_CONSTRAINTS where Table_Name='{0}' AND CONSTRAINT_TYPE='P')", this.TableName);
				this.oraCmd.CommandText = strSQL;
				this.oraAdapter.Fill(ds);
				return ds;
			}
			catch (Exception ex)
			{
				NLogHelper.Error(ex);
			}
			return ds;
		}

		#endregion

		#region 获取外键字段信息
		public override DataSet GetForeignKeys()
		{
			DataSet ds = new DataSet();
			try
			{
				string strSQL = string.Format("select table_name,column_name FROM USER_CONS_COLUMNS where constraint_name in ");
				strSQL = strSQL + string.Format("(select R_Constraint_name from USER_CONSTRAINTS where Table_Name='{0}'  and CONSTRAINT_TYPE='R')", this.TableName);
				this.oraCmd.CommandText = strSQL;

				this.oraAdapter.Fill(ds);
				return ds;
			}
			catch (Exception ex)
			{
				NLogHelper.Error(ex);
			}
			return ds;
		}

		#endregion

		#region 获取索引字段信息

		public override DataSet GetIndexs()
		{
			DataSet ds = new DataSet();
			try
			{
				string strSQL = string.Format("select column_name from user_ind_columns where table_name='{0}'", this.TableName);
				this.oraCmd.CommandText = strSQL;
				this.oraAdapter.Fill(ds);
				return ds;
			}
			catch (Exception ex)
			{
				NLogHelper.Error(ex);
			}
			return ds;
		}

		#endregion

		#region 获取表的空白架构

		public override DataSet GetBlankTableSchema()
		{
			var ds = new DataSet();
			try
			{
				this.oraCmd.CommandText = string.Format("SELECT * FROM {0} where 1=0", this.TableName);
				this.oraAdapter.Fill(ds, this.TableName);
				return ds;
			}
			catch (Exception ex)
			{
				NLogHelper.Error(ex);
			}
			return ds;
		}

		#endregion

		#region 获取 DataSet 架构
		public override DataSet GetTableSchema()
		{
			var dsBlankSchema = new DataSet();
			try
			{
				//获取表所有字段信息
				DataSet dsColumns = GetColumns();

				//获取表主键信息
				DataSet dsPrimaryKeys = GetPrimaryKeys();

				//获取表的空白架构
				dsBlankSchema = GetBlankTableSchema();

				//配置空白架构的主键信息
				DataColumn[] colPK = new DataColumn[dsPrimaryKeys.Tables[0].Rows.Count];
				for (int i = 0; i < dsPrimaryKeys.Tables[0].Rows.Count; i++)
				{
					string colName = dsPrimaryKeys.Tables[0].Rows[i]["column_name"].ToString();
					colPK[i] = dsBlankSchema.Tables[0].Columns[colName];
				}
				string pkName = "ds" + this.TableName + "Key1";
				if (!dsBlankSchema.Tables[0].Constraints.Contains(pkName) && colPK.Length > 0)
				{
					dsBlankSchema.Tables[0].Constraints.Add(pkName, colPK, true);
				}

				//配置空白架构的各字段信息
				for (int i = 0; i < dsColumns.Tables[0].Rows.Count; i++)
				{
					string colName = dsColumns.Tables[0].Rows[i]["column_name"].ToString();
					int maxLength = Convert.ToInt32(dsColumns.Tables[0].Rows[i]["char_length"]);
					string comments = dsColumns.Tables[0].Rows[i]["comments"].ToString();
					//string defaultValue=dsColumns.Tables[0].Rows[i]["Data_Default"].ToString();

					if (dsColumns.Tables[0].Rows[i]["NULLABLE"].ToString().Trim().ToUpper() == "N")
					{
						dsBlankSchema.Tables[0].Columns[colName].AllowDBNull = false;
					}
					else
					{
						dsBlankSchema.Tables[0].Columns[colName].AllowDBNull = true;
					}
					if (comments != "") dsBlankSchema.Tables[0].Columns[colName].Caption = comments;
					if (maxLength > 0) dsBlankSchema.Tables[0].Columns[colName].MaxLength = maxLength;
					//dsBlankSchema.Tables[0].Columns[colName].DefaultValue = defaultValue;

				}
				return dsBlankSchema;
			}
			catch (Exception ex)
			{
				NLogHelper.Error(ex);
			}
			return dsBlankSchema;
		}

		#endregion

		#region 获取表的注释
		public override string GetTableComments()
		{
			string tableComments = "";
			try
			{
				this.oraCmd.CommandText = string.Format("select comments from user_tab_comments where table_name='{0}'", this.TableName);
				DataSet ds = new DataSet();
				this.oraAdapter.Fill(ds);
				if (ds != null && ds.Tables[0].Rows.Count > 0)
				{
					tableComments = ds.Tables[0].Rows[0][0].ToString();
				}
			}
			catch (Exception ex)
			{
				NLogHelper.Error(ex);
			}
			return tableComments;
		}
		#endregion

		#region DML (数据库操作)

		/// 执行不带参数的SQL语句 由SqlString属性设置   
		/// </summary>   
		/// <returns>执行成功返回"" 失败返回错误信息</returns>   
		public override string ExecuteSql(string strSQL)
		{
			string ls_ret = "";

			try
			{
				oraCon.Open();
			}
			catch (OracleException ex)
			{
				ls_ret = "(0)[" + ex.ErrorCode.ToString() + "]" + ex.Message;
				goto
					ErrEnd;
			}

			OracleTransaction myTrans = oraCon.BeginTransaction();
			try
			{
				var Cmd = new OracleCommand(strSQL, oraCon, myTrans);

				Cmd.CommandType = CommandType.Text;

				Cmd.ExecuteNonQuery();

				myTrans.Commit();
			}
			catch (OracleException ex)
			{
				ls_ret = "(-1)[" + ex.ErrorCode.ToString() + "]" + ex.Message.ToString();
				myTrans.Rollback();
			}
			finally
			{
				oraCon.Close();
			}

		ErrEnd:
			return ls_ret;
		}


		/// <summary>
		/// 执行带参数的SQL语句
		/// base function
		/// </summary>
		/// <param name="strSQL"></param>
		/// <param name="parameterNames"></param>
		/// <param name="parameterValues"></param>
		/// <returns></returns>
		public override string ExecuteParamSQL(string strSQL, List<string> parameterNames, List<object> parameterValues)
		{
			string ls_ret = "";

			try
			{
				oraCon.Open();
			}
			catch (OracleException ex)
			{
				ls_ret = "(0)[" + ex.ErrorCode.ToString() + "]" + ex.Message;
				goto
					ErrEnd;
			}

			var OraTran = oraCon.BeginTransaction();
			try
			{
				var Cmd = new OracleCommand(strSQL, oraCon, OraTran);

				Cmd.CommandType = CommandType.Text;

				Cmd.Parameters.Clear();

				for (int i = 0; i < parameterNames.Count; i++)
				{
					var parameter = new OracleParameter(parameterNames[i], parameterValues[i]);
					Cmd.Parameters.Add(parameter);
				}
				Cmd.ExecuteNonQuery();
				OraTran.Commit();
			}
			catch (OracleException ex)
			{
				ls_ret = "(-1)[" + ex.ErrorCode.ToString() + "]" + ex.Message.ToString();
				OraTran.Rollback();
			}
			finally
			{
				oraCon.Close();
			}

		ErrEnd:
			return ls_ret;

		}

		/// <summary>
		///  function in use。List<string> parameterNames, List<object> parameterValues
		/// </summary> VALUES 
		/// <param name="strSQL"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public override string ExecuteParamSQL(string TableName, FilterData filter)
		{
			StringBuilder sb = new StringBuilder(0);
			sb.Append("INSERT INTO ");
			sb.Append(TableName);
			sb.Append(" ( ");

			for (int i = 0; i < filter.ParameterNames.Count; i++)
			{
				sb.Append(filter.ParameterNames[i]);
				sb.Append(",");
			}
			StringHelper.RemoveLastStrSplit(sb, ",");

			sb.Append(" )");

			sb.Append(" VALUES ( ");

			for (int i = 0; i < filter.ParameterNames.Count; i++)
			{
				sb.Append(":");
				sb.Append(filter.ParameterNames[i]);
				sb.Append(",");
			}
			StringHelper.RemoveLastStrSplit(sb, ",");

			sb.Append(" ) ");

			return ExecuteParamSQL(sb.ToString(), filter.ParameterNames, filter.ParameterValues);

		}

		#endregion

		#region 执行带事务的存储过程

		/// <summary>
		/// oracleParameters 里可设置 parameterName、parameterValue、out 参数、OracleType、Size
		/// </summary>
		/// <param name="procedure_Name"></param>
		/// <param name="oracleParameters"></param>
		/// <returns></returns>
		public bool ExecuteProcedure(string procedure_Name, List<OracleParameter> oracleParameters, ref string errorMsg)
		{
			OracleTransaction OraTran = null;
			this.oraAdapter = new OracleDataAdapter();
			this.oraCmd.CommandType = CommandType.StoredProcedure;
			this.oraCmd.CommandText = procedure_Name;
			this.oraCmd.Parameters.Clear();
			for (int i = 0; i < oracleParameters.Count; i++)
			{
				OracleParameter parameter = oracleParameters[i];
				this.oraCmd.Parameters.Add(parameter);
			}

			errorMsg = "";

			try
			{
				this.oraCon.Open();
				OraTran = oraCon.BeginTransaction();
				this.oraCmd.Transaction = OraTran;
				int nSignal = this.oraCmd.ExecuteNonQuery();

				if (nSignal > -1)       // 成功
				{
					OraTran.Commit();
					oraCon.Close();
					return true;
				}
				else
				{
					OraTran.Rollback();
					oraCon.Close();
					return false;
				}
			}
			catch (Exception ex)
			{
				if (OraTran != null)
				{
					OraTran.Rollback();
				}
				if (oraCon.State == ConnectionState.Open)
				{
					oraCon.Close();
				}

				errorMsg = ex.Message;

				NLogHelper.Error(ex);
				return false;
			}
		}

		/// <summary>
		/// 执行带事务的存储过程，允许设定 out/in 参数
		/// </summary>
		/// <param name="procedure_Name"></param>
		/// <param name="parameterNames"></param>
		/// <param name="parameterValues"></param>
		/// <param name="parameterDirections"></param>
		/// <returns></returns>
		public bool ExecuteProcedure(string procedure_Name, List<string> parameterNames, List<object> parameterValues, List<ParameterDirection> parameterDirections)
		{
			OracleTransaction OraTran = null;
			this.oraAdapter = new OracleDataAdapter();
			this.oraCmd.CommandType = CommandType.StoredProcedure;
			this.oraCmd.CommandText = procedure_Name;
			this.oraCmd.Parameters.Clear();
			for (int i = 0; i < parameterNames.Count; i++)
			{
				OracleParameter parameter = new OracleParameter(parameterNames[i], parameterValues[i]);
				if (parameterDirections[i] == ParameterDirection.Output)
				{

					parameter.Direction = parameterDirections[i];
				}
				this.oraCmd.Parameters.Add(parameter);
			}

			try
			{
				this.oraCon.Open();
				OraTran = oraCon.BeginTransaction();
				this.oraCmd.Transaction = OraTran;
				int nSignal = this.oraCmd.ExecuteNonQuery();

				if (nSignal > -1)       // 成功
				{
					OraTran.Commit();
					oraCon.Close();
					return true;
				}
				else
				{
					OraTran.Rollback();
					oraCon.Close();
					return false;
				}
			}
			catch (Exception ex)
			{
				if (OraTran != null)
				{
					OraTran.Rollback();
				}
				if (oraCon.State == ConnectionState.Open)
				{
					oraCon.Close();
				}
				NLogHelper.Error(ex);
				return false;
			}
		}


		// <summary>
		/// 执行带事务的存储过程
		/// </summary>
		/// <param name="procedure_Name"></param>
		/// <param name="parameterNames"></param>
		/// <param name="parameterValues"></param>
		/// <returns></returns>
		public bool ExecuteProcedure(string procedure_Name, List<string> parameterNames, List<object> parameterValues)
		{
			OracleTransaction OraTran = null;
			this.oraAdapter = new OracleDataAdapter();
			this.oraCmd.CommandType = CommandType.StoredProcedure;
			this.oraCmd.CommandText = procedure_Name;
			this.oraCmd.Parameters.Clear();
			for (int i = 0; i < parameterNames.Count; i++)
			{
				OracleParameter parameter = new OracleParameter(parameterNames[i], parameterValues[i]);
				this.oraCmd.Parameters.Add(parameter);
			}

			try
			{
				this.oraCon.Open();
				OraTran = oraCon.BeginTransaction();
				this.oraCmd.Transaction = OraTran;
				int nSignal = this.oraCmd.ExecuteNonQuery();

				if (nSignal > -1)       // 成功
				{
					OraTran.Commit();
					oraCon.Close();
					return true;
				}
				else
				{
					OraTran.Rollback();
					oraCon.Close();
					return false;
				}
			}
			catch (Exception ex)
			{
				if (OraTran != null)
				{
					OraTran.Rollback();
				}
				if (oraCon.State == ConnectionState.Open)
				{
					oraCon.Close();
				}
				NLogHelper.Error(ex);
				return false;
			}
		}

		/// <summary>
		/// 执行不带事务的存储过程
		/// </summary>
		/// <param name="procedure_Name"></param>
		/// <param name="parameterNames"></param>
		/// <param name="parameterValues"></param>
		/// <returns></returns>
		private bool ExecuteProcedure_withoutTrans(string procedure_Name, List<string> parameterNames, List<object> parameterValues)
		{
			this.oraAdapter = new OracleDataAdapter();
			this.oraCmd.CommandType = CommandType.StoredProcedure;
			this.oraCmd.CommandText = procedure_Name;
			this.oraCmd.Parameters.Clear();
			for (int i = 0; i < parameterNames.Count; i++)
			{
				OracleParameter parameter = new OracleParameter(parameterNames[i], parameterValues[i]);
				this.oraCmd.Parameters.Add(parameter);
			}

			try
			{
				this.oraCon.Open();
				if (this.oraCmd.ExecuteNonQuery() > -1)
				{
					oraCon.Close();
					return true;
				}
				else
				{
					oraCon.Close();
					return false;
				}
			}
			catch (Exception ex)
			{
				if (oraCon.State == ConnectionState.Open)
				{
					oraCon.Close();
				}
				NLogHelper.Error(ex);
				return false;
			}
		}

		#endregion

	}
}
