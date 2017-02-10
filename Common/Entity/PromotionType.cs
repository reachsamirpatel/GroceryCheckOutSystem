using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace GroceryCheckOut.Entity
{

    public class PromotionType : IPromotionType
    {
        public Guid PromotionId { get; set; }
        public string PromotionName { get; set; }

        public PromotionType(Guid id, string name)
        {
            PromotionId = id;
            PromotionName = name;
        }
        public PromotionType(string name)
        {
            PromotionId = Guid.NewGuid();
            PromotionName = name;
        }
    }
}
