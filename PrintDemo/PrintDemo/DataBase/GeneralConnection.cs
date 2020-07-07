namespace PrintDemo.DataBase
{
    public abstract class GeneralConnection
    {
        private string _server;
        public string Server
        {
            set
            {
                this._server = value;
            }
            get
            {
                return this._server;
            }
        }

        private string _uid;
        public string UID
        {
            set
            {
                this._uid = value;
            }
            get
            {
                return this._uid;
            }
        }

        private string _pwd;
        public string PWD
        {
            set
            {
                this._pwd = value;
            }
            get
            {
                return this._pwd;
            }
        }

        public abstract bool CheckConnection();

        public abstract string GetConnectionString();

    }
}
