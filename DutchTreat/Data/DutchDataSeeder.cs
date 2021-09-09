using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchDataSeeder
    {
        private readonly DutchTreatDbContext context;
        private readonly IWebHostEnvironment env;

        public DutchDataSeeder(DutchTreatDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        public void SeedData()
        {
            context.Database.EnsureCreated();

            if (!context.Products.Any())
            {
                //Need to create the Sample data
                var filePath = Path.Combine(env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                context.Products.AddRange(products);

                var order = new Order
                {
                    OrderDate = DateTime.Today,
                    OrderNumber = "10000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                context.Orders.Add(order);

                context.SaveChanges();
            }
        }
    }
}
