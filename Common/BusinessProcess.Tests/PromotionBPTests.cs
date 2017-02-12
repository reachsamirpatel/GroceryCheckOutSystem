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
    public class PromotionBPTests
    {
        [Test]
        public void PromotionBPTest()
        {
            Assert.Pass();
        }

        [Test]
        public void CreatePromotionTest()
        {
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            int itemsRequired = new Random().Next(3);
            PromotionBP promotionBp = new PromotionBP();
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.Group, salePrice, itemsRequired)).ToList();

            List<ProductPurchase> purchaseList = TestHelper.Basket2.ToList();
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
        public void CreateExpiredPromotionTest()
        {
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            int itemsRequired = new Random().Next(3);
            PromotionBP promotionBp = new PromotionBP();
            DateTime? startPromotion = DateTime.Now.AddMonths(-1);
            DateTime? endPromotion = DateTime.Now.AddDays(-7);
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.Group, salePrice, itemsRequired, startPromotion, endPromotion)).ToList();

            List<ProductPurchase> purchaseList = TestHelper.Basket2.ToList();
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
        public void CreateNonExpiredPromotionTest()
        {
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            int itemsRequired = new Random().Next(3);
            PromotionBP promotionBp = new PromotionBP();
            DateTime? startPromotion = DateTime.Now.AddMonths(-1);
            DateTime? endPromotion = DateTime.Now.AddDays(30);
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.Group, salePrice, itemsRequired, startPromotion, endPromotion)).ToList();

            List<ProductPurchase> purchaseList = TestHelper.Basket2.ToList();
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
        public void ApplyManyPromotionsTest()
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
        public void ApplyManyExpiredPromotionsTest()
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
        public void ApplyManyNonExpiredPromotionsTest()
        {
            PromotionBP promotionBp = new PromotionBP();
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            DateTime? startPromotion = DateTime.Now.AddMonths(-1);
            DateTime? endPromotion = DateTime.Now.AddDays(7);
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.OnSale, salePrice, 0, startPromotion, endPromotion)).ToList();

            List<ProductPurchase> basketList = promotionBp.ApplyManyPromotions(promotions, TestHelper.Basket1.ToList());

            foreach (string itemName in TestHelper.ItemNames)
            {
                List<ProductPurchase> items = basketList.Where(pi => pi.Product.Name == itemName).ToList();

                Assert.That(items.All(a => a.DiscountedPrice == salePrice));
            }
        }
        [Test]
        public void TestPromotionWithNoDiscount()
        {
            Product foo = new Product("foo", 0);
            PromotionBP promotionBp = new PromotionBP();

            Promotion promotion = promotionBp.CreatePromotion(foo, PromotionTypeEnum.OnSale, 0, 0);

            Assert.That(Math.Abs(promotion.Discount) < 0.0001);
        }

    }
}