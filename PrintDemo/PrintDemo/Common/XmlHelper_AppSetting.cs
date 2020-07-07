using System;
using System.Xml;

namespace PrintDemo.Common
{
    public partial class XmlHelper
    {

        #region 从类似于 AppSettings 的 xml 获取节点信息

        /// <summary>
        /// 从类似于 AppSettings、名称为 ProcessName 的 xml，根据 add > key/value 获取节点信息；实现配置文件的 partial
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="AttributeName">
        /// 使用默认值 = app.config；
        /// 扩展为可使用自定义属性
        /// </param>
        /// <returns></returns>
        public static string GetValueLikeAppSettings(string key, string xmlFileName, string AttributeName = "value")
        {
            string Rtn = "";

            xmlFileName = AvoidXmlNotFound_DuringDebug(xmlFileName);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFileName);    //加载 Xml 文件  
            }
            catch (Exception ex)
            {
                SimpleLogHelper.Error(ex);
                return "";
            }

            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlElement node = null;

            foreach (var xmlNode in rootElem.ChildNodes)
            {
                node = xmlNode as XmlElement;
                if (node == null)
                {
                    continue;
                }

                if (node.NodeType == XmlNodeType.Element && node.GetAttribute("key").Trim() == key.Trim())
                {
                    Rtn = node.GetAttribute(AttributeName);
                    return Rtn;
                }
            }

            return Rtn;
        }

        /// <summary>
        /// 从
        /// <add key="Camera2" name="C525"  >
        ///<![CDATA[usb#vid_058f&pid_a014&mi_00#7&1edadac0&0&0000]]>
        /// </add> 
        /// 中读取 cdata 内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="xmlFileName"></param>
        /// <param name="nIndex">CDATA 在 childnode 中 的索引</param>
        /// <returns></returns>
        public static string GetValue_FromCData_inAppSettingsLikeXML(string key, string xmlFileName, int nIndex = 0)
        {
            string Rtn = "";

            xmlFileName = AvoidXmlNotFound_DuringDebug(xmlFileName);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFileName);    //加载 Xml 文件  
            }
            catch (Exception ex)
            {
                SimpleLogHelper.Error(ex);
                return "";
            }

            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlElement node = null;

            foreach (var xmlNode in rootElem.ChildNodes)
            {
                node = xmlNode as XmlElement;
                if (node == null)
                {
                    continue;
                }

                if (node.NodeType == XmlNodeType.Element && node.GetAttribute("key").Trim() == key.Trim())
                {
                    if (node.ChildNodes.Count > nIndex && node.ChildNodes[nIndex] != null && node.ChildNodes[nIndex].NodeType == XmlNodeType.CDATA)
                    {
                        return node.ChildNodes[nIndex].Value;
                    }
                }
            }

            return Rtn;
        }

        #endregion

        #region 写入类似于 AppSettings 的 xml 节点，存储变化后的 value

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public static string WriteValueLikeAppSettings(string key, string value, string xmlFileName, string AttributeName = "value")
        {
            string Error_Msg = "";

            xmlFileName = AvoidXmlNotFound_DuringDebug(xmlFileName);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFileName);    //加载 Xml 文件  

                XmlElement rootElem = doc.DocumentElement;   //获取根节点  
                XmlElement node = null;

                foreach (var xmlNode in rootElem.ChildNodes)
                {
                    node = xmlNode as XmlElement;
                    if (node == null)
                    {
                        continue;
                    }
                    if (node.NodeType == XmlNodeType.Element && node.GetAttribute("key").Trim() == key.Trim())
                    {
                        node.SetAttribute(AttributeName, value);
                        doc.Save(xmlFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                SimpleLogHelper.Error(ex);
                return ex.Message;
            }

            return Error_Msg;
        }

        /// <summary>
        /// 写入 value 到 CData
        /// </summary>
        /// <param name="key"></param>
        /// <param name="strValue"></param>
        /// <param name="xmlFileName"></param>
        /// <param name="nIndex">CDATA 在 childnode 中 的索引</param>
        /// <returns></returns>
        public static string WriteCData_inAppSettingsLikeXML(string key, string strValue, string xmlFileName, int nIndex = 0)
        {
            string Error_Msg = "";

            xmlFileName = AvoidXmlNotFound_DuringDebug(xmlFileName);

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(xmlFileName);    //加载 Xml 文件  

                XmlElement rootElem = doc.DocumentElement;   //获取根节点  
                XmlElement node = null;

                foreach (var xmlNode in rootElem.ChildNodes)
                {
                    node = xmlNode as XmlElement;
                    if (node == null)
                    {
                        continue;
                    }
                    if (node.NodeType == XmlNodeType.Element && node.GetAttribute("key").Trim() == key.Trim())
                    {
                        if (node.ChildNodes.Count > nIndex && node.ChildNodes[nIndex] != null && node.ChildNodes[nIndex].NodeType == XmlNodeType.CDATA)
                        {
                            node.ChildNodes[nIndex].Value = strValue;
                        }
                        else // 若不存在则新增 Cdata
                        {
                            XmlCDataSection m_cdata = doc.CreateCDataSection(strValue);
                            node.AppendChild(m_cdata);
                        }

                        doc.Save(xmlFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                SimpleLogHelper.Error(ex);
                return ex.Message;
            }

            return Error_Msg;
        }

        #endregion
    }
}
