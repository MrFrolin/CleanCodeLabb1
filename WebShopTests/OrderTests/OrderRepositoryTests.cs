using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.Model;
using Repository;
using WebShop.Controllers;
using WebShop.UnitOfWork;
using Repository.Repositories.Customers;

namespace WebShopTests.OrderTests;

public class OrderRepositoryTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOrderRepository> _mockOrderRepository;

    public OrderRepositoryTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockOrderRepository = new Mock<IOrderRepository>();

        _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepository.Object);
    }

    [Fact]
    public void GetOrder_ReturnsOkResult_WithAnOrder()
    {
        //Arrange
        var orderId = 1;
        Order mockOrder = new Order
        {
            Id = orderId,
            CustomerId = 123,
            OrderDate = DateTime.Now,
            Products = new List<Product>()
        };

        _mockOrderRepository.Setup(repo => repo.Get(orderId)).Returns(mockOrder);

        // Act
        var result = _mockUnitOfWork.Object.Orders.Get(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
        Assert.Equal(mockOrder.CustomerId, result.CustomerId);
        Assert.Equal(mockOrder.OrderDate, result.OrderDate);

        _mockOrderRepository.Verify(repo => repo.Get(orderId), Times.Once);
    }

    [Fact]
    public void GetOrder_ReturnsNull_WhenOrderDoesNotExist()
    {
        // Arrange
        int nonExistentOrderId = 999;
        _mockOrderRepository.Setup(repo => repo.Get(nonExistentOrderId)).Returns((Order)null);

        // Act
        var result = _mockUnitOfWork.Object.Orders.Get(nonExistentOrderId);

        // Assert
        Assert.Null(result);
        _mockOrderRepository.Verify(repo => repo.Get(nonExistentOrderId), Times.Once);
    }

    [Fact]
    public void GetAllOrders_ReturnsOkResult_WithAListOfOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Now },
            new Order { Id = 2, CustomerId = 1, OrderDate = DateTime.Now.AddDays(-1) }
        };

        _mockOrderRepository.Setup(repo => repo.GetAll()).Returns(orders);

        // Act
        var result = _mockUnitOfWork.Object.Orders.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orders, result);
        _mockOrderRepository.Verify(repo => repo.GetAll(), Times.Once);
    }

    [Fact]
    public void AddOrder_ReturnsOkResult()
    {
        // Arrange
        Order order = new Order
        {
            Id = 1,
            CustomerId = 1,
            OrderDate = DateTime.Now,
            Products = new List<Product>()
        };

        _mockOrderRepository.Setup(repo => repo.Add(order));

        // Act
        _mockUnitOfWork.Object.Orders.Add(order);

        // Assert
        _mockOrderRepository.Verify(repo => repo.Add(order), Times.Once);
    }

    [Fact]
    public void UpdateOrder_ReturnsOkResult()
    {
        // Arrange
        Order order = new Order
        {
            Id = 1,
            CustomerId = 1,
            OrderDate = DateTime.Now,
            Products = new List<Product>()
        };
        _mockOrderRepository.Setup(repo => repo.Update(order));

        // Act
        _mockUnitOfWork.Object.Orders.Update(order);

        // Assert
        _mockOrderRepository.Verify(repo => repo.Update(order), Times.Once);
    }

    [Fact]
    public void RemoveOrder_ReturnsOkResult()
    {
        // Arrange
        Order order = new Order
        {
            Id = 1,
            CustomerId = 1,
            OrderDate = DateTime.Now,
            Products = new List<Product>()
        };
        _mockOrderRepository.Setup(repo => repo.Remove(order.Id));

        // Act
        _mockUnitOfWork.Object.Orders.Remove(order.Id);

        // Assert
        _mockOrderRepository.Verify(repo => repo.Remove(order.Id), Times.Once);
    }

    [Fact]
    public void RemoveOrder_ReturnsNull_WhenOrderDoesNotExist()
    {
        // Arrange
        int nonExistentOrderId = 999;
        _mockOrderRepository.Setup(repo => repo.Get(nonExistentOrderId)).Returns((Order)null);

        // Act
        var result = _mockUnitOfWork.Object.Orders.Get(nonExistentOrderId);

        // Assert
        Assert.Null(result);
        _mockOrderRepository.Verify(repo => repo.Get(nonExistentOrderId), Times.Once);
    }
}