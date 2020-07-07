using PrintDemo.Common;
using System;
using System.Data.OracleClient;

namespace PrintDemo.DataBase
{
    public class OraConnection : GeneralConnection
    {
        public string ConStr;

        public override bool CheckConnection()
        {
            using (OracleConnection con = new OracleConnection(ConStr))
            {
                try
                {
                    con.Open();
                    con.Close();
                    con.Dispose();

                    return true;
                }
                catch (Exception ex)
                {
                    NLogHelper.Error(ex);
                    return false;
                }
            }
        }

        public OraConnection(string _conStr)
        {
            this.ConStr = _conStr;
            OracleConnection con = new OracleConnection(ConStr);
        }

        public override string GetConnectionString()
        {
            string strCon = string.Format("data source={0};user id={1};password={2}", this.Server, this.UID, this.PWD);

            return strCon;
        }


    }
}
