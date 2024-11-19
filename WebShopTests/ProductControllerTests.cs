using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.Model;
using WebShop;
using WebShop.Controllers;
using Repository.Repositories.Products;
using WebShop.UnitOfWork;

namespace WebShopTests
{

    public class ProductControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);
            _controller = new ProductController(_mockUnitOfWork.Object);

            // Ställ in mock av Products-egenskapen
        }

        [Fact]
        public void GetProduct_ReturnsOkResult_WithAProduct()
        {
            //Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "TestProduct",
                CategoryId = 0
            };
            _mockUnitOfWork.Setup(u => u.Products.Get(product.Id)).Returns(product);

            //Act
            var result = _controller.GetProduct(product.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);

            Assert.Equal(product, returnedProduct);
            _mockUnitOfWork.Verify(u => u.Products.Get(product.Id), Times.Once);
        }


        [Fact]
        public void GetAllProducts_ReturnsOkResult_WithAListOfProducts()
        {
            // Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "TestProduct",
                CategoryId = 0
            };
            Product product2 = new Product
            {
                Id = 2,
                Name = "TestProduct2",
                CategoryId = 0
            };

            _mockUnitOfWork.Setup(u => u.Products.GetAll()).Returns(new List<Product> { product, product2 });

            // Act
            var result = _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsAssignableFrom<List<Product>>(okResult.Value);
            Assert.Equal(2, products.Count);
            Assert.Equal(product, products[0]);

            _mockUnitOfWork.Verify(u => u.Products.GetAll(), Times.Once);
        }

        [Fact]
        public void AddProduct_ReturnsOkResult()
        {
            // Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "TestProduct",
                CategoryId = 0
            };

            // Act
            var result = _controller.AddProduct(product);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal("Product added successfully.", okResult.Value);
            _mockProductRepository.Verify(p => p.Add(It.Is<Product>(p => p == product)), Times.Once);
            _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
        }

        [Fact]
        public void UpdateProduct_ReturnsOkResult()
        {
            // Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "TestProduct",
                CategoryId = 0
            };
            _mockUnitOfWork.Setup(u => u.Products.Get(product.Id)).Returns(product);

            product.Name = "UpdatedName";
            product.CategoryId = 1;

            // Act
            var result = _controller.UpdateProduct(product);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Product with ID {product.Id} updated successfully.", okResult.Value);

            Assert.Equal("UpdatedName", product.Name);
            Assert.Equal(1, product.CategoryId);

            _mockUnitOfWork.Verify(u => u.Products.Update(product), Times.Once);
            _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
        }

        [Fact]
        public void RemoveProduct_ReturnsOkResult()
        {
            //Arrange
            var productId = 1;
            Product product = new Product
            {
                Id = productId,
                Name = "TestProduct",
                CategoryId = 0
            };
            _mockUnitOfWork.Setup(u => u.Products.Get(productId)).Returns(product);

            //Act
            var result = _controller.RemoveProduct(product.Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Product with ID {productId} removed successfully.", okResult.Value);

            _mockUnitOfWork.Verify(u => u.Products.Remove(productId), Times.Once);
            _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
        }


    }
}
