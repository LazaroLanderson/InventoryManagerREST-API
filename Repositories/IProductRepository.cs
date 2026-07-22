using InventoryManagerREST_API.Models;


namespace InventoryManagerREST_API.Repositories
{
    public interface IProductRepository
    {

        List<Product> LoadProducts();
        void SaveProducts(List<Product> products);


    }
}
