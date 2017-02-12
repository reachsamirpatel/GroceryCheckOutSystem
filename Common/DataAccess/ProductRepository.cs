using System.Collections.Generic;
using System.Configuration;
using GroceryCheckOut.Entity;

namespace GroceryCheckOutSystem.DataAccess
{
    public class ProductRepository : RepositoryDAC
    {
        public ProductRepository()
        {
            base.FilePath = ConfigurationManager.AppSettings["ProductFileName"];
        }

        public List<Product> GetAll()
        {
            return base.GetAll<Product>();
        }

        public void UpSert(List<Product> productList)
        {
            base.UpSert(productList);
        }

    }
}
