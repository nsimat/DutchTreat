using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchTreatDbContext : IdentityDbContext<StoreUser>
    {
        public DutchTreatDbContext(DbContextOptions<DutchTreatDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("money");

            modelBuilder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasColumnType("money");

            modelBuilder.Entity<Order>().HasData(new Order()
            {
                OrderId = 1,
                OrderDate = DateTime.UtcNow,
                OrderNumber = "123456"
            });
        }
    }
}
