using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public static class ShopQueriesService
    {
        public static ICollection<ProductDetails> Sort(this ICollection<ProductDetails> list, string sort)
        {
            switch (sort)
            {
                case "name":
                    return list.OrderBy(obj => obj.Name).ToList();
                case "count":
                    return list.OrderBy(obj => obj.Count).ToList();
                default:
                    return list;
            }
        }

        public static ICollection<ProductDetails> Find(this ProductList productList, string search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                return productList.Products.Where(p => p.Name.Contains(search)).Select(m => m).ToList();
            }

            return productList.Products;
        }

        public static ICollection<ProductDetails> Pagination()
        {
            return new List<ProductDetails>();
        }
    }
}