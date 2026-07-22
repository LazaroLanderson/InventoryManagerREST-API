using InventoryManagerREST_API.Models;

namespace InventoryManagerREST_API.Services
{
    public interface IInventoryService
    {
        List<Product> GetAllProducts();
        Product? SearchProductById(int id);
        string? AddProduct(Product product);
        string? UpdateProduct(int id, Product updatedProduct);
        bool DeleteProduct(int id);
    }
}