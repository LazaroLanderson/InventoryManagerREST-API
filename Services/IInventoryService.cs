using InventoryManagerREST_API.Models;

namespace InventoryManagerREST_API.Services
{
    public interface IInventoryService
    {
        List<Product> GetAllProducts();
        Product? SearchProductById(int id);
        void AddProduct(Product product);
        bool ProductExists(string sku);
    }
}