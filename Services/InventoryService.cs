using InventoryManagerREST_API.Models;
using InventoryManagerREST_API.Repositories;

namespace InventoryManagerREST_API.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly List<Product> products;
        private readonly IProductRepository productRepository;

        public InventoryService(IProductRepository productRepository)
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

        public string? AddProduct(Product product)
        {

            string? validationError = ValidateProduct(product);

            if (validationError != null)
            {
                return validationError;
            }

            int newId = products.Count == 0
                ? 1
                : products.Max(p => p.ProductId) + 1;
            product.ProductId = newId;
            products.Add(product);
            productRepository.SaveProducts(products);

            return null;

        }

        private bool ProductExists(string sku, int? productIdToIgnore = null)
        {
            return products.Any(p => p.SKU.Equals(sku, StringComparison.OrdinalIgnoreCase) 
            && p.ProductId != productIdToIgnore);
        }

        public string? UpdateProduct(int id, Product updatedProduct)
        {
            Product? existingProduct = SearchProductById(id);

            if (existingProduct == null)
            {
                return "Product not found.";
            }

            string? validationError = ValidateProduct(updatedProduct, id);

            if (validationError != null)
            {
                return validationError;
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.SKU = updatedProduct.SKU;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.QuantityOnHand = updatedProduct.QuantityOnHand;

            productRepository.SaveProducts(products);

            return null;

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

        private string? ValidateProduct(Product product, int? productIdToIgnore = null)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return "Product name cannot be empty.";
            }

            if (product.Price <= 0)
            {
                return "Product price must be greater than zero.";
            }

            if (product.QuantityOnHand < 0)
            {
                return "Product quantity on hand cannot be negative.";
            }

            if (string.IsNullOrWhiteSpace(product.SKU))
            {
                return "Product SKU cannot be empty.";
            }

            if (ProductExists(product.SKU, productIdToIgnore))
            {
                return "A product with the same SKU already exists.";
            }

            return null;
        }

    }
}