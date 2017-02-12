using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using GroceryCheckOut.Entity.Enums;
using GroceryCheckOutSystem.DataAccess;
using Interfaces;

namespace BusinessProcess
{
    public class PromotionBP
    {
        private Repository _repository;
        private List<Promotion> _promotionList;

        private void Initialize()
        {
            _repository = new Repository();
            _promotionList = _repository.PromotionRepository.GetAll();
        }

        public PromotionBP()
        {
            Initialize();
        }
        public Promotion CreatePromotion(Product product, int ipromotion, double reductionRate, int noOfItemsRequired)
        {
            int onSale = (int)PromotionTypeEnum.OnSale;
            int group = (int)PromotionTypeEnum.Group;
            int additional = (int)PromotionTypeEnum.AdditionalProduct;
            if (ipromotion == onSale)
            {
                Promotion promotion = new Promotion(product.ProductId, onSale, reductionRate, noOfItemsRequired);

                promotion.Discount = (product.Price == (double)0)
                    ? promotion.Discount = 0
                    : promotion.Discount = (double)(reductionRate / product.Price);

                return promotion;
            }
            if (ipromotion == group)
            {
                Promotion promotion = new Promotion(product.ProductId, group, reductionRate, noOfItemsRequired);

                promotion.Discount = (product.Price == (double)0)
                    ? promotion.Discount = 0
                    : promotion.Discount = (double)(reductionRate / product.Price);

                return promotion;
            }
            if (ipromotion == additional)
            {
                Promotion promotion = new Promotion(product.ProductId, additional, reductionRate, noOfItemsRequired);
                promotion.Discount = (product.Price == (double)0)
                    ? promotion.Discount = 0
                    : promotion.Discount = (double)(1 - reductionRate / product.Price);
                return promotion;
            }
            return null;
        }
        public void ApplyOnSalePromotion(Promotion promotion, ref List<ProductPurchase> PurchaseItems)
        {
            foreach (ProductPurchase item in PurchaseItems.Where(pi => pi.Product.ProductId == promotion.ProductId))
            {
                double discountedPrice = (double)item.DiscountedPrice * promotion.Discount;

                item.DiscountedPrice = Math.Round(Convert.ToDouble(discountedPrice), 2);
            }
        }

        public void ApplyGroupPromotion(Promotion promotion, ref List<ProductPurchase> PurchaseItems)
        {
            IEnumerable<ProductPurchase> applicableItems = PurchaseItems.Where(pi => pi.Product.ProductId == promotion.ProductId).ToList();

            if (promotion.NumberOfItemsRequired == 0)
                return;

            for (int i = 0; i < applicableItems.Count(); i += promotion.NumberOfItemsRequired)
            {
                IEnumerable<ProductPurchase> group = applicableItems.Skip(i).Take(promotion.NumberOfItemsRequired).ToList();

                if (group.Count() != promotion.NumberOfItemsRequired)
                    continue;

                foreach (ProductPurchase item in group)
                {
                    double discountedPrice = (double)item.DiscountedPrice * promotion.Discount;

                    item.DiscountedPrice = Math.Round(discountedPrice, 2);
                }
            }
        }

        public void ApplyAdditionalProductPromotion(Promotion promotion, ref List<ProductPurchase> PurchaseItems)
        {
            IEnumerable<ProductPurchase> applicableItems =
                PurchaseItems.Where(pi => pi.Product.ProductId == promotion.ProductId).ToList();

            for (int i = 0; i < applicableItems.Count(); i += promotion.NumberOfItemsRequired + 1)
            {
                IEnumerable<ProductPurchase> group = applicableItems.Skip(i).Take(promotion.NumberOfItemsRequired + 1).ToList();

                if (group.Count() != promotion.NumberOfItemsRequired + 1) continue;

                ProductPurchase item = group.Last();

                double discountedPrice = (double)item.DiscountedPrice * promotion.Discount;

                item.DiscountedPrice = Math.Round(discountedPrice, 2);
            }
        }
        public void ApplyManyPromotions(List<Promotion> promotions, ref List<ProductPurchase> _purchasedItemList)
        {
            foreach (Promotion promotion in promotions)
            {
                ApplyPromotion(promotion, ref _purchasedItemList);
            }
        }

        public void ApplyPromotion(Promotion promotion, ref List<ProductPurchase> _purchasedItemList)
        {
            switch (promotion.PromotionType)
            {
                case (int)PromotionTypeEnum.OnSale:
                    ApplyOnSalePromotion(promotion, ref _purchasedItemList);
                    break;

                case (int)PromotionTypeEnum.Group:
                    ApplyGroupPromotion(promotion, ref _purchasedItemList);
                    break;

                case (int)PromotionTypeEnum.AdditionalProduct:
                    ApplyAdditionalProductPromotion(promotion, ref _purchasedItemList);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

