using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace abc
{
    public class XmlHelper
    {
        public static Dictionary<string, string> GetMsgEntity(string text)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(text);
                Dictionary<string, string> dict = new Dictionary<string, string>();
                XmlNodeList xml = doc.SelectSingleNode("/xml").ChildNodes;
                foreach (XmlNode node in xml)
                {
                    dict.Add(node.Name, node.InnerText);
                }
                return dict;
            }
            catch (Exception)
            {
                return new Dictionary<string, string>();
            }
        }
    }
}