using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class HomeService
    {
        public AboutView AboutSite(ShopContext db)
        {
            return new AboutView()
            {
                CategoryCount = db.Categories.Count(),
                ProductCount = db.Products.Count(),
                OrdersCount = db.Orders.Count(),
                ProductsInOrderCount = db.ProductsInOrders.Count()
            };
        }
    }
}