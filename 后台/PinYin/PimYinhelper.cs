using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CaterComman
{
    /// <summary>
    /// 生成拼音
    /// </summary>
    public partial class PimYinhelper
    {
        public static string GetPinYin(string str)
        {
            string s = "";
            foreach (char cc in str)
            {
                ChineseChar ch = new ChineseChar(cc);
                s += ch.Pinyins[0][0];
            }
            return s;
        }
    }
}
