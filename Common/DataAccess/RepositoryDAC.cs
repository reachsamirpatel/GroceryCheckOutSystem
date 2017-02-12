using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Interfaces;
using Logging;
using Utility;


namespace GroceryCheckOutSystem.DataAccess
{
    public class RepositoryDAC : IRepositoryBP
    {
        public string FilePath;
        private ILogger _log;
        public RepositoryDAC(string filePath)
        {
            FilePath = filePath;
        }
        public RepositoryDAC()
        {
            _log = LogManager.GetLogger(this);
        }


        public void UpSert<T>(T t) where T : class
        {
            File.WriteAllText(FilePath, ParseHelper.ToXML(t));
        }

        public void UpSert1<T>(T t) where T : class
        {
            File.WriteAllText(FilePath, ParseHelper.DataContractSerializeObject(t));
        }

        public List<T> GetAll<T>() where T : class
        {
            if (File.Exists(FilePath))
                return ParseHelper.FromXML<List<T>>(File.ReadAllText(FilePath));
            else
            {
                return new List<T>();
            }
            //  try
            {
                //if (File.Exists(FilePath))
                //    return ParseHelper.ParseXML<List<T>>(File.ReadAllText(FilePath));
                //else
                //{
                //    return new List<T>();
                //}
            }
            //catch (Exception exp)
            //{
            //    _log.Error(exp.Message);
            //    _log.Error(exp.StackTrace);
            //    return new List<T>();
            //}


        }

        public List<T> GetList<T>() where T : class
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            //  xRoot.ElementName = "Promotions";
            xRoot.IsNullable = true;
            using (StreamReader reader = new StreamReader(FilePath))
            {
                List<T> result = (List<T>)(new XmlSerializer(typeof(List<T>), xRoot)).Deserialize(reader);
                int numOfPersons = result.Count;
                return result;
            }

        }
    }
}
