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
    /// <summary>
    /// Class to perform promotion related operations
    /// </summary>
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
        /// <summary>
        /// Constructor to create promotion object
        /// </summary>
        /// <param name="product"></param>
        /// <param name="ipromotion"></param>
        /// <param name="reductionRate"></param>
        /// <param name="noOfItemsRequired"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public Promotion CreatePromotion(Product product, PromotionTypeEnum ipromotion, double reductionRate, int noOfItemsRequired, DateTime? startDate = null, DateTime? endDate = null, Guid? createdBy = null)
        {
            if (ipromotion == PromotionTypeEnum.OnSale)
            {
                Promotion promotion = new Promotion(product.Name, ipromotion, reductionRate, noOfItemsRequired, startDate, endDate, createdBy);

                promotion.Discount = (product.Price == 0)
                    ? promotion.Discount = 0
                    : promotion.Discount = (double)(reductionRate / product.Price);

                return promotion;
            }
            if (ipromotion == PromotionTypeEnum.Group)
            {
                Promotion promotion = new Promotion(product.Name, ipromotion, reductionRate, noOfItemsRequired, startDate, endDate, createdBy);

                promotion.Discount = (product.Price == 0)
                    ? promotion.Discount = 0
                    : promotion.Discount = (double)(reductionRate / product.Price);

                return promotion;
            }
            if (ipromotion == PromotionTypeEnum.AdditionalProduct)
            {
                Promotion promotion = new Promotion(product.Name, ipromotion, reductionRate, noOfItemsRequired, startDate, endDate, createdBy);
                promotion.Discount = (product.Price == 0)
                    ? promotion.Discount = 0
                    : promotion.Discount = (double)(1 - reductionRate / product.Price);
                return promotion;
            }
            return null;
        }

        /// <summary>
        /// Method to check whether promotion is expired or not
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        private bool CheckPromotionExpiry(Promotion promotion)
        {
            if (promotion.StartDate == null || promotion.EndDate == null)
                return false;
            if (DateTime.Now.Ticks >= promotion.StartDate.Value.Ticks && DateTime.Now.Ticks < promotion.EndDate.Value.Ticks)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method to apply OnSale Promtion
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="purchaseItems"></param>

        private void ApplyOnSalePromotion(Promotion promotion, List<ProductPurchase> purchaseItems)
        {
            foreach (ProductPurchase item in purchaseItems.Where(pi => pi.Product.Name == promotion.Name))
            {
                double discountedPrice = (double)item.DiscountedPrice * promotion.Discount;

                item.DiscountedPrice = Math.Round(Convert.ToDouble(discountedPrice), 2);
            }
        }
        /// <summary>
        /// Method to apply Group promotion
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="purchaseItems"></param>
        private void ApplyGroupPromotion(Promotion promotion, List<ProductPurchase> purchaseItems)
        {
            IEnumerable<ProductPurchase> applicableItems = purchaseItems.Where(pi => pi.Product.Name == promotion.Name).ToList();

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
        /// <summary>
        /// Method to apply addition product promotion
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="purchaseItems"></param>
        private void ApplyAdditionalProductPromotion(Promotion promotion, List<ProductPurchase> purchaseItems)
        {
            IEnumerable<ProductPurchase> applicableItems =
            purchaseItems.Where(pi => pi.Product.Name == promotion.Name).ToList();

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

        /// <summary>
        /// Main promotion method which loops through purchased product and apply promotions
        /// </summary>
        /// <param name="promotions"></param>
        /// <param name="purchasedItemList"></param>
        /// <returns></returns>
        public List<ProductPurchase> ApplyManyPromotions(List<Promotion> promotions, List<ProductPurchase> purchasedItemList)
        {
            foreach (Promotion promotion in promotions)
            {
                if (CheckPromotionExpiry(promotion))
                    continue;
                ApplyPromotion(promotion, purchasedItemList);
            }
            return purchasedItemList;
        }
        /// <summary>
        /// Method to determine type of promotion to applly on purchased product.
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="purchasedItemList"></param>
        private void ApplyPromotion(Promotion promotion, List<ProductPurchase> purchasedItemList)
        {
            switch (promotion.PromotionType)
            {
                case PromotionTypeEnum.OnSale:
                    ApplyOnSalePromotion(promotion, purchasedItemList);
                    break;

                case PromotionTypeEnum.Group:
                    ApplyGroupPromotion(promotion, purchasedItemList);
                    break;

                case PromotionTypeEnum.AdditionalProduct:
                    ApplyAdditionalProductPromotion(promotion, purchasedItemList);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

