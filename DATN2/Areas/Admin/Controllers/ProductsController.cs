using DATN2.Models;
using DATN2.Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace DATN2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Products
        public ActionResult Index(int? page)
        {
            IEnumerable<Product> items = db.Wishlist.OrderByDescending(x=>x.Id );
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Add() 
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
              return View ();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
        {
            if ( ModelState.IsValid)
            {
                if(Images!=null && Images.Count >0 )
                {
                   for(int i=0;i< Images.Count; i++)
                    {
                        if(i+1 == rDefault[0])
                        {
                            model.Image = Images[i];
                            model.ProductImage.Add(new ProductImage { 
                            ProductId=model.Id,
                            Image=Images[i],
                            IsDefault= true,
                            });
                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductId = model.Id,
                                Image = Images[i],
                                IsDefault = false,
                            });
                        }
                    }   
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                if(string.IsNullOrEmpty(model.SeoTitle ))
                {
                    model.SeoTitle = model.Title;
                }
                if(string.IsNullOrEmpty(model.Alias))
                model.Alias = DATN2.Models.Common.Filter.FilterChar(model.Title);
                db.Wishlist.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            var item = db.Wishlist.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedDate = DateTime.Now;
                model.Alias = DATN2.Models.Common.Filter.FilterChar(model.Title);
                db.Wishlist.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Wishlist.Find(id);
            if (item != null)
            {
                db.Wishlist.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Wishlist.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isAcive = item.IsActive });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = db.Wishlist.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsHome = item.IsHome });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var item = db.Wishlist.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsSale = item.IsSale });
            }

            return Json(new { success = false });
        }
    }
}