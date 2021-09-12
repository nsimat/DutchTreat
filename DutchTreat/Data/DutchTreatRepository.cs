using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchTreatRepository : IDutchTreatRepository
    {
        private readonly DutchTreatDbContext ctx;
        private readonly ILogger<DutchTreatRepository> logger;

        public DutchTreatRepository(DutchTreatDbContext ctx, ILogger<DutchTreatRepository> logger)
        {
            this.ctx = ctx;
            this.logger = logger;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return ctx.Orders
                      .Include(o => o.Items)
                      .ThenInclude(i => i.Product)
                      .ToList();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                logger.LogInformation("GetAllProducts was called...");

                return ctx.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get all products: {ex}");
                return null;
            }  
            
        }

        public Order GetOrderById(int id)
        {
            try
            {
                logger.LogInformation("GetOrderById was called.");
                return ctx.Orders
                          .Include(o => o.Items)
                          .ThenInclude(i => i.Product)
                          .Where(o => o.Id == id)
                          .FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get order: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try
            {
                logger.LogInformation("GetProductsByCategory was called.");
                return ctx.Products.Where(p => p.Category == category).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get all products according their category: {ex}.");
                return null;
            }
            
        }

        public bool Save()
        {
            try
            {
                logger.LogInformation("Save was called.");
                return ctx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to save data: {ex}.");
                return false;
            }
            
        }
    }
}
