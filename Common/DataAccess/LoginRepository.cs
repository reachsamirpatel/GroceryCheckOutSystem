using System.Collections.Generic;
using System.Configuration;
using GroceryCheckOut.Entity;

namespace GroceryCheckOutSystem.DataAccess
{
    /// <summary>
    /// Class read and write to Logins.xml file to perform login operations
    /// </summary>
    public class LoginRepository : RepositoryDAC
    {
        public LoginRepository()
        {
            base.FilePath = ConfigurationManager.AppSettings["LoginFileName"];
        }
        /// <summary>
        /// Method to get list from xml
        /// </summary>
        public List<Login> GetAll()
        {
            return base.GetAll<Login>();
        }
        /// <summary>
        /// Method to insert value to xml
        /// </summary>
        /// <param name="loginList"></param>
        public void UpSert(List<Login> loginList)
        {
            base.UpSert(loginList);
        }

    }
}
