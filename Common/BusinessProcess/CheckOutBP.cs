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
using Interfaces;

namespace BusinessProcess
{
    public class CheckOutBP
    {
        private List<ProductPurchase> _purchasedItemList;
        private PromotionBP _promotionBp;
        private List<Product> _productList;
        private List<Promotion> _promotionList;
        private Repository _repository;
        public CheckOutBP()
        {
            Initialize();
        }

        public CheckOutBP(List<ProductPurchase> productPurchaseList)
        {
            _purchasedItemList = productPurchaseList;
            Initialize();
        }

        private void Initialize()
        {
            _purchasedItemList = new List<ProductPurchase>();
            _promotionBp = new PromotionBP();
            _repository = new Repository();
            _promotionList = _repository.PromotionRepository.GetAll();
            _productList = _repository.ProductRepository.GetAll();

        }

        public void Checkout(List<ProductPurchase> basketItems, List<Promotion> effectivePromotions)
        {
            //  ProductPurchase purchase = new ProductPurchase();

            _promotionBp.ApplyManyPromotions(effectivePromotions, ref basketItems);

            DisplayReciept(basketItems);
        }

        private void DisplayReciept(List<ProductPurchase> purchase)
        {
            // string receiptPath = Path.Combine(ConfigurationManager.AppSettings["ReceiptPath"], "receipt.txt");
            ReceiptBP receiptBp = new ReceiptBP();
            string receiptContent = receiptBp.CreateReceipt(purchase);
            File.WriteAllText(ConfigurationManager.AppSettings["ReceiptPath"], receiptContent);
            Process.Start(ConfigurationManager.AppSettings["ReceiptPath"]);
        }
        private void StartCheckout()
        {
            List<ProductPurchase> purchasedItems = new List<ProductPurchase>();
            List<Promotion> effectivePromotions = new List<Promotion>();

            IEnumerable<string> basketContents = GetBasketContents();

            foreach (string basketItem in basketContents)
            {
                Product purchasedGroceryItem = _productList.Single(i => i.Name == basketItem);
                purchasedItems.Add(new ProductPurchase(purchasedGroceryItem));

                Promotion promotion = _promotionList.SingleOrDefault(p => p.ProductId == purchasedGroceryItem.ProductId);

                if ((promotion != null) && !effectivePromotions.Exists(p => p.ProductId == promotion.ProductId))
                    effectivePromotions.Add(promotion);
            }

            Checkout(purchasedItems, effectivePromotions);
        }

        private List<string> GetBasketContents()
        {
            List<string> productNameList = _repository.ProductRepository.GetAll().Select(x => x.Name).ToList();
            List<string> basketItemList = File.ReadAllText(ConfigurationManager.AppSettings["BasketFileName"]).Split(',').ToList();
            basketItemList.RemoveAll(item => !productNameList.Contains(item));
            return basketItemList;
        }

        public void Start()
        {
            StartCheckout();

        }

    }
}
