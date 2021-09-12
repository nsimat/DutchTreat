using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : Controller
    {
        private readonly IDutchTreatRepository repo;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IDutchTreatRepository repo, ILogger<ProductsController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        } 
        
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetAll()
        {
            try
            {
                logger.LogInformation("GetAll was called for products.");
                return Ok(repo.GetAllProducts());
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get the products: {ex}");
                return BadRequest("Failed to load products");
            }
            
        }
    }
}
