using System.Collections.Generic;
using System.Configuration;
using GroceryCheckOut.Entity;

namespace GroceryCheckOutSystem.DataAccess
{
    public class PromotionRepository : RepositoryDAC
    {
        public PromotionRepository()
        {
            base.FilePath = ConfigurationManager.AppSettings["PromotionFileName"];
        }

        public List<Promotion> GetAll()
        {
            return base.GetAll<Promotion>();
        }

        public void UpSert(List<Promotion> promotionList)
        {
            base.UpSert(promotionList);

        }
    }
}
