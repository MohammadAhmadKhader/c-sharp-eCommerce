using c_shap_eCommerce.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_eCommerce.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>,Guid>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetails>().HasKey(model => new { model.Id, model.OrderId, model.ProductId });
            // composite key of Id & OrderId & ProductId
           
            modelBuilder.Entity<User>()
                .Ignore(x => x.TwoFactorEnabled);
                
			base.OnModelCreating(modelBuilder);
        }
    }
}
