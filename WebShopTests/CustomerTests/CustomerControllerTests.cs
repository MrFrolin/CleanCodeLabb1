﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository.Model;
using Repository.Repositories.Customers;
using Repository.Repositories.Products;
using WebShop.Controllers;
using WebShop.UnitOfWork;

namespace WebShopTests.CustomerTests;

public class CustomerControllerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCustomerRepository = new Mock<ICustomerRepository>();

        _mockUnitOfWork.Setup(u => u.Customers).Returns(_mockCustomerRepository.Object);
        _controller = new CustomerController(_mockUnitOfWork.Object);
    }

    [Fact]
    public void GetCustomer_ReturnsOkResult_WithACustomer()
    {
        //Arrange
        Customer customer = new Customer
        {
            Id = 1,
            Name = "TestCustomer",
            Orders = new List<Order>()
        };
        _mockUnitOfWork.Setup(u => u.Customers.Get(customer.Id)).Returns(customer);

        //Act
        var result = _controller.GetCustomer(customer.Id);
        

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCustomer = Assert.IsType<Customer>(okResult.Value);
    }

    [Fact]
    public void GetCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        int nonExistentCustomerId = 999;
        _mockUnitOfWork.Setup(u => u.Customers.Get(nonExistentCustomerId)).Returns((Customer)null);

        // Act
        var result = _controller.GetCustomer(nonExistentCustomerId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void GetAllCustomers_ReturnsOkResult_WithAListOfCustomers()
    {
        // Arrange
        Customer customer = new Customer
        {
            Id = 1,
            Name = "TestCustomer",
            Orders = new List<Order>()
        };
        Customer customer2 = new Customer
        {
            Id = 2,
            Name = "TestCustomer2",
            Orders = new List<Order>()
        };

        _mockUnitOfWork.Setup(u => u.Customers.GetAll()).Returns(new List<Customer> { customer, customer2 });

        // Act
        var result = _controller.GetAllCustomers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<List<Customer>>(okResult.Value);
    }

    [Fact]
    public void AddCustomer_ReturnsOkResult()
    {
        // Arrange
        Customer customer = new Customer
        {
            Id = 1,
            Name = "TestCustomer",
            Orders = new List<Order>()
        };

        // Act
        var result = _controller.AddCustomer(customer);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Customer added successfully.", okResult.Value);
    }

    [Fact]
    public void UpdateCustomer_ReturnsOkResult()
    {
        // Arrange
        Customer customer = new Customer
        {
            Id = 1,
            Name = "TestCustomer",
            Orders = new List<Order>()
        };
        _mockUnitOfWork.Setup(u => u.Customers.Get(customer.Id)).Returns(customer);

        customer.Name = "UpdatedName";

        // Act
        var result = _controller.UpdateCustomer(customer);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Customer with ID {customer.Id} updated successfully.", okResult.Value);
    }

    [Fact]
    public void RemoveCustomer_ReturnsOkResult()
    {
        //Arrange
        var customerId = 1;
        Customer customer = new Customer
        {
            Id = 1,
            Name = "TestCustomer",
            Orders = new List<Order>()
        };
        _mockUnitOfWork.Setup(u => u.Customers.Get(customerId)).Returns(customer);

        //Act
        var result = _controller.RemoveCustomer(customer.Id);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Customer with ID {customerId} removed successfully.", okResult.Value);
    }

    [Fact]
    public void RemoveCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        int nonExistentCustomerId = 999;
        _mockUnitOfWork.Setup(u => u.Customers.Get(nonExistentCustomerId)).Returns((Customer)null);

        // Act
        var result = _controller.RemoveCustomer(nonExistentCustomerId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}