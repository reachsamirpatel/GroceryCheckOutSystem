//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Serialization;
//using Interfaces;

//namespace GroceryCheckOut.Entity
//{
//    [Serializable, XmlRoot("PromotionTypes")]
//    public class PromotionType //: IPromotionType
//    {
//        [DataMember]
//        public string ShortName { get; set; }
//        [DataMember]
//        public Guid PromotionId { get; set; }
//        [DataMember]
//        public string PromotionName { get; set; }

//        public PromotionType()
//        {

//        }
//        public PromotionType(Guid id, string name, string shortName)
//        {
//            PromotionId = id;
//            PromotionName = name;
//            ShortName = shortName;
//        }
//        public PromotionType(string name, string shortName)
//        {
//            PromotionId = Guid.NewGuid();
//            PromotionName = name;
//            ShortName = shortName;
//        }
//    }
//}
