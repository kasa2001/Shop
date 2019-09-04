using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ProductList
    {
        public ICollection<ProductDetails> Products { get; set; }
    }

    public class ProductDetails
    {
        public int Id { get; set; }

        [DisplayName("Nazwa produktu")]
        public string Name { get; set; }

        [DisplayName("Ilość")]
        public int Count { get; set; }

        [DisplayName("Cena")]
        public double Price { get; set; }

        [DisplayName("Nazwa kategorii")]
        public string CategoryName { get; set; }


        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }

    public class CreateProduct
    {

        [Required(ErrorMessage = "Należy podać nazwę produktu")]
        [DisplayName("Nazwa produktu")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Należy podać ilość produktów w magazynie")]
        [DisplayName("Ilość produktów")]
        [DefaultValue(0)]
        public int Count { get; set; }

        [Required(ErrorMessage = "Należy podać cenę jednostową za produkt")]
        [DisplayName("Cena za jednostkę")]
        public double Cost { get; set; }

        [Required(ErrorMessage = "Należy wybrać kategorię produktu")]
        [DisplayName("Kategoria produktu")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }

    public class CategoryDetails
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Należy podać nazwę kategorii")]
        public String Name { get; set; }
    }

    public class CategoryList
    {
        public ICollection<CategoryDetails> Categories { get; set; }
    }

    public class OrderList
    {
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }

    public class OrderDetails
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public Status Status { get; set; }
    }

    public class AboutView
    {
        [DisplayName("Product Count")]
        public int ProductCount { get; set; }

        [DisplayName("Category Count")]
        public int CategoryCount { get; set; }

        [DisplayName("Order Count")]
        public int OrdersCount { get; set; }

        [DisplayName("Products in order Count")]
        public int ProductsInOrderCount { get; set; }
    }
    
}