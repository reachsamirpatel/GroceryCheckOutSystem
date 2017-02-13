using System.Collections.Generic;
using System.Configuration;
using GroceryCheckOut.Entity;

namespace GroceryCheckOutSystem.DataAccess
{
    /// <summary>
    /// Class read and write to Products.xml file to perform product operations
    /// </summary>
    public class ProductRepository : RepositoryDAC
    {
        public ProductRepository()
        {
            base.FilePath = ConfigurationManager.AppSettings["ProductFileName"];
        }
        /// <summary>
        /// Method to get list from xml
        /// </summary>
        public List<Product> GetAll()
        {
            return base.GetAll<Product>();
        }
        /// <summary>
        /// Method to insert value to xml
        /// </summary>
        /// <param name="loginList"></param>
        public void UpSert(List<Product> productList)
        {
            base.UpSert(productList);
        }

    }
}
