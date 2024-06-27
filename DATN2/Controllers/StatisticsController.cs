using DATN2.Models.EF;
using DATN2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DATN2.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController()
        {
            _context = new ApplicationDbContext(); // Khởi tạo DbContext
        }

        // Action để thống kê sản phẩm bán chạy nhất
        public ActionResult BestSellingProducts()
        {
            var bestSellingProducts = _context.OrderDetails
                .GroupBy(od => new { od.ProductId, od.Product.Title })
                .Select(g => new BestSellingProductViewModel
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Title,
                    TotalQuantitySold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(g => g.TotalQuantitySold)
                .Take(2) // Chọn top 2 sản phẩm bán chạy nhất
                .ToList();

            return PartialView(bestSellingProducts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        // Action cho Index
        public ActionResult Index()
        {
            // Your logic for Index view if needed
            return View();
        }
    }
}