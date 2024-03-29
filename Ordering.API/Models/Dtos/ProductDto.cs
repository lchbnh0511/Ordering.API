namespace Ordering.API.Models.Dtos
{
    public class ProductDto
    {
        public int ID { get; set; }
        public string? ProductName { get; set; }
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public int QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitInStock { get; set; }

        public string? CategoryName { get; set; }
        public string? SupplierName { get; set; }
    }
}
