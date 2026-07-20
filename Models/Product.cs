using System;

namespace InventoryManagerREST_API.Models
{
    public class Product
    {

        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int QuantityOnHand { get; set; }



    }
}
