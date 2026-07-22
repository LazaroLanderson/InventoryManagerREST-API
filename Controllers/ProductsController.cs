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
        public IActionResult GetAllProducts([FromQuery] string? sortBy)
        {
            var products = inventoryService.GetAllProducts();
            if ((sortBy ?? string.Empty).Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                products = products.OrderBy(p => p.Name).ToList();
            }
            else if ((sortBy ?? string.Empty).Equals("price", StringComparison.OrdinalIgnoreCase))
            {
                products = products.OrderBy(p => p.Price).ToList();
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {

            if (id <= 0)
            {
                return BadRequest("Invalid product ID.");
            }

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

            string? error = inventoryService.AddProduct(product);

            if (error != null)
            {
                return BadRequest(error);
            }

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {

            if (id <= 0)
            {
                return BadRequest("Invalid product ID.");
            }

            if (id != updatedProduct.ProductId)
            {
                return BadRequest("Product ID mismatch.");
            }

            string? error = inventoryService.UpdateProduct(id, updatedProduct);

            if (error == "Product not found.")
            {
                return NotFound(error);
            }

            if (error != null)
            {
                return BadRequest(error);
            }

            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid product ID.");
            }

            bool success = inventoryService.DeleteProduct(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}