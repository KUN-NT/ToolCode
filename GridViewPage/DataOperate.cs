using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace GridViewPage
{
    class DataOperate
    {
        public static MySqlConnection myconn = null;
        public static string connectionString = "Server=47.111.108.199;port=3306;database=his_db;user=root;password=root;charset=utf8;pooling=true;";

        public static MySqlConnection getcon()
        {
            try
            {
                if (myconn == null)
                {
                    myconn = new MySqlConnection(connectionString);
                }
                return myconn;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        public static DataSet getds(string sql)
        {
            DataSet myds = null;
            MySqlConnection myconn = null;
            MySqlCommand mycmd = null;
            MySqlDataAdapter myda = null;
            try
            {
                myconn = getcon();
                myconn.Open();
                mycmd = new MySqlCommand(sql, myconn);
                myda = new MySqlDataAdapter(mycmd);
                myds = new DataSet();
                myda.Fill(myds);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                myds = null;
            }
            finally
            {
                if (myda != null)
                {
                    myda.Dispose();
                }
                if (myds != null)
                {
                    myds.Dispose();
                }
                if (mycmd != null)
                {
                    mycmd.Dispose();
                }
                myconn.Close();
            }
            return myds;
        }


        public static bool getCommand(string sql)
        {
            bool flag = false;
            MySqlConnection sqlconn = null;
            MySqlCommand sqlcom = null;
            try
            {
                sqlconn = getcon();
                sqlconn.Open();
                sqlcom = new MySqlCommand(sql, sqlconn);
                int result = sqlcom.ExecuteNonQuery();
                if (result > 0) flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Console.Write(e.Message);
            }
            finally
            {
                sqlcom.Dispose();
                sqlconn.Close();
            }
            return flag;
        }
        public static bool getComWithParams(string sql, List<MySqlParameter> paras)
        {
            bool flag = false;
            MySqlConnection sqlconn = null;
            MySqlCommand sqlcom = null;
            try
            {
                sqlconn = getcon();
                sqlconn.Open();
                sqlcom = new MySqlCommand(sql, sqlconn);
                sqlcom.Parameters.AddRange(paras.ToArray<MySqlParameter>());
                int result = sqlcom.ExecuteNonQuery();
                if (result > 0) flag = true;
            }
            catch (Exception e)
            {
                flag = false;
                Console.Write(e.Message);
            }
            finally
            {
                sqlcom.Dispose();
                sqlconn.Close();
            }
            return flag;
        }

    }
}
