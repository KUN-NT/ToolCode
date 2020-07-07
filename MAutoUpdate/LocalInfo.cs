using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace MAutoUpdate
{
    public class LocalInfo
    {
        public string LocalVersion { get; set; }
        public string LastUdpate { get; set; }
        public string ServerUpdateUrl { get; set; }
        public string LocalIgnoreVersion { get; set; }

        private string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Local.xml");


        public LocalInfo(string localAddress)
        {
            url = Path.Combine(localAddress, "Local.xml");
        }
        
        public void LoadXml()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(url);
            var root = xdoc.DocumentElement;
            var listNodes = root.SelectNodes("/LocalUpdate");
            foreach (XmlNode item in listNodes)
            {
                RemoteInfo remote = new RemoteInfo();
                foreach (XmlNode pItem in item.ChildNodes)
                {
                    GetType().GetProperty(pItem.Name).SetValue(this, pItem.InnerText, null);
                }
            }
        }
        public void SaveXml()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(url);
            var root = xdoc.DocumentElement;
            var listNodes = root.SelectNodes("/LocalUpdate");
            foreach (XmlNode item in listNodes)
            {
                foreach (XmlNode pItem in item.ChildNodes)
                {
                    // Key.SetValue(item.Name.ToString(), this.GetType().GetProperty(item.Name.ToString()).GetValue(this, null).ToString());
                    pItem.InnerText = this.GetType().GetProperty(pItem.Name.ToString()).GetValue(this, null).ToString();
                }
            }
            xdoc.Save(url);
        }
    }
}
