using InventoryManagerREST_API.Services;
using Microsoft.AspNetCore.Mvc;
using InventoryManagerREST_API.Models;

namespace InventoryManagerREST_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {


        private readonly IInventoryService inventoryService;

        public ProductsController(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = inventoryService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = inventoryService.SearchProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);

        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            if (inventoryService.ProductExists(product.SKU))
            {
                return BadRequest("A product with the same SKU already exists.");
            }


            inventoryService.AddProduct(product);

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

    }

}