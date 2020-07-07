using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace PrintDemo.DataBase
{
    [Serializable]
    public abstract partial class GeneralAccess
    {

        #region 属性列表
        private string _tableName;
        public string TableName
        {
            set
            {
                this._tableName = value;
            }
            get
            {
                return this._tableName;
            }
        }

        //private string _tableComment;
        //public string TableComment
        //{
        //    set
        //    {
        //        this._tableComment = value;
        //    }
        //    get
        //    {
        //        return this._tableComment;
        //    }
        //}

        private ConflictOption _conflict = ConflictOption.OverwriteChanges;
        public ConflictOption Conflict
        {
            set
            {
                _conflict = value;
            }
            get
            {
                return this._conflict;
            }
        }

        private DataSet _dsAllTables;
        public DataSet dsAllTables
        {
            get
            {
                this._dsAllTables = this.GetAllTables();
                return this._dsAllTables;
            }
        }

        private DataSet _dsAllViews;
        public DataSet dsAllViews
        {
            get
            {
                this._dsAllViews = this.GetAllViews();
                return this._dsAllViews;
            }
        }

        private DataSet _dsTableSchema;
        public DataSet dsTableSchema
        {
            get
            {
                this._dsTableSchema = this.GetTableSchema();
                return this._dsTableSchema;
            }
        }

        public List<string> ForeignKeys
        {
            get
            {

                List<string> listKeys = new List<string>(0);
                DataSet dsForeignKeys = this.GetForeignKeys();
                for (int i = 0; i < dsForeignKeys.Tables[0].Rows.Count; i++)
                {
                    if (!listKeys.Contains(dsForeignKeys.Tables[0].Rows[i]["column_name"].ToString()))
                    {
                        listKeys.Add(dsForeignKeys.Tables[0].Rows[i]["column_name"].ToString());
                    }
                }

                return listKeys;
            }
        }

        public DataSet ForeignRelations
        {
            get
            {
                return this.GetForeignKeys();
            }
        }
        #endregion

        public abstract Hashtable GetCommand();

        public abstract DataSet GetAllTables();

        public abstract DataSet GetAllViews();

        public abstract string GetTableComments();

        public abstract DataSet GetTableSchema();

        public abstract DataSet GetColumns();

        public abstract DataSet GetPrimaryKeys();

        public abstract DataSet GetForeignKeys();

        public abstract DataSet GetIndexs();

        public abstract DataSet GetBlankTableSchema();



    }
}
