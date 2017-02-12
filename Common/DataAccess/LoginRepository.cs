using System.Collections.Generic;
using System.Configuration;
using GroceryCheckOut.Entity;

namespace GroceryCheckOutSystem.DataAccess
{
    public class LoginRepository : RepositoryDAC
    {
        public LoginRepository()
        {
            base.FilePath = ConfigurationManager.AppSettings["LoginFileName"];
        }

        public List<Login> GetAll()
        {
            return base.GetAll<Login>();
        }

        public void UpSert(List<Login> loginList)
        {
            base.UpSert(loginList);
        }

    }
}
