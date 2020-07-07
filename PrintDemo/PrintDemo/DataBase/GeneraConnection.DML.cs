using System.Collections.Generic;
using System.Data;

namespace PrintDemo.DataBase
{
    public abstract partial class GeneralAccess
    {

        #region 查询属性

        private Dictionary<string, object> _filterParameter;

        public Dictionary<string, object> FilterParameter
        {
            set
            {
                this._filterParameter = value;

            }
            get
            {
                return this._filterParameter;
            }
        }

        private string _filterString;

        public string FilterString
        {
            set
            {
                this._filterString = value;
            }
            get
            {
                return this._filterString;
            }
        }

        private string _sortString;

        public string SortString
        {
            set
            {
                this._sortString = value;

            }
            get
            {
                return this._sortString;
            }
        }

        //private int _startIndex;
        //private int _endIndex;

        //private string _queryName;

        //public string QueryName
        //{
        //    get
        //    {
        //        return this._queryName;
        //    }
        //}

        #endregion

        //public abstract bool CheckConState();        //--检查连接状态

        public abstract DataSet SelectBySQL(string strSQL);

        public abstract string ExecuteSql(string strSQL);

        public abstract DataSet SelectBySQL(string tableName, string filter, List<string> parameterNames, List<object> parameterValues);

        public abstract DataSet SelectBySQL(string tableName, FilterData filter);

        public abstract string ExecuteParamSQL(string strSQL, List<string> parameterNames, List<object> parameterValues);

        public abstract string ExecuteParamSQL(string tableName, FilterData filter);
    }
}
