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
        public string Name { get; set; }

        [DataMember]
        public PromotionTypeEnum PromotionType { get; set; }

        [DataMember]
        public double Discount { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [DataMember]
        public Guid? CreatedBy { get; set; }

        [DataMember]
        public int NumberOfItemsRequired { get; set; }

        public Promotion()
        {
        }
        public Promotion(string name, PromotionTypeEnum promotionType, double discount, int numberOfItemsRequired, DateTime? startDate, DateTime? endDate, Guid? createdBy)
        {
            PromotionId = Guid.NewGuid();
            Name = name;
            PromotionType = promotionType;
            Discount = discount;
            NumberOfItemsRequired = numberOfItemsRequired;
            StartDate = startDate;
            EndDate = endDate;
            CreatedBy = createdBy;
        }
        public Promotion(string name, PromotionTypeEnum promotionType, double discount, int numberOfItemsRequired)
        {
            PromotionId = Guid.NewGuid();
            Name = name;
            PromotionType = promotionType;
            Discount = discount;
            NumberOfItemsRequired = numberOfItemsRequired;
        }

        public Promotion(Guid promotionId, string name, PromotionTypeEnum promotionType, double discount, int numberOfItemsRequired)
        {
            PromotionId = promotionId;
            Name = name;
            PromotionType = promotionType;
            Discount = discount;
            NumberOfItemsRequired = numberOfItemsRequired;
        }
    }
}
