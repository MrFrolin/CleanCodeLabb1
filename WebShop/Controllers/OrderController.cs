using Microsoft.AspNetCore.Mvc;
using Repository.Model;
using WebShop.UnitOfWork;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor with UnitOfWork injected
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST: api/Order
        [HttpPost]
        public IActionResult AddOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Order is null.");

            try
            {
                _unitOfWork.Orders.Add(order);

                // Save changes
                _unitOfWork.Complete();

                return Ok("Order added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
