using Microsoft.AspNetCore.Mvc;
using Repository.Model;
using WebShop.UnitOfWork;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor with UnitOfWork injected
        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //POST: api/Customer
        [HttpPost]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest("Customer is null.");

            try
            {
                _unitOfWork.Customers.Add(customer);

                // Save changes
                _unitOfWork.Complete();

                return Ok("Customer added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
