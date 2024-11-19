using Repository.Repositories;
using Repository.Model;

namespace Repository
{
    // Gränssnitt för orderrepositoryt enligt Repository Pattern
    public interface IOrderRepository : IRepository<Order>
    {
        bool ChangeOrderStatus(Order order);
    }
}
