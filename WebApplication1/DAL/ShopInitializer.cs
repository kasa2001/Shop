using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class ShopInitializer : DropCreateDatabaseIfModelChanges<ShopContext>
    {

        protected override void Seed(ShopContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext())
            );

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext())
            );

            roleManager.Create(new IdentityRole("Administrator"));
            roleManager.Create(new IdentityRole("User"));

            base.Seed(context);
        }
    }
}