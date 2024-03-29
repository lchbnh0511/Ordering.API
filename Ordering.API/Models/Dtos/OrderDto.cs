namespace Ordering.API.Models.Dtos
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; } 
        public int ShipperID { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set;}

        public string? ContactName { get; set; }
    }
}
