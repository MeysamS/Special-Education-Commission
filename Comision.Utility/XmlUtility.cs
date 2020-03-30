using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Comision.Utility
{
    public static class XmlUtility
    {
        //public XmlUtility()
        //{ }

        public static string ConvertObjectToXml<TObject>(TObject tobject)
        {
            try
            {
                XmlDocument myXml = new XmlDocument();
                XPathNavigator xNav = myXml.CreateNavigator();
                XmlSerializer x = new XmlSerializer(tobject.GetType());
                using (var xs = xNav.AppendChild())
                {
                    x.Serialize(xs, tobject);
                }
                return "<?xml version=\"1.0\" encoding=\"ibm850\"?>" + myXml.OuterXml;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public static TObject ConvertXmlToObject<TObject>(string xml)
        {
            try
            {
                string xData = xml;
                XmlSerializer x = new XmlSerializer(typeof(TObject));
                var myTest = (TObject)x.Deserialize(new StringReader(xData));
                return myTest;
            }
            catch (Exception exception)
            {
                return default(TObject);
            }
        }

    }
}
