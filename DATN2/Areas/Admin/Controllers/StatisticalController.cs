using DATN2.Models;
using DATN2.Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DATN2.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticalController()
        {
            _context = new ApplicationDbContext(); // Khởi tạo DbContext
        }
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
           
            return View();
        }
        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in db.Wishlist
                        on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
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
                .Take(5) // Chọn top 5 sản phẩm bán chạy nhất
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

    }
}