using System.Collections.Generic;
using System.Configuration;
using GroceryCheckOut.Entity;

namespace GroceryCheckOutSystem.DataAccess
{
    public class UserRepository : RepositoryDAC
    {
        public UserRepository()
        {
            base.FilePath = ConfigurationManager.AppSettings["UserFileName"];
        }

        public List<User> GetAll()
        {
            return base.GetAll<User>();
        }

        public void UpSert(List<User> userList)
        {
            base.UpSert(userList);
        }

    }
}
