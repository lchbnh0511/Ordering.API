using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Models.RequestModels;
using Ordering.API.Services;
namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly IDatabaseService _databaseService;

        public CustomerController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet]
        [Route("get-customer")]
        public IActionResult GetCustomer()
        {
            var items = _databaseService.GetCustomers();

            return Ok(items);
        }

        [HttpGet]
        [Route("get-orders")]
        public IActionResult GetOrders()
        {
            var items = _databaseService.GetOrders();

            return Ok(items);
        }

        [HttpGet]
        [Route("get-customer/{customerID}")] // /api/Categories/get-products/123 // Path String
        [Route("get-customer-by-id")] // /api/Categories/get-product-by-id/?productId=123 // Query String

        public IActionResult GetCustomers(int customerID)
        {
            var item = _databaseService.GetCustomerByID(customerID);
            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        [Route("add-customer")]
        public IActionResult InsertCustomer([FromBody] CategoryRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _databaseService.Add(model);

                if (result > 0)
                {
                    return Ok(new { msg = "insert successful" });
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("update-customer/{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] CustomerRequestModel model)
        {
            var customer = _databaseService.GetCustomerByID(id);

            if (customer is null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = _databaseService.Update(id, model);

                if (result > 0)
                {
                    return Ok(new { msg = "update successful" });
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("delete-customer/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _databaseService.GetCustomerByID(id);

            if (customer is null)
            {
                return NotFound();
            }

            var orders = _databaseService.GetOrders(id);

            if (orders != null && orders.Count > 0)
            {
                ModelState.AddModelError("", $"This customer has {orders.Count} order");
            }

            if (ModelState.IsValid)
            {
                var result = _databaseService.DeleteCustomer(id);

                if (result > 0)
                {
                    return Ok(new { msg = "delete successful" });
                }
            }

            return BadRequest(ModelState);
        }
    }
}
