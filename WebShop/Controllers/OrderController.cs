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

        // GET: api/Order/id
        [HttpGet("{orderId}")]
        public IActionResult GetOrder(int orderId)
        {

            if (orderId == null)
            {
                return BadRequest("Order Id is null");
            }

            try
            {
                var order = _unitOfWork.Orders.Get(orderId);

                if (order == null)
                    return NotFound($"Order with ID {orderId} not found.");

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Order
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            try
            {
                var orders = _unitOfWork.Orders.GetAll();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Order
        [HttpPost]
        public IActionResult AddOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Order is null.");

            try
            {
                order.Products = GetProductsFromDb(order.Products);

                _unitOfWork.Orders.Add(order);
                _unitOfWork.Complete();

                return Ok("Order added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Order
        [HttpPut]
        public IActionResult UpdateOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Order is null.");

            try
            {
                var existingOrder = _unitOfWork.Orders.Get(order.Id);
                if (existingOrder == null)
                    return NotFound($"Order with ID {order.Id} not found.");

                _unitOfWork.Orders.Update(order);
                _unitOfWork.Complete();

                return Ok($"Order with ID {order.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Order/id
        [HttpDelete("{orderId}")]
        public IActionResult RemoveOrder(int orderId)
        {
            try
            {
                var order = _unitOfWork.Orders.Get(orderId);
                if (order == null)
                    return NotFound($"Order with ID {orderId} not found.");

                _unitOfWork.Orders.Remove(orderId);
                _unitOfWork.Complete();

                return Ok($"Order with ID {orderId} removed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private List<Product> GetProductsFromDb(List<Product> productsList)
        {
            var prodList = new List<Product>();
            foreach (var product in productsList)
            {
                var productDb = _unitOfWork.Products.Get(product.Id);
                prodList.Add(productDb);
            }
            return prodList;
        }
    }
}
