using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using Utility;
using System.IO;
using BusinessProcess;
using GroceryCheckOutSystem.DataAccess;

namespace GroceryCheckOutSystem.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                System.Console.Write("Select application mode ([C]heckout,[S]ettings)");

                string mode = System.Console.ReadLine()?.ToLowerInvariant();

                switch (mode)
                {
                    case "s":
                        System.Console.WriteLine("Entering settings...");
                        SettingsBP settings = new SettingsBP();
                        settings.Start();
                        break;

                    case "c":
                        System.Console.WriteLine("Entering checkout mode...");
                        CheckOutBP checkOutBp = new CheckOutBP();
                        checkOutBp.Start();
                        break;

                    default:
                        System.Console.WriteLine("Invalid selection.");
                        break;
                }
            }

            List<Product> productList = new List<Product>()
            {
                new Product("Banana",1),
                //new Product("Apple",2),
                //new Product("Orange",3),
                //new Product("Butter",4),
                //new Product("Pineapple",5),
                //new Product("Mango",6),
                //new Product("Strawberry",7),
            };
            List<Promotion> PromotionList = new List<Promotion>()
            {
                new Promotion(Guid.NewGuid(),1,0.6,0),
                //new Product("Apple",2),
                //new Product("Orange",3),
                //new Product("Butter",4),
                //new Product("Pineapple",5),
                //new Product("Mango",6),
                //new Product("Strawberry",7),
            };
            //List<PromotionType> promotionTypes = new List<PromotionType>()
            //{
            //    new PromotionType("OnSale", "0"),
            //    new PromotionType("Group", "1"),
            //    new PromotionType("AdditionalProduct", "2"),
            //};

            //Repository repositoryBp = new Repository();
            // repositoryBp.PromotionTypeRepository.UpSert(promotionTypes);
            //repositoryBp.UpSert(productList);
            ////string xml = ParseHelper.ToXML(productList);
            ////System.Console.WriteLine(xml);
            ////File.WriteAllText(Directory.GetCurrentDirectory() + "Product.xml", xml);


            //Repository repositoryBp = new Repository();
            //repositoryBp.ProductRepository.UpSert(productList);
            //repositoryBp.PromotionRepository.UpSert(PromotionList);
            ////string xml = ParseHelper.ToXML(productList);
            ////System.Console.WriteLine(xml);
            ////File.WriteAllText(Directory.GetCurrentDirectory() + "Product.xml", xml);
            //List<Product> tempProd = repositoryBp.ProductRepository.GetAll();
            //foreach (Product prod in tempProd)
            //{
            //    System.Console.WriteLine(prod.Name + " " + prod.Price);
            //}
            //List<Promotion> tempPromo = repositoryBp.PromotionRepository.GetAll();
            //foreach (Promotion promo in tempPromo)
            //{
            //    System.Console.WriteLine(promo.PromotionId + " " + promo.Discount);
            //}
        }
    }
}
