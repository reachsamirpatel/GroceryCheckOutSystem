using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Utility
{
    /// <summary>
    /// Class to perform xml serializing and deserializing xml
    /// </summary>
    static public class ParseHelper
    {
        /// <summary>
        /// Serialize an object to xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXML<T>(T obj)
        {
            using (StringWriter stringWriter = new Utf8StringWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }
        /// <summary>
        /// Deserialzed object from xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXML<T>(string xml)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute());
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }

    /// <summary>
    /// Class to create UTF-8 decoded xml
    /// </summary>
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return new UTF8Encoding(false); }
        }
    }
}
