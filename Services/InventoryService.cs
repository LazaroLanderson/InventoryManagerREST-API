using InventoryManagerREST_API.Models;
using InventoryManagerREST_API.Repositories;

namespace InventoryManagerREST_API.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly List<Product> products;
        private readonly ProductRepository productRepository;

        public InventoryService(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
            products = productRepository.LoadProducts();
        }

        public List<Product> GetAllProducts()
        {
            return products;
        }

        public Product? SearchProductById(int id)
        {
            return products.FirstOrDefault(p => p.ProductId == id);
        }

        public void AddProduct(Product product)
        {

            int newId = products.Count == 0
                ? 1
                : products.Max(p => p.ProductId) + 1;
            product.ProductId = newId;
            products.Add(product);
            productRepository.SaveProducts(products);

        }

        public bool ProductExists(string sku)
        {
            return products.Any(p => p.SKU.Equals(sku, StringComparison.OrdinalIgnoreCase));
        }

    }
}