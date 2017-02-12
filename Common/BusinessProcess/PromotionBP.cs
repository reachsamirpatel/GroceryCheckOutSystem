using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using GroceryCheckOut.Entity.Enums;
using GroceryCheckOutSystem.DataAccess;


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
        public Promotion CreatePromotion(Product product, PromotionTypeEnum ipromotion, double reductionRate, int noOfItemsRequired)
        {
            if (ipromotion == PromotionTypeEnum.OnSale)
            {
                Promotion promotion = new Promotion(product.ProductId, ipromotion, reductionRate, noOfItemsRequired);

                promotion.Discount = (product.Price == 0)
                    ? promotion.Discount = 0
                    : promotion.Discount = (double)(reductionRate / product.Price);

                return promotion;
            }
            if (ipromotion == PromotionTypeEnum.Group)
            {
                Promotion promotion = new Promotion(product.ProductId, ipromotion, reductionRate, noOfItemsRequired);

                promotion.Discount = (product.Price == 0)
                    ? promotion.Discount = 0
                    : promotion.Discount = (double)(reductionRate / product.Price);

                return promotion;
            }
            if (ipromotion == PromotionTypeEnum.AdditionalProduct)
            {
                Promotion promotion = new Promotion(product.ProductId, ipromotion, reductionRate, noOfItemsRequired);
                promotion.Discount = (product.Price == 0)
                    ? promotion.Discount = 0
                    : promotion.Discount = (double)(1 - reductionRate / product.Price);
                return promotion;
            }
            return null;
        }

        private void ApplyOnSalePromotion(Promotion promotion, ref List<ProductPurchase> purchaseItems)
        {
            foreach (ProductPurchase item in purchaseItems.Where(pi => pi.Product.ProductId == promotion.ProductId))
            {
                double discountedPrice = (double)item.DiscountedPrice * promotion.Discount;

                item.DiscountedPrice = Math.Round(Convert.ToDouble(discountedPrice), 2);
            }
        }

        private void ApplyGroupPromotion(Promotion promotion, ref List<ProductPurchase> purchaseItems)
        {
            IEnumerable<ProductPurchase> applicableItems = purchaseItems.Where(pi => pi.Product.ProductId == promotion.ProductId).ToList();

            if (promotion.NumberOfItemsRequired == 0)
                return;

            for (int i = 0; i < applicableItems.Count(); i += promotion.NumberOfItemsRequired)
            {
                List<ProductPurchase> group = applicableItems.Skip(i).Take(promotion.NumberOfItemsRequired).ToList();

                if (group.Count() != promotion.NumberOfItemsRequired)
                    continue;

                foreach (ProductPurchase item in group)
                {
                    double discountedPrice = (double)item.DiscountedPrice * promotion.Discount;

                    item.DiscountedPrice = Math.Round(discountedPrice, 2);
                }
            }
        }

        private void ApplyAdditionalProductPromotion(Promotion promotion, ref List<ProductPurchase> purchaseItems)
        {
            IEnumerable<ProductPurchase> applicableItems =
                purchaseItems.Where(pi => pi.Product.ProductId == promotion.ProductId).ToList();

            for (int i = 0; i < applicableItems.Count(); i += promotion.NumberOfItemsRequired + 1)
            {
                IEnumerable<ProductPurchase> group = applicableItems.Skip(i).Take(promotion.NumberOfItemsRequired + 1).ToList();

                if (group.Count() != promotion.NumberOfItemsRequired + 1)
                    continue;

                ProductPurchase item = group.Last();

                double discountedPrice = (double)item.DiscountedPrice * promotion.Discount;

                item.DiscountedPrice = Math.Round(discountedPrice, 2);
            }
        }
        public void ApplyManyPromotions(List<Promotion> promotions, ref List<ProductPurchase> purchasedItemList)
        {
            foreach (Promotion promotion in promotions)
            {
                ApplyPromotion(promotion, ref purchasedItemList);
            }
        }

        private void ApplyPromotion(Promotion promotion, ref List<ProductPurchase> purchasedItemList)
        {
            switch (promotion.PromotionType)
            {
                case PromotionTypeEnum.OnSale:
                    ApplyOnSalePromotion(promotion, ref purchasedItemList);
                    break;

                case PromotionTypeEnum.Group:
                    ApplyGroupPromotion(promotion, ref purchasedItemList);
                    break;

                case PromotionTypeEnum.AdditionalProduct:
                    ApplyAdditionalProductPromotion(promotion, ref purchasedItemList);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

