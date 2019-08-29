using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class ProductsInOrderService
    {
        /// <summary>
        /// Dodawanie produktu do zamówienia
        /// </summary>
        /// <param name="order">Zamówienie</param>
        /// <param name="product">Produkt</param>
        /// <param name="count">Ilość</param>
        /// <returns></returns>
        public ProductsInOrder AddProduct(Order order, Product product, int count)
        {
            return new ProductsInOrder
            {
                Product = product,
                Order = order,
                Count = count,
                Cost = product.Cost,
                Active = true
            };
        }

        public ProductsInOrder RemoveProduct(ProductsInOrder productsInOrder)
        {
            productsInOrder.Active = false;
            return productsInOrder;
        }
    }
}