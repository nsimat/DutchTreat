using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
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
        private readonly IMapper mapper;

        public OrdersController(IDutchTreatRepository repo, ILogger<OrdersController> logger, IMapper mapper)
        {
            this.repo = repo;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll(bool includeItems = true)
        {
            try
            {
                logger.LogInformation("GetAll was called for orders in OrdersController.");
                var results = repo.GetAllOrders(includeItems);
                return Ok(mapper.Map<IEnumerable<OrderViewModel>>(results));
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
                if (order != null) return Ok(mapper.Map<Order, OrderViewModel>(order));
                else return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get order: {ex}");
                return BadRequest("Failed to get order from OrdersController");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderViewModel model)
        {
            try
            {
                logger.LogInformation("Post was called in OrdersController");
                if (ModelState.IsValid)
                {
                    var newOrder = mapper.Map<OrderViewModel, Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    repo.AddEntity(newOrder);
                    if (repo.SaveAll())
                    {
                        return Created($"/api/orders/{newOrder.OrderId}", mapper.Map<Order, OrderViewModel>(newOrder));
                    }

                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to save new order: {ex}");
                return BadRequest("Failed to save new order");
            }

            return BadRequest("Failed to create new order");
        }
    }
}
