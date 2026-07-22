using InventoryManagerREST_API.Models;
using InventoryManagerREST_API.Repositories;

namespace InventoryManagerREST_API.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IProductRepository productRepository;

        public InventoryService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public List<Product> GetAllProducts()
        {
            return productRepository.GetAllProducts();
        }

        public Product? SearchProductById(int id)
        {
            return productRepository.SearchProductById(id);
        }

        public string? AddProduct(Product product)
        {

            string? validationError = ValidateProduct(product);

            if (validationError != null)
            {
                return validationError;
            }

            List<Product> products = productRepository.GetAllProducts();

            int newId = products.Count == 0
                ? 1
                : products.Max(p => p.ProductId) + 1;
            product.ProductId = newId;
            productRepository.AddProduct(product);

            return null;

        }

        private bool ProductExists(string sku, int? productIdToIgnore = null)
        {
            return productRepository.ProductExists(sku, productIdToIgnore);
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

            productRepository.UpdateProduct(existingProduct);

            return null;

        }

        public bool DeleteProduct(int id)
        {
            return productRepository.DeleteProduct(id);
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