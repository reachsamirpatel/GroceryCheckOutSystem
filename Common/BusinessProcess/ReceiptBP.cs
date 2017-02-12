using System;
using System.Collections.Generic;
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

        public string CreateReceipt(List<ProductPurchase> purchasedProductList)
        {
            List<ProductPurchase> temp = purchasedProductList;
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

            purchasedProductList = query.ToList();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0,10}{1,10}{2,20}{3,20}{4,20}", "Product", "Quantity", "Unit Price", "Total Sale Price", Environment.NewLine);
            foreach (ProductPurchase item in purchasedProductList)
            {
                sb.AppendFormat("{0,10}{1,10}{2,20}{3,20}{4,20}", item.Product.Name, item.Quantity, item.Product.Price, item.DiscountedPrice, Environment.NewLine);
            }
            sb.AppendLine();
            sb.AppendFormat("Total Cost : {0}", temp.Sum(x => x.DiscountedPrice));
            _log.Debug(sb.ToString());
            return sb.ToString();
        }
    }
}
