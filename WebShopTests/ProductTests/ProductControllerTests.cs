using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.Model;
using WebShop;
using WebShop.Controllers;
using Repository.Repositories.Products;
using WebShop.Notifications;
using WebShop.UnitOfWork;

namespace WebShopTests.ProductTests
{

    public class ProductControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<INotificationObserver> _mockObserver;

        private readonly ProductSubject _productSubject;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockObserver = new Mock<INotificationObserver>();

            _productSubject = new ProductSubject();
            _productSubject.Attach(_mockObserver.Object);

            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);
            _controller = new ProductController(_mockUnitOfWork.Object, _productSubject);

        }

        [Fact]
        public void GetProduct_ReturnsOkResult_WithAProduct()
        {
            //Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "TestProduct"
            };
            _mockUnitOfWork.Setup(u => u.Products.Get(product.Id)).Returns(product);

            //Act
            var result = _controller.GetProduct(product.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result); 
            Assert.IsType<Product>(okResult.Value);
        }

        [Fact]
        public void GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            //Arrange
            int nonExistentProductId = 999;
            _mockUnitOfWork.Setup(u => u.Products.Get(nonExistentProductId)).Returns((Product)null);

            //Act
            var result = _controller.GetProduct(nonExistentProductId);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public void GetAllProducts_ReturnsOkResult_WithAListOfProducts()
        {
            // Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "TestProduct"
            };
            Product product2 = new Product
            {
                Id = 2,
                Name = "TestProduct2"
            };

            _mockUnitOfWork.Setup(u => u.Products.GetAll()).Returns(new List<Product> { product, product2 });

            // Act
            var result = _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<List<Product>>(okResult.Value);
        }

        [Fact]
        public void GetAllProducts_ReturnsOkResult_WithNullFromRepository()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Products.GetAll()).Returns((List<Product>)null);

            // Act
            var result = _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseMessage = Assert.IsType<string>(okResult.Value);
            Assert.Equal("No products found.", responseMessage);
        }

        [Fact]
        public void AddProduct_ReturnsOkResult()
        {
            // Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "TestProduct"
            };

            // Act
            var result = _controller.AddProduct(product);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Product added successfully.", okResult.Value);
        }

        [Fact]
        public void UpdateProduct_ReturnsOkResult()
        {
            // Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "TestProduct"
            };
            _mockUnitOfWork.Setup(u => u.Products.Get(product.Id)).Returns(product);

            product.Name = "UpdatedName";

            // Act
            var result = _controller.UpdateProduct(product);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Product with ID {product.Id} updated successfully.", okResult.Value);
        }

        [Fact]
        public void RemoveProduct_ReturnsOkResult()
        {
            //Arrange
            var productId = 1;
            Product product = new Product
            {
                Id = productId,
                Name = "TestProduct"
            };
            _mockUnitOfWork.Setup(u => u.Products.Get(productId)).Returns(product);

            //Act
            var result = _controller.RemoveProduct(product.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Product with ID {productId} removed successfully.", okResult.Value);
        }

        [Fact]
        public void RemoveProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            //Arrange
            int nonExistentProductId = 999;
            _mockUnitOfWork.Setup(u => u.Products.Get(nonExistentProductId)).Returns((Product)null);

            //Act
            var result = _controller.RemoveProduct(nonExistentProductId);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
