using Moq;
using Repository.Repositories.Customers;
using Repository.Repositories.Products;
using WebShop.Controllers;

namespace WebShop.Tests;

public class CustomerControllerTests
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();

        // Ställ in mock av Products-egenskapen
    }
}