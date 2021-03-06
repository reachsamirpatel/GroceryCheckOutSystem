﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using GroceryCheckOut.Entity.Enums;
using GroceryCheckOutSystem.DataAccess;
using Logging;
using Utility;

namespace BusinessProcess
{
    /// <summary>
    /// Class to modifying or creating promotions
    /// </summary>
    public class SettingsBP
    {
        private Repository _repository;
        private PromotionBP _promotionBp;
        private List<Promotion> _promotionList;
        private List<Product> _productList;
        private ILogger _log;
        public User CurrentUser;
        public SettingsBP(User user)
        {
            Initialize(user);
        }

        private void Initialize(User user)
        {
            _repository = new Repository();
            _promotionBp = new PromotionBP();
            CurrentUser = user;
            _promotionList = _repository.PromotionRepository.GetAll();
            _productList = _repository.ProductRepository.GetAll();
            _log = LogManager.GetLogger(this);
        }
        /// <summary>
        /// Main wrapper to add promotion or end existing promotion
        /// </summary>
        public void Start()
        {
            try
            {
                bool goBack = false;

                while (!goBack)
                {
                    Console.WriteLine("Select an action...");
                    Console.Write("[A]dd a new Promotion | [E]nd an existing Promotion | Go [b]ack:");

                    string action = Console.ReadLine()?.ToLowerInvariant();
                    Console.WriteLine("You have selected : {0}", action);
                    switch (action)
                    {
                        case "a":
                            StartPromotion();
                            break;

                        case "e":
                            EndPromotion();
                            break;

                        case "b":
                            goBack = true;
                            break;

                        default:
                            Console.WriteLine("Invalid selection.");
                            break;
                    }
                }
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                throw;
            }
        }
        /// <summary>
        /// Method to end promotion
        /// </summary>
        private void EndPromotion()
        {
            try
            {
                Product selectedItem = SelectProduct("Select the item to end promotions for:");

                Promotion toRemove = _promotionList.SingleOrDefault(p => p.Name == selectedItem.Name);

                if (toRemove != null)
                {
                    _promotionList.Remove(toRemove);
                    Console.WriteLine("Promotion for {0} has been ended.", selectedItem.Name);
                    _repository.PromotionRepository.UpSert(_promotionList);
                }
                else
                {
                    Console.WriteLine("No promotions for {0} to end.", selectedItem.Name);
                }
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                throw;
            }
        }
        /// <summary>
        /// Method to select product and prompt message
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        private Product SelectProduct(string prompt = null)
        {

            if (!_productList.Any())
                throw new ArgumentException("options cannot be emtpty");

            while (true)
            {
                Console.WriteLine(prompt ?? "Select a Product");
                int i = 1;
                foreach (Product product in _productList)
                {
                    Console.WriteLine("\t[{0}] {1}", i, product.Name);
                    i++;
                }
                Console.Write("Selection:");
                string input = Console.ReadLine();
                int selection;
                if (int.TryParse(input, out selection) && (selection <= _productList.Count))
                    return _productList[selection - 1];

                Console.WriteLine("Selection {0} is not valid.", input);
            }
        }
        /// <summary>
        /// Method to start promotion
        /// </summary>
        private void StartPromotion()
        {
            try
            {
                Product toPromote = SelectProduct("Select a Product");

                if (GroceryItemHasExistingPromotions(toPromote.Name))
                    return;

                Console.WriteLine("Select the type of promotion you want to create...");

                PromotionTypeEnum type = SelectFrom(Enum.GetValues(typeof(PromotionTypeEnum)).Cast<PromotionTypeEnum>());

                switch (type)
                {
                    case PromotionTypeEnum.OnSale:
                        AddOnSalePromotion(toPromote);
                        break;
                    case PromotionTypeEnum.Group:
                        AddGroupPromotion(toPromote);
                        break;
                    case PromotionTypeEnum.AdditionalProduct:
                        AddAdditionalProductPromotion(toPromote);
                        break;
                    default:
                        Console.WriteLine("Invalid selection.");
                        return;
                }

                Console.WriteLine("Promotion added for {0}.", toPromote.Name);
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message);
                _log.Error(exp.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Method to get discount values with message on console
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        private double GetDouble(string prompt = null)
        {
            Console.Write(prompt);

            while (true)
            {
                string input = Console.ReadLine();

                double result;

                if (double.TryParse(input, out result))
                    return result;

                Console.WriteLine("{0} is not valid.", input);
            }
        }
        /// <summary>
        /// Method to get valid date in dd/MM/yyyy format
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        private DateTime GetDate(string prompt)
        {
            Console.WriteLine(prompt);
            string line = Console.ReadLine();
            DateTime dt;
            while (!DateTime.TryParseExact(line, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                Console.WriteLine("Invalid date, please retry");
                line = Console.ReadLine();
            }
            return dt;
        }
        /// <summary>
        /// Method to add OnSale Promotion
        /// </summary>
        /// <param name="groceryItem"></param>
        private void AddOnSalePromotion(Product groceryItem)
        {
            double salePrice = GetSalePrice();
            DateTime startDate = GetDate("Promotion Start Date (dd/MM/yyy)");
            DateTime endDate = GetDate("Promotion End Date (dd/MM/yyy)");
            Promotion promotion = _promotionBp.CreatePromotion(groceryItem, PromotionTypeEnum.OnSale, salePrice, 0, startDate, endDate, CurrentUser.UserId);
            _promotionList.Add(promotion);
            _repository.PromotionRepository.UpSert(_promotionList);
        }
        /// <summary>
        /// Method to add Group Promotion
        /// </summary>
        /// <param name="groceryItem"></param>

        private void AddGroupPromotion(Product groceryItem)
        {
            int requiredItems = GetRequiredItems();
            double salePrice = GetSalePrice();
            DateTime startDate = GetDate("Promotion Start Date (dd/MM/yyy)");
            DateTime endDate = GetDate("Promotion End Date (dd/MM/yyy)");
            Promotion promotion = _promotionBp.CreatePromotion(groceryItem, PromotionTypeEnum.Group, salePrice, requiredItems, startDate, endDate, CurrentUser.UserId);
            _promotionList.Add(promotion);
            _repository.PromotionRepository.UpSert(_promotionList);
        }
        /// <summary>
        /// Method to add Additional Product promotion
        /// </summary>
        /// <param name="groceryItem"></param>
        private void AddAdditionalProductPromotion(Product groceryItem)
        {
            int requiredItems = GetRequiredItems();
            double discount = GetDouble("Enter the discount (%) for this promotion:");
            DateTime startDate = GetDate("Promotion Start Date (dd/MM/yyy)");
            DateTime endDate = GetDate("Promotion End Date (dd/MM/yyy)");
            if (discount > 1)
                discount = discount / 100;
            Promotion promotion = _promotionBp.CreatePromotion(groceryItem, PromotionTypeEnum.AdditionalProduct, discount, requiredItems, startDate, endDate, CurrentUser.UserId);
            _promotionList.Add(promotion);
            _repository.PromotionRepository.UpSert(_promotionList);
        }
        /// <summary>
        /// Method to get No. of required items
        /// </summary>
        /// <returns></returns>

        private int GetRequiredItems()
        {
            return GetInt("Enter the number of items required for eligibility:");
        }
        /// <summary>
        /// Method to get sale price
        /// </summary>
        /// <returns></returns>
        private Double GetSalePrice()
        {
            return GetDouble("Enter the sale price (0.00):");
        }
        /// <summary>
        /// Method to check whether product has existing promotions or not
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool GroceryItemHasExistingPromotions(string name)
        {
            if (_repository.PromotionRepository.GetAll().All(p => p.Name != name))
                return false;

            Console.WriteLine("There is already a promotion running for this item.");
            return true;
        }

        /// <summary>
        /// Method to validate integer value on console
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        private int GetInt(string prompt)
        {
            Console.Write(prompt);

            while (true)
            {
                string input = Console.ReadLine();

                int result;

                if (int.TryParse(input, out result))
                    return result;

                Console.WriteLine("{0} is not valid.", input);
            }
        }

        /// <summary>
        /// Method to select list of options 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="prompt"></param>
        /// <returns></returns>
        private T SelectFrom<T>(IEnumerable<T> options, string prompt = null)
        {
            T[] items = options.ToArray();

            if (!items.Any())
                throw new ArgumentException("options cannot be emtpty");

            while (true)
            {
                Console.WriteLine(prompt ?? string.Format("Select a {0}...", typeof(T).Name));

                for (int i = 1; i <= items.Length; i++)
                {
                    Console.WriteLine("\t[{0}] {1}", i, items[i - 1]);
                }

                Console.Write("Selection:");

                string input = Console.ReadLine();

                int selection;

                if (int.TryParse(input, out selection) && (selection <= items.Length))
                    return items[selection - 1];

                Console.WriteLine("Selection {0} is not valid.", input);
            }
        }
    }
}
