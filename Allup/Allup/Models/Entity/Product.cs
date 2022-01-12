using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Models.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Rating { get; set; }
        public decimal Price { get; set; }
        public bool IsDiscounted { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public int Tax { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
    }

}
