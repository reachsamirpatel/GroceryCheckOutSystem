using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GroceryCheckOut.Entity.Interfaces;
using Logging;
using Utility;


namespace GroceryCheckOutSystem.DataAccess
{
    public class RepositoryDAC : IRepositoryBP
    {
        public string FilePath;
        private readonly ILogger _log;
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
            try
            {
                File.WriteAllText(FilePath, ParseHelper.ToXML(t));
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                throw;
            }
        }

        public List<T> GetAll<T>() where T : class
        {
            try
            {
                return File.Exists(FilePath) ? ParseHelper.FromXML<List<T>>(File.ReadAllText(FilePath)) : new List<T>();
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                return new List<T>();
            }
        }
    }
}
