namespace RequestImg
{
    public class Guan
    {
        //{"list":[{"errcode":0,"errmsg":null,"objId":"01_44BE546DA4154B5983511A198C5C3787","objNo":"prod001",
        //"fname":"Oracle判断是否数值.sql","suffix":"sql","hasAffine":false,"name":"prot001title","fsize":1184,"fsizeStr":"1.16KB",
        //"type":"F","tablename":"DESF","smemo":null,"ctimestr":"2017-08-05 22:23","mtimestr":"2017-08-05 22:23","creator":"adm",
        //"modifier":"adm","ver":"1","stimestr":null,"etimestr":null,"extra":null,"searchText":null}],
        //"errcode":0,"errmsg":null,"total":2}
        public int? errcode { get; set; }
        public string errmsg { get; set; }
        public string objId { get; set; }
        public string objNo { get; set; }
        public string fname { get; set; }
        public string suffix { get; set; }
        public string hasAffine { get; set; }
        public string name { get; set; }
        public int? fsize { get; set; }
        public string fsizeStr { get; set; }
        public string type { get; set; }
        public string tablename { get; set; }
        public string smemo { get; set; }
        public string ctimestr { get; set; }
        public string mtimestr { get; set; }
        public string creator { get; set; }
        public string modifier { get; set; }
        public string ver { get; set; }
        public string stimestr { get; set; }
        public string etimestr { get; set; }
        public string extra { get; set; }
        public string searchText { get; set; }
    }
}
