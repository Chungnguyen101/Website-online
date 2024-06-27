using DATN2.Models;
using DATN2.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DATN2.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products

        //private readonly ApplicationDbContext _context;

        //public ProductsController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public ActionResult Index()
        {
            var items = db.Wishlist.ToList();

            return View(items);
        }

        public ActionResult Detail(string alias, int id)
        {
            var item = db.Wishlist.Find(id);
            if (item != null)
            {
                db.Wishlist.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            var countReview = db.Reviews.Where(x => x.ProductId == id).Count();
            ViewBag.CountReview = countReview;
            return View(item);
        }
        public ActionResult ProductCategory(string alias, int id)
        {
            var items = db.Wishlist.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }

            ViewBag.CateId = id;
            return View(items);
        }

        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Wishlist.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductSales()
        {
            var items = db.Wishlist.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        //public ActionResult BestSellingProducts()
        //{
        //    var bestSellingProducts = _context.OrderDetails
        //        .GroupBy(od => new { od.ProductId, od.Product.Title })
        //        .Select(g => new BestSellingProductViewModel
        //        {
        //            ProductId = g.Key.ProductId,
        //            ProductName = g.Key.Title,
        //            TotalQuantitySold = g.Sum(od => od.Quantity)
        //        })
        //        .OrderByDescending(g => g.TotalQuantitySold)
        //        .Take(10) // Chọn top 10 sản phẩm bán chạy nhất
        //        .ToList();

        //    return View(bestSellingProducts);
        //}

    }
}