
using Moq;
using Repository.Model;
using Repository.Repositories.Products;
using WebShop.Controllers;
using WebShop.Notifications;
using WebShop.UnitOfWork;

namespace WebShopTests
{
    public class UnitOfWorkTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<INotificationObserver> _mockObserver;
        private readonly ProductController _controller;
        private readonly ProductSubject _mockProductSubject;

        public UnitOfWorkTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);

            _mockObserver = new Mock<INotificationObserver>();
            
            _mockProductSubject = new ProductSubject();
            _mockProductSubject.Attach(_mockObserver.Object);
            _controller = new ProductController(_mockUnitOfWork.Object, _mockProductSubject);
        }

        [Fact]
        public void NotifyProductAdded_CallsObserverUpdate()
        {
            Product product = new Product
            // Arrange
            {
                Id = 1,
                Name = "TestProduct"
            };

            // Act
            _controller.AddProduct(product);

            // Assert
            _mockObserver.Verify(p => p.Update(product), Times.Once);
        }
    }
}
