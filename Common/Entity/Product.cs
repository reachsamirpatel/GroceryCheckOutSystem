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
    public class Product
    {
        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Price { get; set; }
    }
}
