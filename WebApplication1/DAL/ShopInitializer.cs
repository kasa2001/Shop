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

            var user = new ApplicationUser()
            {
                UserName = "pawelgomolka@interia.pl"
            };

            userManager.Create(
                user,
                "zaq1@WSX"
            );

            userManager.AddToRole(
                user.Id,
                "Administrator"
            );

            context.Profiles.Add(
                new Profile
                {
                    UserName = user.UserName
                }
            );

            context.SaveChanges();

            base.Seed(context);
        }
    }
}