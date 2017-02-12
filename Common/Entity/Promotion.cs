using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using GroceryCheckOut.Entity.Enums;


namespace GroceryCheckOut.Entity
{
    [DataContract]
    [Serializable, XmlRoot("Promotions")]
    public class Promotion
    {
        [DataMember]
        public Guid PromotionId { get; set; }

        [DataMember]
        public Guid ProductId { get; set; }

        [DataMember]
        // public PromotionType PromotionType { get; set; }
        public PromotionTypeEnum PromotionType { get; set; }

        [DataMember]
        public double Discount { get; set; }

        [DataMember]
        public int NumberOfItemsRequired { get; set; }

        public Promotion()
        {
        }

        public Promotion(Guid productId, PromotionTypeEnum promotionType, double discount, int numberOfItemsRequired)
        {
            PromotionId = Guid.NewGuid();
            ProductId = productId;
            PromotionType = promotionType;
            Discount = discount;
            NumberOfItemsRequired = numberOfItemsRequired;
        }

        public Promotion(Guid promotionId, Guid productId, PromotionTypeEnum promotionType, double discount, int numberOfItemsRequired)
        {
            PromotionId = promotionId;
            ProductId = productId;
            PromotionType = promotionType;
            Discount = discount;
            NumberOfItemsRequired = numberOfItemsRequired;
        }
    }
}
