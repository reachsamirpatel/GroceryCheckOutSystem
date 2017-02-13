using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using System.Windows.Forms;
using GroceryCheckOut.Entity.Enums;
using GroceryCheckOutSystem.DataAccess;
using Logging;


namespace BusinessProcess
{
    public class CheckOutBP
    {

        private PromotionBP _promotionBp;
        private List<Product> _productList;
        private List<Promotion> _promotionList;
        private Repository _repository;
        public User CurrentUser;
        private ILogger _log;
        public CheckOutBP(User user)
        {
            Initialize(user);
        }


        private void Initialize(User user)
        {
            _promotionBp = new PromotionBP();
            _repository = new Repository();
            _promotionList = _repository.PromotionRepository.GetAll();
            _productList = _repository.ProductRepository.GetAll();
            CurrentUser = user;
            _log = LogManager.GetLogger(this);
        }

        private void Checkout(List<ProductPurchase> basketItems, List<Promotion> effectivePromotions)
        {
            try
            {
                _promotionBp.ApplyManyPromotions(effectivePromotions, basketItems);
                DisplayReciept(basketItems);
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                throw;
            }
        }

        private void DisplayReciept(List<ProductPurchase> purchase)
        {
            try
            {
                ReceiptBP receiptBp = new ReceiptBP();
                string receiptContent = receiptBp.CreateReceipt(purchase, CurrentUser);
                File.WriteAllText(ConfigurationManager.AppSettings["ReceiptFileName"], receiptContent);
                ProcessStartInfo startInfo = new ProcessStartInfo("notepad.exe")
                {
                    Arguments = ConfigurationManager.AppSettings["ReceiptFileName"],
                    WindowStyle = ProcessWindowStyle.Maximized
                };
                Process.Start(startInfo);
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                throw;
            }
        }

        private void StartCheckout()
        {
            try
            {
                List<ProductPurchase> purchasedItems = new List<ProductPurchase>();
                List<Promotion> effectivePromotions = new List<Promotion>();

                IEnumerable<string> basketContents = GetCheckOutItems();

                foreach (string basketItem in basketContents)
                {
                    Product purchasedGroceryItem = _productList.Single(i => i.Name == basketItem);
                    purchasedItems.Add(new ProductPurchase(purchasedGroceryItem));

                    Promotion promotion = _promotionList.SingleOrDefault(p => p.Name == purchasedGroceryItem.Name);

                    if ((promotion != null) && !effectivePromotions.Exists(p => p.Name == promotion.Name))
                    {
                        if (promotion.EndDate != null &&
                            (promotion.StartDate != null &&
                             (DateTime.Now.Ticks >= promotion.StartDate.Value.Ticks &&
                              DateTime.Now.Ticks < promotion.EndDate.Value.Ticks)))
                            effectivePromotions.Add(promotion);
                        else if (promotion.StartDate == null || promotion.EndDate == null)
                        {
                            effectivePromotions.Add(promotion);
                        }
                    }
                }

                Checkout(purchasedItems, effectivePromotions);
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                throw;
            }
        }

        private List<string> GetCheckOutItems()
        {
            try
            {
                List<string> productNameList = _productList.Select(x => x.Name).ToList();
                List<string> basketItemList =
                    File.ReadAllText(ConfigurationManager.AppSettings["BasketFileName"]).Split(',').ToList();
                //Removing items that are not present int the product catalog.
                basketItemList.RemoveAll(item => !productNameList.Contains(item));
                return basketItemList;
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                throw;
            }
        }

        public void Start()
        {
            StartCheckout();
        }
    }
}
