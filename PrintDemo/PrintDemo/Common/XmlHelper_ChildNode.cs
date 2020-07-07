using System;
using System.Xml;

namespace PrintDemo.Common
{
    public partial class XmlHelper
    {

        #region read by xmlpath

        public static string AvoidXmlNotFound_DuringDebug(string xmlFileName)
        {
            xmlFileName = xmlFileName.Replace(".vshost", ""); // 避免调试时找不到  对应文件
            if (!xmlFileName.EndsWith(".xml"))
            {
                xmlFileName = string.Format("{0}.xml", xmlFileName);
            }
            return xmlFileName;
        }

        /// <summary>
        /// 从单子节点 xml 获取Value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public static string GetSingleValueFromChildNode1(string key, string xmlFileName)
        {
            string Rtn = "";
            XmlDocument doc = new XmlDocument();

            xmlFileName = AvoidXmlNotFound_DuringDebug(xmlFileName);

            try
            {
                doc.Load(xmlFileName);    //加载Xml文件  
                XmlElement rootElem = doc.DocumentElement;   //获取根节点  

                XmlNode node = rootElem.ChildNodes[0];
                if (node == null)
                {
                    return "";
                }

                XmlElement element = node as XmlElement;
                Rtn = element.GetAttribute(key);
            }
            catch (Exception ex)
            {
                SimpleLogHelper.Error(ex);
            }

            return Rtn;
        }

        /// <summary>
        ///  从多子节点 xml 获取单个value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="childrootName"></param>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public static string GetSingleValue(string key, string childrootName, string xmlFileName)
        {
            string Rtn = "";
            XmlDocument doc = new XmlDocument();

            xmlFileName = AvoidXmlNotFound_DuringDebug(xmlFileName);

            try
            {
                doc.Load(xmlFileName);    //加载Xml文件  
                XmlElement rootElem = doc.DocumentElement;   //获取根节点  

                XmlNodeList rootNodes = rootElem.GetElementsByTagName(childrootName); //获取rootName子节点集合  
                XmlElement node = rootNodes[0] as XmlElement;
                Rtn = node.GetAttribute(key);
            }
            catch (Exception ex)
            {
                SimpleLogHelper.Error(ex);
            }

            return Rtn;
        }



        #endregion

    }
}
