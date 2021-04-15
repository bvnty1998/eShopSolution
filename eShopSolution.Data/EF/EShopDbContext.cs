using eShopSolution.Data.Cofiguarations;
using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.EF
{
   public class EShopDbContext : DbContext
    {
        public EShopDbContext( DbContextOptions options) : base(options)
        {
        }

        // cofig các table bằng fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // config table Product
            modelBuilder.ApplyConfiguration(new ProductConfiguaration());
            // config table Category
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            // config table AppCogfig
            modelBuilder.ApplyConfiguration(new AppConfigConfiguration());
            // congfig table Cart 
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            // config table CategoryTranslations
            modelBuilder.ApplyConfiguration(new CategoryTranslationsConfiguration());
            // config table Contact
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            // Configuaration table Languge
            modelBuilder.ApplyConfiguration(new LangugeConfiguration());
            //config table Order
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            // config table OrderDetail 
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            // config table ProductTranslations
            modelBuilder.ApplyConfiguration(new ProductTranslationConfiguration());
            // config table Promotion
            modelBuilder.ApplyConfiguration(new PromotionConfiguration());
            // config table Transaction
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        }

        public DbSet<Product> Products { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        //public DbSet<ProductInCategory> ProductInCategories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
