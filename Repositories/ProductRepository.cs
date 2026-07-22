using InventoryManagerREST_API.Models;
using System;
using System.Text.Json;

namespace InventoryManagerREST_API.Repositories
{
    public class ProductRepository : IProductRepository
    {


        public ProductRepository()
        {
            Directory.CreateDirectory("Data");

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            products = LoadProducts();
        }

        private readonly string filePath = Path.Combine("Data", "Products.json");

        public List<Product> LoadProducts()
        {

            Console.WriteLine("Reading products...");

            try
            {

                string json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Product>();
                }

                List<Product>? products = JsonSerializer.Deserialize<List<Product>>(json);
                return products ?? new List<Product>();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error loading products: {ex.Message}");
                return new List<Product>();

            }


        }


        public void SaveProducts(List<Product> products)
        {

            Console.WriteLine("Saving products...");

            try
            {

                string json = JsonSerializer.Serialize(products);
                File.WriteAllText(filePath, json);

                Console.WriteLine("Products saved successfully.");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error saving products: {ex.Message}");
            }


        }

        private readonly List<Product> products;

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
            products.Add(product);
            SaveProducts(products);
        }


        public void UpdateProduct(Product product)
        {
            SaveProducts(products);
        }

        public bool DeleteProduct(int id)
        {
            Product? product = SearchProductById(id);

            if (product == null)
            {
                return false;
            }

            products.Remove(product);
            SaveProducts(products);

            return true;

        }

        public bool ProductExists(string sku, int? productIdToIgnore = null)
        {
            return products.Any(p => p.SKU.Equals(sku, StringComparison.OrdinalIgnoreCase)
            && p.ProductId != productIdToIgnore);
        }

    }
}
