using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CaterComman
{
    /// <summary>
    /// 生成MD5值
    /// </summary>
    public partial class MD5Helper
    {
        public static string EncryptString(string str)
        {
            //创建对象的方法：构造方法、静态方法(工厂)
            MD5 md5 = MD5.Create();
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            byte[] byteNew = md5.ComputeHash(byteOld);
            StringBuilder build=new StringBuilder();
            foreach(byte b in byteNew)
            {
                //减肥字符转换称16进制表示的字符串、而且是恒占用两位
                build.Append(b.ToString("x2"));
            }
            return build.ToString();
        }
    }
}
