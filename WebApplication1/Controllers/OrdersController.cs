using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    public class OrdersController : Controller
    {
        private ShopContext db = new ShopContext();
        private OrderService service = new OrderService();

        // GET: Orders
        [Authorize]
        public ActionResult Index()
        {
            OrderList list = this.service.OrderList(this.db.Orders.ToList());

            if (!User.IsInRole("Administrator"))
            {
                Profile profile = db.Profiles.Single(p => p.UserName == User.Identity.Name);
                list.OrderDetails = list.OrderDetails.Where(item => item.Profile == profile).ToList();
            }

            return View(list.OrderDetails);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(
                this.service.OrderDetails(
                    order
                )    
            );
        }

        // GET: Orders/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Id")] OrderDetails order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(
                    this.service.CreateOrder(
                        db.Profiles.Single(p => p.UserName == User.Identity.Name)
                    )
                );
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(
                this.service.OrderDetails(
                    order
                )
            );
        }

        // POST: Orders/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                order = this.service.Delived(
                    order,
                    db.Profiles.Single(p => p.UserName == User.Identity.Name)
                );

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(
                this.service.OrderDetails(
                    order
                )    
            );
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(
                this.service.OrderDetails(
                    order
                )
            );
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [Authorize]
        public ActionResult AddProduct(int id)
        {
            Profile profile = db.Profiles.Single(a => a.UserName == User.Identity.Name);

            Order order = this.db.Orders.Order(
                profile,
                this.service
            );

            Product p = this.db.Products.Find(id);

            if (order.Id.Equals(0))
            {
                return RedirectToAction("Index");
            }

            ProductsInOrder productsInOrder = new ProductsInOrder()
            {
                OrderId = order.Id,
                ProductId = p.Id,
                Added = DateTime.Now,
                Updated = DateTime.Now,
                AdderId = profile.Id,
                ModifierId = profile.Id,
                Active = true,
                Cost = p.Cost,
                Count = 1
            };

            new ProductService().RemoveProduct(p, 1);

            this.db.ProductsInOrders.Add(productsInOrder);
            this.db.SaveChanges();

            return RedirectToAction("Index", "Products");
        }

        public ActionResult CreateOrder(int id)
        {
            Profile profile = db.Profiles.Single(a => a.UserName == User.Identity.Name);

            Order order = this.db.Orders.Order(
                profile,
                this.service
            );

            if (order.Id.Equals(0))
            {
                this.db.Orders.Add(order);
                this.db.SaveChanges();
            }
            
            return RedirectToAction("AddProduct", "Orders", new { id });
        }

        public ActionResult ToPay(int id)
        {
            Order order = this.service.ToPay(
                this.db.Orders.Find(id),
                db.Profiles.Single(p => p.UserName == User.Identity.Name)
            );

            return Save(order);
        }

        public ActionResult Payed(int id)
        {
            Order order = this.service.Payed(
                this.db.Orders.Find(id),
                db.Profiles.Single(p => p.UserName == User.Identity.Name)
            );

            return Save(order);
        }

        public ActionResult Deliving(int id)
        {
            Order order = this.service.Deliving(
                this.db.Orders.Find(id),
                db.Profiles.Single(p => p.UserName == User.Identity.Name)
            );

            return Save(order);
        }

        public ActionResult Delived(int id)
        {
            Order order = this.service.Delived(
                this.db.Orders.Find(id),
                db.Profiles.Single(p => p.UserName == User.Identity.Name)
            );

            return Save(order);
        }

        public ActionResult Returned(int id)
        {
            Order order = this.service.Return(
                this.db.Orders.Find(id),
                db.Profiles.Single(p => p.UserName == User.Identity.Name)
            );

            this.ReturnProducts(order);

            return Save(order);
        }

        private void ReturnProducts(Order order)
        {
            List<ProductsInOrder> list = this.db.ProductsInOrders.Where(p => p.OrderId == order.Id && p.Active == true).ToList();

            ProductService service = new ProductService();

            foreach (ProductsInOrder products in list)
            {
                service.AddProduct(products.Product, 1);
            }
        }

        public ActionResult Cancelled(int id)
        {
            Order order = this.service.Cancel(
                this.db.Orders.Find(id),
                db.Profiles.Single(p => p.UserName == User.Identity.Name)
            );

            this.ReturnProducts(order);

            return Save(order);
        }

        private ActionResult Save(Order order)
        {
            this.db.Entry(order).State = EntityState.Modified;
            this.db.SaveChanges();
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
