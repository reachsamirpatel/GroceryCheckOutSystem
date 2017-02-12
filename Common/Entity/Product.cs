using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GroceryCheckOut.Entity
{
    [DataContract]
    [Serializable, XmlRoot("Products")]
    public class Product
    {
        public Product()
        {

        }
        public Product(string name, double price)
        {
            ProductId = Guid.NewGuid();
            Name = name;
            Price = price;
        }
        public Product(Guid productId, string name, double price)
        {
            ProductId = productId;
            Name = name;
            Price = price;
        }

        [DataMember]
        public Guid ProductId { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Price { get; set; }
    }
}
