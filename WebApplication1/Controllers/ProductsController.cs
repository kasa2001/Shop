﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    public class ProductsController : Controller
    {
        private ShopContext db = new ShopContext();

        private ProductService productService = new ProductService();

        // GET: Products/Category/5
        public ActionResult Category(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = this.db.Categories.Single(item => item.Id == id);

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(
                this.productService.ProductList(
                    this.db.Products.Where(product => product.Category.Id == category.Id).ToList()
                ).Products
            );
        }

        // GET: Products
        public ActionResult Index(string sort, string currentFilter, string search, string category, int? page)
        {
            ViewBag.CurrentSort = sort;

            
            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;


            var products = db.Products;
            ProductList list = this.productService.ProductList(
                db.Products.ToList()
            );

            ViewBag.Categories = this.db.Categories.ToList();

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            if (category == null)
            {
                return View(list.Find(search).Sort(sort).ToPagedList(pageNumber, pageSize));

            }

            return View(list.Find(search).Categories(category).Sort(sort).ToPagedList(pageNumber, pageSize));

        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductDetails product = this.productService.ProductDetails(
                db.Products.Find(id)
            );

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Name,Count,Cost,CategoryId")] CreateProduct product)
        {
            if (ModelState.IsValid)
            {
                Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);

                Product products = this.productService.CreateProduct(
                        profile,
                        product
                    );

                HttpPostedFileBase file = Request.Files["file"];

                if (file != null && file.ContentLength > 0)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + file.FileName);
                    products.FileName = file.FileName;
                }


                db.Products.Add(
                    products
                );

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ProductDetails productDetails = this.productService.ProductDetails(
                product
            );

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(productDetails);
        }

        // POST: Products/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "Name,Count,Cost,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["file"];

                if (file != null && file.ContentLength > 0)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Obrazki/") + file.FileName)  ;
                    product.FileName = file.FileName;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ProductDetails productDetails = this.productService.ProductDetails(
                product
            );

            return View(productDetails);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
