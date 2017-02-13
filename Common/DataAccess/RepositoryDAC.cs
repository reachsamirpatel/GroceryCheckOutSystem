using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using GroceryCheckOut.Entity.Interfaces;
using Logging;
using Utility;


namespace GroceryCheckOutSystem.DataAccess
{
    /// <summary>
    /// Base class to read and write to xml files.
    /// </summary>
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
        /// <summary>
        /// Method to insert value to xml
        /// </summary>
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
        /// <summary>
        /// Method to get list from xml
        /// </summary>
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
    }
}
