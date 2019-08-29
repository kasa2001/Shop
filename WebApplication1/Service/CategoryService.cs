using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class CategoryService
    {

        public CategoryList CategoryList(ICollection<Category> categories)
        {
            CategoryList list = new CategoryList
            {
                Categories = new List<CategoryDetails>()
            };

            foreach (Category category in categories)
            {
                list.Categories.Add(
                    this.CategoryDetails(category)  
                );
            }

            return list;
        }

        public CategoryDetails CategoryDetails(Category category)
        {
            return new CategoryDetails()
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public Category CreateCategory(Profile profile, CategoryDetails category)
        {
            return new Category()
            {
                Name = category.Name,
                Added = DateTime.Now,
                Updated = DateTime.Now,
                AdderId = profile.Id,
                ModifierId = profile.Id
            };
        }

        public Category EditCategory(Profile profile, Category category, CategoryDetails categoryDetails)
        {
            category.Name = categoryDetails.Name;
            category.Updated = DateTime.Now;
            category.ModifierId = profile.Id;

            return category;
        }
    }
}