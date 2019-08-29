using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductsInOrdersController : Controller
    {
        private ShopContext db = new ShopContext();

        // GET: ProductsInOrders
        public ActionResult Index()
        {
            var productsInOrders = db.ProductsInOrders.Include(p => p.Adder).Include(p => p.Modifier).Include(p => p.Order).Include(p => p.Product);
            return View(productsInOrders.ToList());
        }

        // GET: ProductsInOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsInOrder productsInOrder = db.ProductsInOrders.Find(id);
            if (productsInOrder == null)
            {
                return HttpNotFound();
            }
            return View(productsInOrder);
        }

        // GET: ProductsInOrders/Create
        public ActionResult Create()
        {
            ViewBag.AdderId = new SelectList(db.Profiles, "Id", "UserName");
            ViewBag.ModifierId = new SelectList(db.Profiles, "Id", "UserName");
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "Id");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: ProductsInOrders/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Count,Cost,Active,ProductId,OrderId,Added,Updated,AdderId,ModifierId")] ProductsInOrder productsInOrder)
        {
            if (ModelState.IsValid)
            {
                db.ProductsInOrders.Add(productsInOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdderId = new SelectList(db.Profiles, "Id", "UserName", productsInOrder.AdderId);
            ViewBag.ModifierId = new SelectList(db.Profiles, "Id", "UserName", productsInOrder.ModifierId);
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "Id", productsInOrder.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productsInOrder.ProductId);
            return View(productsInOrder);
        }

        // GET: ProductsInOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsInOrder productsInOrder = db.ProductsInOrders.Find(id);
            if (productsInOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdderId = new SelectList(db.Profiles, "Id", "UserName", productsInOrder.AdderId);
            ViewBag.ModifierId = new SelectList(db.Profiles, "Id", "UserName", productsInOrder.ModifierId);
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "Id", productsInOrder.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productsInOrder.ProductId);
            return View(productsInOrder);
        }

        // POST: ProductsInOrders/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Count,Cost,Active,ProductId,OrderId,Added,Updated,AdderId,ModifierId")] ProductsInOrder productsInOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productsInOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdderId = new SelectList(db.Profiles, "Id", "UserName", productsInOrder.AdderId);
            ViewBag.ModifierId = new SelectList(db.Profiles, "Id", "UserName", productsInOrder.ModifierId);
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "Id", productsInOrder.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productsInOrder.ProductId);
            return View(productsInOrder);
        }

        // GET: ProductsInOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsInOrder productsInOrder = db.ProductsInOrders.Find(id);
            if (productsInOrder == null)
            {
                return HttpNotFound();
            }
            return View(productsInOrder);
        }

        // POST: ProductsInOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductsInOrder productsInOrder = db.ProductsInOrders.Find(id);
            db.ProductsInOrders.Remove(productsInOrder);
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
