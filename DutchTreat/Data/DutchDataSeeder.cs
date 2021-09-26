using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<StoreUser> userManager;

        public DutchDataSeeder(DutchTreatDbContext context, IWebHostEnvironment env, UserManager<StoreUser> userManager)
        {
            this.context = context;
            this.env = env;
            this.userManager = userManager;
        }

        public async Task SeedDataAsync()
        {
            //To ensure that the database exists
            context.Database.EnsureCreated();

            StoreUser user = await userManager.FindByEmailAsync("brian.kapesa@dutchtreat.com");

            if(user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Brian",
                    LastName = "Kapesa",
                    Email = "brian.kapesa@dutchtreat.com",
                    UserName = "brian.kapesa@dutchtreat.com"
                };

                var result = await userManager.CreateAsync(user, "P@ssw0rd!");
                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in the seeder");
                }
            }

            if (!context.Products.Any())
            {
                //Need to create the Sample data
                var filePath = Path.Combine(env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                context.Products.AddRange(products);

                var order = context.Orders.Where(o => o.OrderId == 1).FirstOrDefault();

                if(order != null)
                {
                    order.User = user;

                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }                

                context.SaveChanges();
            }
        }
    }
}
