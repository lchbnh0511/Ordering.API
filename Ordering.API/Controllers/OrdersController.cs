using Microsoft.AspNetCore.Mvc;
using Ordering.API.Services;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IDatabaseService _databaseService;
        public OrdersController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet]
        [Route("get-orders")]
        public IActionResult GetOrders()
        {
            var items = _databaseService.GetOrders();

            return Ok(items);
        }

        [HttpGet]
        [Route("get-orders/{orderID}")]
        public IActionResult GetOrders(int orderID)
        {
            var item = _databaseService.GetOrderByID(orderID);
            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }
        [HttpGet]
        [Route("get-customer")]
        public IActionResult GetCustomer()
        {
            var items = _databaseService.GetCustomers();

            return Ok(items);
        }

    }
}
