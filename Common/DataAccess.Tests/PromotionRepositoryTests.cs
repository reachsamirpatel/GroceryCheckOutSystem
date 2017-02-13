using NUnit.Framework;
using GroceryCheckOutSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Tests;
using GroceryCheckOut.Entity;
using GroceryCheckOut.Entity.Enums;

namespace GroceryCheckOutSystem.DataAccess.Tests
{
    [TestFixture()]
    public class PromotionRepositoryTests
    {
        [Test()]
        public void GetAllPromotionTest()
        {
            PromotionRepository promotionRepository = new PromotionRepository();
            List<Promotion> promotions = promotionRepository.GetAll();
            List<Promotion> expectedList = TestHelper.TestPromotions.ToList();
            foreach (Promotion promotion in promotions)
            {
                Promotion output = expectedList.Find(x => x.Name == promotion.Name);
                Assert.That(output != null);
            }
        }

        [Test()]
        public void UpSertPromotionTest()
        {
            PromotionRepository promotionRepository = new PromotionRepository();
            Promotion promotion = new Promotion("xyz", PromotionTypeEnum.OnSale, 1, 0);
            List<Promotion> promotionList = promotionRepository.GetAll();
            int promotionListCount = promotionList.Count;
            promotionList.Add(promotion);
            promotionRepository.UpSert(promotionList);
            promotionList = promotionRepository.GetAll();
            Assert.That(promotionListCount + 1 == promotionList.Count);
            promotionList.RemoveAt(promotionList.Count - 1);
            promotionRepository.UpSert(promotionList);
        }
    }
}