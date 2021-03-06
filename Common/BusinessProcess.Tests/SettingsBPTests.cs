﻿using BusinessProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using GroceryCheckOut.Entity.Enums;
using NUnit.Framework;
using System.IO;

namespace BusinessProcess.Tests
{
    [TestFixture]
    public class SettingsBPTests
    {
        [Test]
        public void AddOnSalePromotionTest()
        {
            PromotionBP promotionBp = new PromotionBP();
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            List<Promotion> promotions = TestHelper.ItemNames.Select(itemName => promotionBp.CreatePromotion(TestHelper.GetTestProduct(itemName), PromotionTypeEnum.OnSale, salePrice, 0)).ToList();

            List<ProductPurchase> basketList = promotionBp.ApplyManyPromotions(promotions, TestHelper.Basket2.ToList());

            foreach (string itemName in TestHelper.ItemNames)
            {
                List<ProductPurchase> items = basketList.Where(pi => pi.Product.Name == itemName).ToList();

                Assert.That(items.All(a => a.DiscountedPrice == salePrice));
            }
        }
        [Test]
        public void AddGroupPromotionTest()
        {
            double salePrice = Math.Round(TestHelper.GetRandomNumber());
            int itemsRequired = new Random().Next(4);
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
        public void AddAdditionalProductPromotionTest()
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
    }
}