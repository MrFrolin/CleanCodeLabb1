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

        // GET: api/Product/id
        [HttpGet("{productId}")]
        public IActionResult GetProduct(int productId)
        {
            if (productId == null)
            {
                return BadRequest("Product Id is null");
            }
            try
            {
                var product = _unitOfWork.Products.Get(productId);
                if (product == null)
                {
                    return NotFound("No product found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // GET: api/Product
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _unitOfWork.Products.GetAll();
                if (products == null || !products.Any())
                {
                    return Ok("No products found");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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

        //Update: api/Product
        [HttpPut]
        public IActionResult UpdateProduct(Product product)
        {
            if(product == null)
            {
                return BadRequest("Product is null");
            }

            try
            {
                _unitOfWork.Products.Update(product);
                _unitOfWork.Complete();
                return Ok($"Product with ID {product.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        //Delete: api/Product
        [HttpDelete("{productId}")]
        public IActionResult RemoveProduct(int productId)
        {
            if (productId == null)
            {
                return BadRequest("Product Id is null");
            }

            try
            {
                _unitOfWork.Products.Remove(productId);
                _unitOfWork.Complete();

                return Ok($"Product with ID {productId} removed successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
