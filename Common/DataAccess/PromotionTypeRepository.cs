//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using GroceryCheckOut.Entity;

//namespace GroceryCheckOutSystem.DataAccess
//{
//    public class PromotionTypeRepository : RepositoryDAC
//    {
//        public PromotionTypeRepository()
//        {
//            base.FilePath = ConfigurationManager.AppSettings["PromotionTypeFileName"];
//        }

//        public List<PromotionType> GetAll()
//        {
//            return base.GetAll<PromotionType>();
//        }
//        public void UpSert(List<PromotionType> promotionTypeList)
//        {
//            base.UpSert(promotionTypeList);
//        }
//    }
//}
