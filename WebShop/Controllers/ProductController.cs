using Microsoft.AspNetCore.Mvc;
using Repository.Model;
using WebShop.UnitOfWork;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor with UnitOfWork injected
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST: api/Product
        [HttpPost]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Product is null.");

            try
            {
                _unitOfWork.Products.Add(product);

                // Save changes
                _unitOfWork.Complete();

                return Ok("Product added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
