using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public abstract class Entity
    {
        public int Id { get; set; }
        
        [Column(TypeName = "datetime2")]
        public DateTime Added { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Updated { get; set; }
        
    }

    public class Category : Entity
    {
        public int AdderId { get; set; }

        public int? ModifierId { get; set; }

        public virtual Profile Modifier { get; set; }

        public virtual Profile Adder { get; set; }

        public String Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

    public class Order : Entity
    {
        public int AdderId { get; set; }

        public int? ModifierId { get; set; }

        public virtual Profile Modifier { get; set; }

        public virtual Profile Adder { get; set; }

        public Status Status { get; set; }

        public int ProfileId { get; set; }

        public virtual Profile Profile { get; set; }
    }

    public class Product : Entity
    {
        public int AdderId { get; set; }

        public int? ModifierId { get; set; }

        public virtual Profile Modifier { get; set; }

        public virtual Profile Adder { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public double Cost { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }

    public class ProductsInOrder : Entity
    {
        public int AdderId { get; set; }

        public int? ModifierId { get; set; }

        public virtual Profile Modifier { get; set; }

        public virtual Profile Adder { get; set; }

        public int Count { get; set; }

        public double Cost { get; set; }

        public bool Active { get; set; }

        public int ProductId { get; set; }

        public int OrderId { get; set; }

        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }
    }

    public class Profile
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Order> AddedOrders { get; set; }

        public virtual ICollection<Order> ModifiedOrders { get; set; }

        public virtual ICollection<Category> AddedCategories { get; set; }

        public virtual ICollection<Category> ModifiedCategories { get; set; }

        public virtual ICollection<Product> AddedProducts { get; set; }

        public virtual ICollection<Product> ModifiedProducts { get; set; }

        public virtual ICollection<ProductsInOrder> AddedProductsInOrders { get; set; }

        public virtual ICollection<ProductsInOrder> ModifiedProductsInOrders { get; set; }
    }


    public enum Status
    {
        New,
        ToPay,
        Payed,
        Deliving,
        Delived,
        Returned,
        Cancelled
    }

    public class ShopContext : DbContext
    {
        public ShopContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ProductsInOrder> ProductsInOrders { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Product>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Category>()
                .HasKey(i => i.Id );

            modelBuilder.Entity<Order>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<ProductsInOrder>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Profile>()
                .HasKey(i => i.Id);

            // Orders
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.Orders)
                .WithRequired(i => i.Adder)
                .HasForeignKey(i => i.AdderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.AddedOrders)
                .WithRequired(i => i.Modifier)
                .HasForeignKey(i => i.ModifierId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.ModifiedOrders)
                .WithRequired(i => i.Profile)
                .HasForeignKey(i => i.ProfileId)
                .WillCascadeOnDelete(false);

            // Products
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.AddedProducts)
                .WithRequired(i => i.Adder)
                .HasForeignKey(i => i.AdderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.ModifiedProducts)
                .WithRequired(i => i.Modifier)
                .HasForeignKey(i => i.ModifierId)
                .WillCascadeOnDelete(false);

            // Categories
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.AddedCategories)
                .WithRequired(i => i.Adder)
                .HasForeignKey(i => i.AdderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.ModifiedCategories)
                .WithRequired(i => i.Modifier)
                .HasForeignKey(i => i.ModifierId)
                .WillCascadeOnDelete(false);

            // Products in Order
            modelBuilder.Entity<Profile>()
                .HasMany(p => p.AddedProductsInOrders)
                .WithRequired(i => i.Adder)
                .HasForeignKey(i => i.AdderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Profile>()
                .HasMany(p => p.ModifiedProductsInOrders)
                .WithRequired(i => i.Modifier)
                .HasForeignKey(i => i.ModifierId)
                .WillCascadeOnDelete(false);
            
            base.OnModelCreating(modelBuilder);
        }
        
    }
}