using eShopSolution.Data.Cofiguarations;
using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.EF
{
   public class EShopDbContext : IdentityDbContext<AppUser,AppRole,Guid, IdentityUserClaim<Guid>
       ,AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public EShopDbContext( DbContextOptions options) : base(options)
        {
        }

        // cofig các table bằng fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // config table Product
            modelBuilder.ApplyConfiguration(new ProductConfiguaration());
            // config table ProductImage
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
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
            // config table Function
            modelBuilder.ApplyConfiguration(new FunctionConfiguration());
            // config table Permission
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            // config table AppUser
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            // config table AppRole
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            // config table AppUserRoles
            modelBuilder.ApplyConfiguration(new AppUserRoleCofiguration());

            /// Tạo các bảng liên quan đến Identity
            // Tạo bảng và config IdentityUserClaim
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims") ;
            // Tạo bảng và config IdentityUserLogin
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x=>x.UserId);
            // Tạo bảng và config IdentityRoleClaim
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims") ;
            // Tạo bảng và config IdentityUserToken
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x=>x.UserId);
        }

        public DbSet<Product> Products { set; get; }
        public DbSet<ProductImage> productImages { set; get; }
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
        public DbSet<Function> Functions { get; set; }
        public DbSet<Permission> Permissions { get; set; }

    }
}
