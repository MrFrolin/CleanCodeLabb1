using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.Model;
using Repository.Repositories.Customers;
using WebShop.Controllers;
using WebShop.UnitOfWork;

namespace WebShopTests.CustomerTests;

public class CustomerRepositoryTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;

    public CustomerRepositoryTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCustomerRepository = new Mock<ICustomerRepository>();

        _mockUnitOfWork.Setup(u => u.Customers).Returns(_mockCustomerRepository.Object);
    }

    [Fact]
    public void GetCustomer_ReturnsOkResult_WithACustomer()
    {
        // Arrange
        var customerId = 1;
        var mockCustomer = new Customer
        {
            Id = customerId,
            Name = "TestCustomer",
        };

        _mockCustomerRepository.Setup(repo => repo.Get(customerId)).Returns(mockCustomer);

        // Act
        var result = _mockUnitOfWork.Object.Customers.Get(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.Id);
        Assert.Equal("TestCustomer", result.Name);

        _mockCustomerRepository.Verify(repo => repo.Get(customerId), Times.Once);
    }
    [Fact]
    public void GetCustomer_ReturnsOkResult_WithCustomerHavingOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.Now },
            new Order { Id = 2, CustomerId = 1, OrderDate = DateTime.Now.AddDays(-1) }
        };

        var customer = new Customer
        {
            Id = 1,
            Name = "TestCustomer",
            Orders = orders
        };

        _mockCustomerRepository.Setup(repo => repo.Get(customer.Id)).Returns(customer);

        // Act
        var result = _mockUnitOfWork.Object.Customers.Get(customer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.Id, result.Id);
        Assert.Equal(customer.Name, result.Name);
        Assert.Equal(orders, result.Orders);
        _mockCustomerRepository.Verify(repo => repo.Get(customer.Id), Times.Once);
    }

    [Fact]
    public void GetCustomer_ReturnsNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        int nonExistentCustomerId = 999;
        _mockCustomerRepository.Setup(repo => repo.Get(nonExistentCustomerId)).Returns((Customer)null);

        // Act
        var result = _mockUnitOfWork.Object.Customers.Get(nonExistentCustomerId);

        // Assert
        Assert.Null(result);
        _mockCustomerRepository.Verify(repo => repo.Get(nonExistentCustomerId), Times.Once);
    }

    [Fact]
    public void GetAllCustomers_ReturnsOkResult_WithAListOfCustomer()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "TestCustomer1", Orders = new List<Order>() },
            new Customer { Id = 2, Name = "TestCustomer2", Orders = new List<Order>() }
        };

        _mockCustomerRepository.Setup(repo => repo.GetAll()).Returns(customers);

        // Act
        var result = _mockUnitOfWork.Object.Customers.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _mockCustomerRepository.Verify(repo => repo.GetAll(), Times.Once);
    }

    [Fact]
    public void AddCustomer_ReturnsOkResult()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "TestCustomer",
            Orders = new List<Order>()
        };

        // Act
        _mockUnitOfWork.Object.Customers.Add(customer);

        // Assert
        _mockCustomerRepository.Verify(repo => repo.Add(It.Is<Customer>(c => c == customer)), Times.Once);
    }

    [Fact]
    public void UpdateCustomer_ReturnsOkResult()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "TestCustomer",
            Orders = new List<Order>()
        };

        _mockUnitOfWork.Setup(u => u.Customers.Get(customer.Id)).Returns(customer);

        // Act
        customer.Name = "Updated Name";
        _mockUnitOfWork.Object.Customers.Update(customer);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.Customers.Update(customer), Times.Once);
    }

    [Fact]
    public void RemoveCustomer_ReturnsOkResult()
    {
        // Arrange
        var customerId = 1;
        var customer = new Customer
        {
            Id = customerId,
            Name = "TestCustomer",
            Orders = new List<Order>()
        };

        _mockUnitOfWork.Setup(u => u.Customers.Get(customerId)).Returns(customer);

        // Act
        _mockUnitOfWork.Object.Customers.Remove(customerId);

        // Assert
        _mockUnitOfWork.Verify(uow => uow.Customers.Remove(customerId), Times.Once);
    }
}
