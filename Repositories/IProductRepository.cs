using InventoryManagerREST_API.Models;


namespace InventoryManagerREST_API.Repositories
{
    public interface IProductRepository
    {

        List<Product> LoadProducts();
        void SaveProducts(List<Product> products);
        void AddProduct(Product product);
        List<Product> GetAllProducts();
        Product? SearchProductById(int id);
        void UpdateProduct(Product product);
        bool DeleteProduct(int id);
        bool ProductExists(string sku, int? productIdToIgnore = null);

    }
}
