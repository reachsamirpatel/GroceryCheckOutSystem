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
using GroceryCheckOut.Entity.Enums;
using GroceryCheckOutSystem.DataAccess;

namespace GroceryCheckOutSystem.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<Login> loginList = new List<Login>
            //{
            //    new Login("clerk1",Encryptor.Base64Encode("test12"),new Guid("6fef5486-fd19-4ead-8090-e93dfc437641")),
            //    new Login("marketing1",Encryptor.Base64Encode("test12"),new Guid("0326c248-106f-4990-82c3-b41dd6e1c553")),
            //    new Login("clerk1",Encryptor.Base64Encode("test12"),new Guid("ad6deb47-dbc8-4c79-a9d3-6573af2f56c4"))
            //};
            //Repository repositoryBp = new Repository();
            //repositoryBp.LoginRepository.UpSert(loginList);
            //foreach (Login login in loginList)
            //{
            //    System.Console.WriteLine("{0}==>{1}", login.LoginName, Encryptor.Base64Decode(login.Password));
            //}
            UserBP userBp = new UserBP();
            userBp.Start();
            User currentUser = userBp.CurrentUser;
            if (currentUser.UserType == UserTypeEnum.Admin)
            {
                while (true)
                {
                    System.Console.Write("Select application mode ([C]heckout,[S]ettings)");

                    string mode = System.Console.ReadLine()?.ToLowerInvariant();
                    System.Console.WriteLine("You have selected {0}", mode);
                    switch (mode)
                    {
                        case "c":
                            System.Console.WriteLine("Entering checkout mode...");
                            CheckOutBP checkOutBp = new CheckOutBP(userBp.CurrentUser);
                            checkOutBp.Start();
                            break;
                        case "s":
                            System.Console.WriteLine("Entering settings...");
                            SettingsBP settings = new SettingsBP(userBp.CurrentUser);
                            settings.Start();
                            break;
                        default:
                            System.Console.WriteLine("Invalid selection.");
                            break;
                    }
                }
            }
            if (currentUser.UserType == UserTypeEnum.Marketing)
            {
                System.Console.WriteLine("You have access to Settings only.");
                while (true)
                {
                    System.Console.Write("Select application mode ([E]xit,[S]ettings)");

                    string mode = System.Console.ReadLine()?.ToLowerInvariant();
                    switch (mode)
                    {
                        case "e":
                            return;
                            break;
                        case "s":
                            System.Console.WriteLine("Entering settings...");
                            SettingsBP settings = new SettingsBP(userBp.CurrentUser);
                            settings.Start();
                            break;
                        default:
                            System.Console.WriteLine("Invalid selection.");
                            break;
                    }
                }
            }
            if (currentUser.UserType == UserTypeEnum.Clerk)
            {
                System.Console.WriteLine("You have access to checkout only.");
                while (true)
                {
                    System.Console.Write("Select application mode ([E]xit,[C]heckout)");

                    string mode = System.Console.ReadLine()?.ToLowerInvariant();
                    switch (mode)
                    {
                        case "e":
                            return;
                            break;
                        case "c":
                            System.Console.WriteLine("Entering checkout mode...");
                            CheckOutBP checkOutBp = new CheckOutBP(userBp.CurrentUser);
                            checkOutBp.Start();
                            break;
                        default:
                            System.Console.WriteLine("Invalid selection.");
                            break;
                    }
                }

            }
            //List<User> userList = new List<User>()
            //{
            //    new User("Mike","Anderson","mike@yopmail.com",UserTypeEnum.Clerk),
            //    new User("Jessica","Simpson","mike@yopmail.com",UserTypeEnum.Marketing),
            //    new User("John","Kennedy","mike@yopmail.com",UserTypeEnum.Admin)
            //};
            //Repository repositoryBp = new Repository();
            //repositoryBp.UserRepository.UpSert(userList);
            //List<Product> productList = new List<Product>()
            //{
            //    new Product("Banana",1),
            //    //new Product("Apple",2),
            //    //new Product("Orange",3),
            //    //new Product("Butter",4),
            //    //new Product("Pineapple",5),
            //    //new Product("Mango",6),
            //    //new Product("Strawberry",7),
            //};
            //List<Promotion> PromotionList = new List<Promotion>()
            //{
            //    new Promotion(Guid.NewGuid(),1,0.6,0),
            //    //new Product("Apple",2),
            //    //new Product("Orange",3),
            //    //new Product("Butter",4),
            //    //new Product("Pineapple",5),
            //    //new Product("Mango",6),
            //    //new Product("Strawberry",7),
            //};
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
