using PrintDemo.DataBase;

namespace PrintDemo.GlobalParams
{
    public partial class GlobalRes
    {
        private static string _ConStr; // 连接字符串
        public static string ConStr
        {
            set
            {
                _ConStr = value;
            }
            get
            {
                return _ConStr;
            }
        }

        private static OraConnection _OraConnection; // 用于 check connection
        public static OraConnection m_OraConnection
        {
            set
            {
                _OraConnection = value;
            }
            get
            {
                return _OraConnection;
            }
        }
    }
}
