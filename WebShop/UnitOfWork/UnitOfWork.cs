using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Data;
using Repository.Model;
using Repository.Repositories;
using Repository.Repositories.Customers;
using Repository.Repositories.Products;

namespace WebShop.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MyDbContext _context;

        private readonly DbSet<Product> _productDbSet;
        private readonly DbSet<Customer> _customerDbSet;
        private readonly DbSet<Order> _orderDbSet;

        public IProductRepository Products { get; set; }
        public ICustomerRepository Customers { get; set; }
        public IOrderRepository Orders { get; set; }

        public UnitOfWork(MyDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            Products = new ProductRepository(_context, _productDbSet);
            Customers = new CustomerRepository(_context, _customerDbSet);
            Orders = new OrderRepository(_context, _orderDbSet);
        }


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
