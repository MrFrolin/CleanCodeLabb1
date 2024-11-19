using Repository;
using Repository.Repositories;
using Repository.Repositories.Customers;
using Repository.Repositories.Products;

namespace WebShop.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        int Complete();
    }
}
