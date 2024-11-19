using Moq;
using Repository;
using Repository.Repositories;
using Repository.Repositories.Products;
using WebShop.Controllers;

namespace WebShop.Tests;

public class OrderControllerTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly OrderRepository _controller;

    public OrderControllerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();

        // Ställ in mock av Products-egenskapen
    }
}