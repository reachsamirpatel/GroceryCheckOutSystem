using System;
using System.Collections.Generic;
using System.Linq;
using GroceryCheckOut.Entity;

namespace BusinessProcess.Tests
{
    public class TestHelper
    {
        public static IEnumerable<string> ItemNames
        {
            get
            {
                yield return "apple";
                yield return "banana";
                yield return "orange";
            }
        }

        public static IEnumerable<Product> TestProducts
        {
            get
            {
                yield return new Product("apple", 1.52);
                yield return new Product("banana", 6.66);
                yield return new Product("orange", 1.11);
            }
        }

        public static IEnumerable<ProductPurchase> Basket4
        {
            get
            {
                List<ProductPurchase> items = new List<ProductPurchase>();

                items.AddRange(GetTestProductPurchase("apple", 1));
                items.AddRange(GetTestProductPurchase("banana", 1));
                items.AddRange(GetTestProductPurchase("orange", 1));

                return items;
            }
        }

        public static IEnumerable<ProductPurchase> Basket1
        {
            get
            {
                List<ProductPurchase> items = new List<ProductPurchase>();

                items.AddRange(GetTestProductPurchase("apple", 3));
                items.AddRange(GetTestProductPurchase("banana", 3));
                items.AddRange(GetTestProductPurchase("orange", 3));

                return items;
            }
        }

        public static IEnumerable<ProductPurchase> Basket2
        {
            get
            {
                List<ProductPurchase> items = new List<ProductPurchase>();

                items.AddRange(GetTestProductPurchase("apple", 2));
                items.AddRange(GetTestProductPurchase("banana", 3));
                items.AddRange(GetTestProductPurchase("orange", 4));

                return items;
            }
        }

        private static IEnumerable<ProductPurchase> Basket3
        {
            get
            {
                List<ProductPurchase> items = new List<ProductPurchase>();

                items.AddRange(GetTestProductPurchase("apple", 3));
                items.AddRange(GetTestProductPurchase("banana", 4));
                items.AddRange(GetTestProductPurchase("orange", 9));

                return items;
            }
        }

        public static IEnumerable<IEnumerable<ProductPurchase>> SampleBaskets
        {
            get
            {
                yield return new List<ProductPurchase>(Basket1);
                yield return new List<ProductPurchase>(Basket2);
                yield return new List<ProductPurchase>(Basket3);
            }
        }

        public static Product GetTestProduct(string name)
        {
            return TestProducts.Single(g => g.Name == name);
        }

        private static IEnumerable<ProductPurchase> GetTestProductPurchase(string name, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new ProductPurchase(TestProducts.Single(gi => gi.Name == name));
            }
        }

        public static double GetRandomNumber()
        {
            Random random = new Random();
            double next = random.NextDouble();
            return next;
        }
    }
}