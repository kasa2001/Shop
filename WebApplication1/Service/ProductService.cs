using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class ProductService
    {
        /// <summary>
        /// Utworzenie nowego produktu
        /// </summary>
        /// <param name="applicationUser">Kto utworzył</param>
        /// <param name="name">Nazwa produktu</param>
        /// <param name="count">Ilość</param>
        /// <param name="price">Cena za jednostkę</param>
        /// <param name="category">Kategoria</param>
        /// <returns></returns>
        public Product CreateProduct(Profile applicationUser, CreateProduct product)
        {
            return new Product
            {
                Added = DateTime.Now,
                AdderId = applicationUser.Id,
                Updated = DateTime.Now,
                ModifierId = applicationUser.Id,
                Name = product.Name,
                Count = product.Count,
                Cost = product.Cost,
                CategoryId = product.CategoryId
            };
        }

        /// <summary>
        /// Zwiększanie ilości produktów w magazynie
        /// </summary>
        /// <param name="product">Produkt</param>
        /// <param name="added">Ilość dostarczona do magazynu</param>
        /// <returns>Produkt ze zwiększonym stanem na magazynie</returns>
        public Product AddProduct(Product product, int added)
        {
            product.Count += added;

            return product;
        }

        /// <summary>
        /// Zmniejszanie ilości produktów w magazynie
        /// </summary>
        /// <param name="product">Produkt</param>
        /// <param name="removed">Ilość odjęta z magazynu</param>
        /// <returns>Produkt ze zmniejszonym stanem na magazynie</returns>
        public Product RemoveProduct(Product product, int removed)
        {
            product.Count -= removed;

            if (product.Count < 0)
            {
                throw new Exception();
            }

            return product;
        }

        /// <summary>
        /// Aktualizacja produktu
        /// </summary>
        /// <param name="product"></param>
        /// <param name="user">Kto dokonał aktualizacji</param>
        /// <param name="name">Nazwa produktu</param>
        /// <param name="price">Cena produktu</param>
        /// <param name="category">Kategoria</param>
        /// <returns></returns>
        public Product UpdateProduct(Product product, Profile user, String name, double price, Category category)
        {
            product.ModifierId = user.Id;
            product.Updated = DateTime.Now;
            product.Name = name;
            product.Cost = price;
            product.CategoryId = category.Id;

            return product;
        }

        /// <summary>
        /// Przygotowuje listę produktów do wyświetlenia na froncie
        /// </summary>
        /// <param name="products"></param>
        /// <returns>Lista produktów</returns>
        public ProductList ProductList(ICollection<Product> products)
        {
            ProductList list = new ProductList();

            ICollection<ProductDetails> productDetails = new List<ProductDetails>();

            foreach (Product product in products)
            {
                productDetails.Add(
                    this.ProductDetails(product)
                );
            }

            list.Products = productDetails;

            return list;
        }

        /// <summary>
        /// Przygotowuje produkt do wyświetlenia na froncie
        /// </summary>
        /// <param name="product">Produkt</param>
        /// <returns>Szczegóły produktu</returns>
        public ProductDetails ProductDetails(Product product)
        {
            return new ProductDetails
            {
                Id = product.Id,
                CategoryName = product.Category.Name,
                CategoryId = product.Category.Id,
                Name = product.Name,
                Count = product.Count,
                Price = product.Cost,
                FileName = product.FileName
            };
        }
    }
}