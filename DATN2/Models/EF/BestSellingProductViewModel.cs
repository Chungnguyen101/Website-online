using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATN2.Models.EF
{
    public class BestSellingProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}