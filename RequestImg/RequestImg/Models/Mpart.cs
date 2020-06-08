namespace RequestImg
{
    public class Mpart
    {
        //{"list":[{"errcode":0,"errmsg":null,"objId":"01_B864B6D8CC78460E852592BABC4E8DCB","objNo":"自定义A3_420_0",
        //"fname":"自定义A3_420_0.pdf","suffix":"pdf","hasAffine":false,"name":"自定义A3_420_0","fsize":96178,
        //"fsizeStr":"93.92KB","type":"F","tablename":"DESF","smemo":null,"ctimestr":"2017-03-31 13:28","mtimestr":"2017-03-31 13:28",
        //"creator":"adm","modifier":"adm","ver":"1","stimestr":null,"etimestr":null,"extra":null}],
        //"errcode":0,"errmsg":null,"total":416}
        public int? errcode { get; set; }
        public string errmsg { get; set; }
        public string objId { get; set; }
        public string objNo { get; set; }
        public string fname { get; set; }
        public string suffix { get; set; }
        public string hasAffine { get; set; }
        public string name { get; set; }
        public string fsize { get; set; }
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
    }
}
