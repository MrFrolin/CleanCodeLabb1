using Microsoft.AspNetCore.Mvc;
using Repository.Model;
using WebShop.Notifications;
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

        // GET: api/Customer/id
        [HttpGet("{customerId}")]
        public IActionResult GetCustomer(int customerId)
        {
            if (customerId == null)
            {
                return BadRequest("Customer Id is null");
            }
            try
            {
                var customer = _unitOfWork.Customers.Get(customerId);
                if (customer == null)
                {
                    return NotFound("No customer found");
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Customer
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            try
            {
                var customers = _unitOfWork.Customers.GetAll();
                if (customers == null || !customers.Any())
                {
                    return Ok("No customers found");
                }
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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

        // PUT: api/Customer
        [HttpPut]
        public IActionResult UpdateCustomer([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest("Customer is null.");

            try
            {

                _unitOfWork.Customers.Update(customer);

                // Save changes
                _unitOfWork.Complete();

                return Ok($"Customer with ID {customer.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Customer/id
        [HttpDelete("{customerId}")]
        public IActionResult RemoveCustomer(int customerId)
        {
            if (customerId == null)
                return BadRequest("Customer Id is null.");

            try
            {
                var customer = _unitOfWork.Customers.Get(customerId);
                if (customer == null)
                {
                    return NotFound("No customer found");
                }

                _unitOfWork.Customers.Remove(customerId);

                _unitOfWork.Complete();

                return Ok($"Customer with ID {customerId} removed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
