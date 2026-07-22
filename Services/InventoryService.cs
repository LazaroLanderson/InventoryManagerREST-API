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

        public bool UpdateProduct(int id, Product updatedProduct)
        {
            Product? existingProduct = SearchProductById(id);

            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.SKU = updatedProduct.SKU;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.QuantityOnHand = updatedProduct.QuantityOnHand;

            productRepository.SaveProducts(products);

            return true;

        }

        public bool DeleteProduct(int id)
        {
            Product? productToDelete = SearchProductById(id);
            if (productToDelete == null)
            {
                return false;
            }
            products.Remove(productToDelete);
            productRepository.SaveProducts(products);
            return true;
        }

    }
}