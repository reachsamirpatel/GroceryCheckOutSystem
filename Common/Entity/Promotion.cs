using System;
using System.Collections;
using System.Runtime.Serialization;
using Interfaces;

namespace GroceryCheckOut.Entity
{
    [DataContract]
    [Serializable]
    public class Promotion
    {
        [DataMember]
        public Guid PromotionId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public IPromotionType PromotionType { get; set; }

        [DataMember]
        public double Discount { get; set; }

        [DataMember]
        public int NumberOfItemsRequired { get; set; }

        public Promotion(string name, IPromotionType promotionType, double discount = 0, int numberOfItemsRequired = 0)
        {
            PromotionId = Guid.NewGuid();
            ProductName = name;
            PromotionType = promotionType;
            Discount = discount;
            NumberOfItemsRequired = numberOfItemsRequired;
        }

        public Promotion(Guid promotionId, string name, IPromotionType promotionType, double discount = 0, int numberOfItemsRequired = 0)
        {
            PromotionId = promotionId;
            ProductName = name;
            PromotionType = promotionType;
            Discount = discount;
            NumberOfItemsRequired = numberOfItemsRequired;
        }
    }
}
