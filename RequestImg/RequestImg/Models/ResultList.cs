using System.Collections.Generic;

namespace RequestImg
{
    public class ResultList<T>
    {
        public List<T> list { get; set; }
        public int? errcode { get; set; }
        public string errmsg { get; set; }
        public int? total { get; set; }
    }
}
