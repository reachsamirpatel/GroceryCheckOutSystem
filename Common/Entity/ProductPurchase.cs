using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GroceryCheckOut.Entity
{
    [DataContract]
    [Serializable]
    public class ProductPurchase
    {
        public ProductPurchase(Product product)
        {
            Product = product;
            DiscountedPrice = product.Price;
        }

        public ProductPurchase()
        {
        }
        [DataMember]
        public Product Product { get; set; }

        [DataMember]
        public double DiscountedPrice { get; set; }

        [DataMember]
        public int Quantity { get; set; } = 1;
    }
}

