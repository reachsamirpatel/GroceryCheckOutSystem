using System.Collections.Generic;
using System.Configuration;
using GroceryCheckOut.Entity;

namespace GroceryCheckOutSystem.DataAccess
{
    public class PromotionRepository : RepositoryDAC
    {
        /// <summary>
        /// Class read and write to Promotions.xml file to perform promotion operations
        /// </summary>
        public PromotionRepository()
        {
            base.FilePath = ConfigurationManager.AppSettings["PromotionFileName"];
        }
        /// <summary>
        /// Method to get list from xml
        /// </summary>
        public List<Promotion> GetAll()
        {
            return base.GetAll<Promotion>();
        }
        /// <summary>
        /// Method to insert value to xml
        /// </summary>
        /// <param name="loginList"></param>
        public void UpSert(List<Promotion> promotionList)
        {
            base.UpSert(promotionList);

        }
    }
}
