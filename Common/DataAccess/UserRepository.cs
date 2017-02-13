using System.Collections.Generic;
using System.Configuration;
using GroceryCheckOut.Entity;

namespace GroceryCheckOutSystem.DataAccess
{
    /// <summary>
    /// Class read and write to Users.xml file to perform user operations
    /// </summary>
    public class UserRepository : RepositoryDAC
    {
        public UserRepository()
        {
            base.FilePath = ConfigurationManager.AppSettings["UserFileName"];
        }
        /// <summary>
        /// Method to get list from xml
        /// </summary>
        public List<User> GetAll()
        {
            return base.GetAll<User>();
        }
        /// <summary>
        /// Method to insert value to xml
        /// </summary>
        /// <param name="loginList"></param>
        public void UpSert(List<User> userList)
        {
            base.UpSert(userList);
        }

    }
}
