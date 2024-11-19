using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Model;

namespace Repository.Repositories.Customers;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(MyDbContext context, DbSet<Customer> dbSet) : base(context, dbSet)
    {
    }
}