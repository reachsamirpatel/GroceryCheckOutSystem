using System;
using System.Collections.Generic;
using System.Linq;
using GroceryCheckOut.Entity;
using GroceryCheckOut.Entity.Enums;

namespace BusinessProcess.Tests
{
    public class TestHelper
    {

        public static IEnumerable<User> UserList
        {
            get
            {
                yield return new User(new Guid("6fef5486-fd19-4ead-8090-e93dfc437641"), "Mike", "Anderson", "mike@yopmail.com", UserTypeEnum.Clerk);
                yield return new User(new Guid("0326c248-106f-4990-82c3-b41dd6e1c553"), "Jessica", "Simpson", "jessica@yopmail.com", UserTypeEnum.Marketing);
                yield return new User(new Guid("ad6deb47-dbc8-4c79-a9d3-6573af2f56c4"), "John", "Kennedy", "john@yopmail.com", UserTypeEnum.Admin);
            }
        }
        public static IEnumerable<Login> LoginList
        {
            get
            {
                yield return new Login("clerk1", "test12", new Guid("6fef5486-fd19-4ead-8090-e93dfc437641"));
                yield return new Login("marketing1", "test12", new Guid("0326c248-106f-4990-82c3-b41dd6e1c553"));
                yield return new Login("admin1", "test12", new Guid("ad6deb47-dbc8-4c79-a9d3-6573af2f56c4"));
            }
        }
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