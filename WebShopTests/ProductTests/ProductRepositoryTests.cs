using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.Model;
using Repository.Repositories.Products;
using WebShop.Controllers;
using WebShop.Notifications;
using WebShop.UnitOfWork;

namespace WebShopTests.ProductTests;

public class ProductRepositoryTests
{

    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepository;

    private readonly ProductSubject _productSubject;

    public ProductRepositoryTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IProductRepository>();

        _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);
    }

    [Fact]
    public void GetProduct_ReturnsOkResult_WithAProduct()
    {

        // Arrange
        Product product = new Product
        {
            Id = 1,
            Name = "productTest",
            Orders = new List<Order>()
        };

        _mockProductRepository.Setup(repo => repo.Get(product.Id)).Returns(product);


        // Act
        var result = _mockUnitOfWork.Object.Products.Get(product.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal("productTest", result.Name);

        _mockProductRepository.Verify(repo => repo.Get(product.Id), Times.Once);
    }

    [Fact]
    public void GetProduct_ReturnsNull_WhenProductDoesNotExist()
    {
        // Arrange
        int nonExistentProductId = 999;
        _mockProductRepository.Setup(repo => repo.Get(nonExistentProductId)).Returns((Product)null);

        // Act
        var result = _mockUnitOfWork.Object.Products.Get(nonExistentProductId);

        // Assert
        Assert.Null(result);

        _mockProductRepository.Verify(repo => repo.Get(nonExistentProductId), Times.Once);
    }
    [Fact]
    public void GetAllProducts_ReturnsOkResult_WithAListOfProducts()
    {
        // Arrange
        var products = new List<Product>
    {
        new Product { Id = 1, Name = "productTest" },
        new Product { Id = 2, Name = "productTest2"}
    };

        _mockProductRepository.Setup(repo => repo.GetAll()).Returns(products);

        // Act
        var result = _mockUnitOfWork.Object.Products.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _mockProductRepository.Verify(repo => repo.GetAll(), Times.Once);

    }

    [Fact]
    public void GetAllProducts_ReturnsOkResult_WithNullFromRepository()
    {

    }

    [Fact]
    public void AddProduct_ReturnsOkResult()
    {

        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "productTest"
        };

        // Act
        _mockUnitOfWork.Object.Products.Add(product);

        // Assert
        _mockProductRepository.Verify(repo => repo.Add(It.Is<Product>(p => p == product)), Times.Once);
    }


    [Fact]
    public void UpdateProduct_ReturnsOkResult()
    {

        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "productTest"
        };

        _mockUnitOfWork.Setup(u => u.Products.Get(product.Id)).Returns(product);

        // Act
        product.Name = "Updated Name";
        _mockUnitOfWork.Object.Products.Update(product);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.Products.Update(product), Times.Once);
    }
    [Fact]
    public void RemoveProduct_ReturnsOkResult()
    {
        // Arrange
        var productId = 1;
        var product = new Product
        {
            Id = productId,
            Name = "productTest"
        };

        _mockUnitOfWork.Setup(u => u.Products.Get(productId)).Returns(product);

        // Act
        _mockUnitOfWork.Object.Products.Remove(productId);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.Products.Remove(productId), Times.Once);
    }
}