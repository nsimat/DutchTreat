using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : Controller
    {
        private readonly IDutchTreatRepository repo;
        private readonly ILogger logger;

        public OrdersController(IDutchTreatRepository repo, ILogger<OrdersController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                logger.LogInformation("GetAll was called for orders in OrdersController.");
                return Ok(repo.GetAllOrders());
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get orders.");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                logger.LogInformation("Get was called in OrdersController.");

                var order = repo.GetOrderById(id);
                if (order != null) return Ok(repo.GetOrderById(id));
                else return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get order: {ex}");
                return BadRequest("Failed to get order from OrdersController");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Order model)
        {
            try
            {
                logger.LogInformation("Post was called in OrdersController");
                repo.AddEntity(model);
                if (repo.SaveAll())
                {
                    return Created($"/api/orders/{model.Id}", model);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to save new order: {ex}");
                return BadRequest("Failed to save new order");
            }
            return BadRequest("Failed to save new order");
        }
    }
}
