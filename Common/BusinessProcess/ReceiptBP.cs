using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryCheckOut.Entity;
using Logging;

namespace BusinessProcess
{
    public class ReceiptBP
    {
        private ILogger _log;
        public ReceiptBP()
        {
            Initialize();
        }

        private void Initialize()
        {
            _log = LogManager.GetLogger(this);
        }

        public string CreateReceipt(List<ProductPurchase> purchasedProductList, User user)
        {
            string delimiter = "**************************************************************************" +
                              Environment.NewLine;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0,40}", ConfigurationManager.AppSettings["StoreName"]);
            sb.AppendLine();
            sb.AppendFormat("{0,50}", ConfigurationManager.AppSettings["StoreAddress"]);
            sb.AppendLine();
            sb.AppendLine();
            Random generator = new Random();
            String transactionId = generator.Next(0, 1000000).ToString("D6");
            sb.AppendFormat("Transaction Id : {0}", transactionId);
            sb.AppendLine();
            sb.AppendFormat("EmployeeId: {0} {1,30} {2} {3}", generator.Next(0, 1000000).ToString("D6"), "Name:", user.FirstName, user.LastName);
            sb.AppendLine();
            sb.AppendFormat("Order Taken: {0}", DateTime.Now);
            sb.AppendLine();
            sb.Append(delimiter);
            sb.AppendFormat("{0,10}{1,10}{2,15}{3,15}{4,15}", "Product", "Quantity", "Unit Price", "Sale Price", "Savings");
            sb.AppendLine();
            foreach (ProductPurchase item in purchasedProductList)
            {
                sb.AppendFormat("{0,10}{1,10}{2,15}{3,15}{4,15}", item.Product.Name, item.Quantity, item.Product.Price.ToString("C"),
                    item.DiscountedPrice.ToString("C"), (item.Product.Price - item.DiscountedPrice).ToString("C"));
                sb.AppendLine();
            }
            sb.Append(PrintSummary(purchasedProductList));
            _log.Debug(sb.ToString());
            return sb.ToString();
        }

        private static StringBuilder PrintSummary(List<ProductPurchase> purchasedProductList)
        {
            IEnumerable<ProductPurchase> query =
               from ci in purchasedProductList
               group ci by new { ci.Product, ci.DiscountedPrice }
               into gcis
               select new ProductPurchase
               {
                   Product = gcis.Key.Product,
                   DiscountedPrice = gcis.Sum(x => x.DiscountedPrice),
                   Quantity = gcis.Sum(x => x.Quantity)
               };
            List<ProductPurchase> tempPurchaseList = query.ToList();
            StringBuilder sb = new StringBuilder();
            string delimiter = "**************************************************************************" +
                               Environment.NewLine;
            sb.Append(delimiter);
            sb.Append("                             SUMMARY                             ");
            sb.AppendLine();
            sb.Append(delimiter);
            sb.AppendFormat("{0,10}{1,10}{2,20}{3,20}{4,20}", "Product", "Quantity", "Unit Price", "Total Sale Price",
                Environment.NewLine);
            foreach (ProductPurchase item in tempPurchaseList)
            {
                sb.AppendFormat("{0,10}{1,10}{2,20}{3,20}{4,20}", item.Product.Name, item.Quantity, item.Product.Price.ToString("C"),
                    item.DiscountedPrice.ToString("C"), Environment.NewLine);
            }
            sb.Append(delimiter);
            sb.AppendLine();
            double totalRawPrice = purchasedProductList.Sum(x => x.Product.Price);
            double totalCost = purchasedProductList.Sum(x => x.DiscountedPrice);
            double totalSavings = totalCost - totalRawPrice;
            double tax = totalCost * (13) / 100;
            double total = totalCost + tax;
            sb.AppendFormat("Total Price(without savings) : {0,5}", totalRawPrice.ToString("C"));
            sb.AppendLine();
            sb.AppendFormat("Total Savings : {0,21}", totalSavings.ToString("C"));
            sb.AppendLine();
            sb.Append(delimiter);
            sb.AppendFormat("SubTotal : {0,26}", totalCost.ToString("C"));
            sb.AppendLine();
            sb.AppendFormat("Tax (13%) : {0,24}", tax.ToString("C"));
            sb.AppendLine();
            sb.Append(delimiter);
            sb.AppendFormat("Total : {0,29}", total.ToString("C"));
            return sb;
        }
    }
}
