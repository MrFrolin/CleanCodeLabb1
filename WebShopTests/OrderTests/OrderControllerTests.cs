using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository;
using Repository.Model;
using Repository.Repositories;
using Repository.Repositories.Products;
using WebShop.Controllers;
using WebShop.UnitOfWork;

namespace WebShopTests.OrderTests;

public class OrderControllerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockOrderRepository = new Mock<IOrderRepository>();

        _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepository.Object);
        _controller = new OrderController(_mockUnitOfWork.Object);
    }

    [Fact]
    public void GetOrder_ReturnsOkResult_WithAnOrder()
    {
        // Arrange
        Order order = new Order
        {
            Id = 1,
            CustomerId = 123,
            OrderDate = DateTime.Now,
            Products = new List<Product>()
        };
        _mockUnitOfWork.Setup(u => u.Orders.Get(order.Id)).Returns(order);

        // Act
        var result = _controller.GetOrder(order.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<Order>(okResult.Value);
    }

    [Fact]
    public void GetOrder_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        int nonExistentOrderId = 999;
        _mockUnitOfWork.Setup(u => u.Orders.Get(nonExistentOrderId)).Returns((Order)null);

        // Act
        var result = _controller.GetOrder(nonExistentOrderId);


        // Assert
        var badRequestResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Order with ID {nonExistentOrderId} not found.", badRequestResult.Value);

    }

    [Fact]
    public void GetAllOrders_ReturnsOkResult_WithAListOfOrders()
    {
        // Arrange
        Order order1 = new Order
        {
            Id = 1,
            CustomerId = 123,
            OrderDate = DateTime.Now,
            Products = new List<Product>()
        };
        Order order2 = new Order
        {
            Id = 2,
            CustomerId = 456,
            OrderDate = DateTime.Now,
            Products = new List<Product>()
        };

        _mockUnitOfWork.Setup(u => u.Orders.GetAll()).Returns(new List<Order> { order1, order2 });

        // Act
        var result = _controller.GetAllOrders();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<List<Order>>(okResult.Value);
    }

    [Fact]
    public void AddOrder_ReturnsOkResult()
    {
        // Arrange
        Order order = new Order
        {
            Id = 1,
            CustomerId = 123,
            OrderDate = DateTime.Now,
            Products = new List<Product>()
        };

        // Act
        var result = _controller.AddOrder(order);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Order added successfully.", okResult.Value);
    }

    [Fact]
    public void AddOrder_WithProduct_ReturnsOkResult()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product1" };
        _mockUnitOfWork.Setup(u => u.Products.Get(It.IsAny<int>())).Returns(product);


        var order = new Order
        {
            Id = 1,
            CustomerId = 123,
            OrderDate = DateTime.Now,
            Products = [product]
        };

        // Act
        var result = _controller.AddOrder(order);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Order added successfully.", okResult.Value);
    }

    [Fact]
    public void UpdateOrder_ReturnsOkResult()
    {
        // Arrange
        Order order = new Order
        {
            Id = 1,
            CustomerId = 123,
            OrderDate = DateTime.Now,
            Products = new List<Product>()
        };
        _mockUnitOfWork.Setup(u => u.Orders.Get(order.Id)).Returns(order);

        //korrigera efter beslut vad som ska uppdateras i ordern(OrderStatus?)
        order.CustomerId = 456;

        // Act
        var result = _controller.UpdateOrder(order);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Order with ID {order.Id} updated successfully.", okResult.Value);
    }

    [Fact]
    public void RemoveOrder_ReturnsOkResult()
    {
        // Arrange
        var orderId = 1;
        Order order = new Order
        {
            Id = 1,
            CustomerId = 123,
            OrderDate = DateTime.Now,
            Products = new List<Product>()
        };
        _mockUnitOfWork.Setup(u => u.Orders.Get(orderId)).Returns(order);

        // Act
        var result = _controller.RemoveOrder(order.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Order with ID {orderId} removed successfully.", okResult.Value);
    }

    [Fact]
    public void RemoveOrder_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        int nonExistentOrderId = 999;
        _mockUnitOfWork.Setup(u => u.Orders.Get(nonExistentOrderId)).Returns((Order)null);

        // Act
        var result = _controller.RemoveOrder(nonExistentOrderId);

        // Assert
        var badRequestResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Order with ID {nonExistentOrderId} not found.", badRequestResult.Value);
    }
}