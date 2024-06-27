using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATN2.Models.EF
{
    public class TopSellingProduct
    {
        public int ProductId { get; set; }
        public int SoldQuantity { get; set; }

        public virtual Product Product { get; set; }
    }
}