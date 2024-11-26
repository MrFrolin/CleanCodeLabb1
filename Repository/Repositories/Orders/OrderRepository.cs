using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Model;

namespace Repository.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(MyDbContext context, DbSet<Order> dbSet) : base(context, dbSet) { }

        public bool ChangeOrderStatus(Order order)
        {
            throw new NotImplementedException();
        }


    }
}
