using BusinessProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using GroceryCheckOut.Entity.Enums;
using NUnit.Framework;

namespace BusinessProcess.Tests
{
    [TestFixture]
    public class CheckOutBPTests
    {

        [Test]
        public void DiscountSameAsRegularPriceTest()
        {
            List<ProductPurchase> items = TestHelper.Basket1.ToList();

            for (int i = 0; i < items.Count(); i++)
            {
                Assert.AreEqual(items.ElementAt(i).Product.Price, items.ElementAt(i).DiscountedPrice);
            }
        }

        [Test]
        public void OnSalePromotionTest()
        {
            PromotionBP promotionBp = new PromotionBP();
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.OnSale, salePrice, 0)).ToList();

            List<ProductPurchase> basketList = promotionBp.ApplyManyPromotions(promotions, TestHelper.Basket1.ToList());

            foreach (string itemName in TestHelper.ItemNames)
            {
                List<ProductPurchase> items = basketList.Where(pi => pi.Product.Name == itemName).ToList();

                Assert.That(items.All(a => a.DiscountedPrice == salePrice));
            }
        }

        [Test]
        public void GroupPromotionTest()
        {
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            int itemsRequired = new Random().Next(5);
            PromotionBP promotionBp = new PromotionBP();
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.Group, salePrice, itemsRequired)).ToList();

            List<ProductPurchase> purchaseList = TestHelper.Basket1.ToList();
            promotionBp.ApplyManyPromotions(promotions, purchaseList);

            foreach (string itemName in TestHelper.ItemNames)
            {
                int applicableItemsCount = purchaseList.Count(pi => pi.Product.Name == itemName);
                int discountedItemsCount = purchaseList.Count(
                    p => (p.Product.Name == itemName) && (p.DiscountedPrice == salePrice));

                Assert.That((discountedItemsCount == 0) || (applicableItemsCount % discountedItemsCount < itemsRequired));
            }
        }

        [Test]
        public void AdditionalItemPromotionTest()
        {
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            const int itemsRequired = 1;
            PromotionBP promotionBp = new PromotionBP();
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.AdditionalProduct, salePrice, itemsRequired)).ToList();

            List<ProductPurchase> purchaseList = TestHelper.Basket4.ToList();
            purchaseList = promotionBp.ApplyManyPromotions(promotions, purchaseList);

            foreach (string itemName in TestHelper.ItemNames)
            {
                double applicableItemsCount = purchaseList.Count(pi => pi.Product.Name == itemName);
                double discountedItemsCount = purchaseList.Count(p => (p.Product.Name == itemName) && (p.DiscountedPrice == salePrice));

                int expectedDiscountedItemsCount = (int)Math.Floor(applicableItemsCount / (itemsRequired + 1));

                Assert.That(expectedDiscountedItemsCount == (int)discountedItemsCount);
            }
        }


        [Test]
        public void OnSaleExpiredPromotionTest()
        {
            PromotionBP promotionBp = new PromotionBP();
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            DateTime? startPromotion = DateTime.Now.AddMonths(-1);
            DateTime? endPromotion = DateTime.Now.AddDays(-7);
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.OnSale, salePrice, 0, startPromotion, endPromotion)).ToList();

            List<ProductPurchase> basketList = promotionBp.ApplyManyPromotions(promotions, TestHelper.Basket1.ToList());

            foreach (string itemName in TestHelper.ItemNames)
            {
                List<ProductPurchase> items = basketList.Where(pi => pi.Product.Name == itemName).ToList();

                Assert.That(items.All(a => a.DiscountedPrice != salePrice));
            }
        }

        [Test]
        public void GroupExpiredPromotionTest()
        {
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            int itemsRequired = new Random().Next(3);
            PromotionBP promotionBp = new PromotionBP();
            DateTime? startPromotion = DateTime.Now.AddMonths(-1);
            DateTime? endPromotion = DateTime.Now.AddDays(-7);
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.Group, salePrice, itemsRequired, startPromotion, endPromotion)).ToList();

            List<ProductPurchase> purchaseList = TestHelper.Basket4.ToList();
            promotionBp.ApplyManyPromotions(promotions, purchaseList);

            foreach (string itemName in TestHelper.ItemNames)
            {
                int applicableItemsCount = purchaseList.Count(pi => pi.Product.Name == itemName);
                int discountedItemsCount = purchaseList.Count(
                    p => (p.Product.Name == itemName) && (p.DiscountedPrice == salePrice));

                Assert.That((discountedItemsCount == 0) || (applicableItemsCount % discountedItemsCount < itemsRequired));
            }
        }

        [Test]
        public void AdditionalItemExpiredPromotionTest()
        {
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            const int itemsRequired = 1;
            PromotionBP promotionBp = new PromotionBP();
            DateTime? startPromotion = DateTime.Now.AddMonths(-1);
            DateTime? endPromotion = DateTime.Now.AddDays(-7);
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.AdditionalProduct, salePrice, itemsRequired, startPromotion, endPromotion)).ToList();

            List<ProductPurchase> purchaseList = TestHelper.Basket2.ToList();
            purchaseList = promotionBp.ApplyManyPromotions(promotions, purchaseList);

            foreach (string itemName in TestHelper.ItemNames)
            {
                double applicableItemsCount = purchaseList.Count(pi => pi.Product.Name == itemName);
                double discountedItemsCount = purchaseList.Count(p => (p.Product.Name == itemName) && (p.DiscountedPrice == salePrice));

                int expectedDiscountedItemsCount = (int)Math.Floor(applicableItemsCount / (itemsRequired + 1));

                Assert.That(expectedDiscountedItemsCount != (int)discountedItemsCount);
            }
        }

        [Test]
        public void OnSaleNonExpiredPromotionTest()
        {
            PromotionBP promotionBp = new PromotionBP();
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            DateTime? startPromotion = DateTime.Now.AddMonths(-1);
            DateTime? endPromotion = DateTime.Now.AddDays(30);
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.OnSale, salePrice, 0, startPromotion, endPromotion)).ToList();

            List<ProductPurchase> basketList = promotionBp.ApplyManyPromotions(promotions, TestHelper.Basket1.ToList());

            foreach (string itemName in TestHelper.ItemNames)
            {
                List<ProductPurchase> items = basketList.Where(pi => pi.Product.Name == itemName).ToList();

                Assert.That(items.All(a => a.DiscountedPrice == salePrice));
            }
        }

        [Test]
        public void GroupNonExpiredPromotionTest()
        {
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            int itemsRequired = new Random().Next(3);
            PromotionBP promotionBp = new PromotionBP();
            DateTime? startPromotion = DateTime.Now.AddMonths(-1);
            DateTime? endPromotion = DateTime.Now.AddDays(30);
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.Group, salePrice, itemsRequired, startPromotion, endPromotion)).ToList();

            List<ProductPurchase> purchaseList = TestHelper.Basket4.ToList();
            promotionBp.ApplyManyPromotions(promotions, purchaseList);

            foreach (string itemName in TestHelper.ItemNames)
            {
                int applicableItemsCount = purchaseList.Count(pi => pi.Product.Name == itemName);
                int discountedItemsCount = purchaseList.Count(
                    p => (p.Product.Name == itemName) && (p.DiscountedPrice == salePrice));

                Assert.That((discountedItemsCount == 0) || (applicableItemsCount % discountedItemsCount < itemsRequired));
            }
        }

        [Test]
        public void AdditionalItemNonExpiredPromotionTest()
        {
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            const int itemsRequired = 1;
            PromotionBP promotionBp = new PromotionBP();
            DateTime? startPromotion = DateTime.Now.AddMonths(-1);
            DateTime? endPromotion = DateTime.Now.AddDays(30);
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.AdditionalProduct, salePrice, itemsRequired, startPromotion, endPromotion)).ToList();

            List<ProductPurchase> purchaseList = TestHelper.Basket2.ToList();
            purchaseList = promotionBp.ApplyManyPromotions(promotions, purchaseList);

            foreach (string itemName in TestHelper.ItemNames)
            {
                double applicableItemsCount = purchaseList.Count(pi => pi.Product.Name == itemName);
                double discountedItemsCount = purchaseList.Count(p => (p.Product.Name == itemName) && (p.DiscountedPrice == salePrice));

                int expectedDiscountedItemsCount = (int)Math.Floor(applicableItemsCount / (itemsRequired + 1));

                Assert.That(expectedDiscountedItemsCount != (int)discountedItemsCount);
            }
        }
    }
}