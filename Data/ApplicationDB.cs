using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC_Web_App.Models;

namespace MVC_Web_App.Data
{
    public class ApplicationDB : DbContext
    {
        public ApplicationDB(DbContextOptions<ApplicationDB> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppRole>().Property(x => x.Id).HasDefaultValueSql("NewID()");
            modelBuilder.Entity<UserRole>().Property(s => s.Id).HasDefaultValueSql("NewID()");
            modelBuilder.Entity<AppUsers>().Property(s => s.Id).HasDefaultValueSql("NewID()");

        }
        public DbSet<AppUsers> AppUser { get; set; }
        public DbSet<AppConfrim> AppConfrims { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserProfile> userProfiles { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<BillingAddress> BillingAddresses { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
