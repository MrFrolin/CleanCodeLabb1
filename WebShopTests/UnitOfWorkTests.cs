
using Microsoft.EntityFrameworkCore;
using Moq;
using Repository.Data;
using Repository.Model;
using Repository.Repositories.Products;
using WebShop.Controllers;
using WebShop.Notifications;
using WebShop.UnitOfWork;

namespace WebShopTests
{
    public class UnitOfWorkTests
    {
        private readonly Mock<MyDbContext> _mockContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<INotificationObserver> _mockObserver;
        private readonly ProductController _controller;
        private readonly ProductSubject _productSubject;

        public UnitOfWorkTests()
        {
            _mockContext = new Mock<MyDbContext>(new DbContextOptions<MyDbContext>());

            _mockProductRepository = new Mock<IProductRepository>();

            _unitOfWork = new UnitOfWork(_mockContext.Object);
            _unitOfWork.Products = _mockProductRepository.Object;

            _mockObserver = new Mock<INotificationObserver>();
            _productSubject = new ProductSubject();
            _productSubject.Attach(_mockObserver.Object);

            _controller = new ProductController(_unitOfWork, _productSubject);
        }

        [Fact]
        public void NotifyProductAdded_CallsObserverUpdate()
        {
            // Arrange
            Product product = new Product
            {
                Id = 1,
                Name = "TestProduct"
            };

            // Act
            _controller.AddProduct(product);

            // Assert
            _mockObserver.Verify(p => p.Update(product), Times.Once);
        }

        [Fact]
        public void Complete_SaveChangesCalled_ReturnsOkValue()
        {
            // Arrange
            int expectedChange = 1;
            _mockContext.Setup(c => c.SaveChanges()).Returns(expectedChange);

            // Act
            int result = _unitOfWork.Complete();

            // Assert
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.Equal(expectedChange, result);
        }

        [Fact]
        public void Complete_SaveChanges_CatchException()
        {
            // Arrange
            _mockContext.Setup(c => c.SaveChanges()).Throws(new InvalidOperationException("Test Exception"));

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => _unitOfWork.Complete());

            //Assert
            Assert.Equal("Test Exception", exception.Message);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}
