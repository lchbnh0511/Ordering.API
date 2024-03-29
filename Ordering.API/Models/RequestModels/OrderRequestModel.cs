using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.API.Models.RequestModels
{
    public class CreateOrderRequestModel
    {
        [Required]
        public CustomerRequestModel? Customer { get; set; }

        [Required]
        public OrderRequestModel? Order { get; set; }

        [Required]
        public List<ProductRequestModel>? Products { get; set; }
    }

    public class CustomerRequestModel
    {
        [Required]
        public string? ContactName { get; set; }

        public string? ContactTitle { get; set; }

        public string? CompanyName { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Phone { get; set; }

        [Required]
        public string? Email { get; set; }
    }

    public class OrderRequestModel
    {
        [Required]
        public string? ShipAddress { get; set; }

        [Required]
        public string? ShipCity { get; set; }
    }

    public class ProductRequestModel
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}