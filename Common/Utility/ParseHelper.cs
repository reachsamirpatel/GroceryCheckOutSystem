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
    static public class ParseHelper
    {
        //private static JavaScriptSerializer _javaScriptSerializer;

        //private static JavaScriptSerializer JavaScriptSerializer => _javaScriptSerializer ?? (_javaScriptSerializer = new JavaScriptSerializer());

        //public static Stream ToStream(this string @this)
        //{
        //    var stream = new MemoryStream();
        //    var writer = new StreamWriter(stream);
        //    writer.Write(@this);
        //    writer.Flush();
        //    stream.Position = 0;
        //    return stream;
        //}

        //public static T ParseXML<T>(this string @this) where T : class
        //{
        //    XmlReader reader = XmlReader.Create(@this.Trim().ToStream(), new XmlReaderSettings()
        //    {
        //        ConformanceLevel = ConformanceLevel.Document
        //    });
        //    return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        //}

        //public static T ParseJSON<T>(this string @this) where T : class
        //{
        //    return JavaScriptSerializer.Deserialize<T>(@this.Trim());
        //}

        public static string DataContractSerializeObject<T>(T objectToSerialize)
        {
            using (MemoryStream memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memStm, objectToSerialize);

                memStm.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStm))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }

        public static string ToXML<T>(T obj)
        {
            //XmlDocument xmlDoc = new XmlDocument();
            //XPathNavigator nav = xmlDoc.CreateNavigator();
            ////using (StringWriter stringWriter = new Utf8StringWriter())
            ////{
            ////    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            ////    xmlSerializer.Serialize(stringWriter, obj);
            ////    return stringWriter.ToString();
            ////}
            //using (XmlWriter writer = nav.AppendChild())
            //{
            //    XmlSerializer ser = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("RootElement"));
            //    ser.Serialize(writer, obj);
            //    return ser.ToString();
            //}
            //// return xmlDoc;

            using (StringWriter stringWriter = new Utf8StringWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
            //using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
            //{
            //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            //    xmlSerializer.Serialize(stringWriter, obj);
            //    return stringWriter.ToString();
            //}
        }
        public static XElement ToXML<T>(this IList<T> lstToConvert, Func<T, bool> filter, string rootName)
        {
            var lstConvert = (filter == null) ? lstToConvert : lstToConvert.Where(filter);
            return new XElement(rootName,
               (from node in lstConvert
                select new XElement(typeof(T).ToString(),
        from subnode in node.GetType().GetProperties()
        select new XElement(subnode.Name, subnode.GetValue(node, null)))));

        }
        public static T FromXML<T>(string xml)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute());
                //var xmlSerializer = new XmlSerializer(MyApp.GetType(), new XmlRootAttribute("car");
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
    public class Utf8StringWriter : StringWriter
    {
        // Use UTF8 encoding but write no BOM to the wire
        public override Encoding Encoding
        {
            get { return new UTF8Encoding(false); }
        }
    }
}
