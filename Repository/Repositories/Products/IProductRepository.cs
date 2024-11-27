using Repository.Model;

namespace Repository.Repositories.Products
{
    // Gränssnitt för produktrepositoryt enligt Repository Pattern
    public interface IProductRepository : IRepository<Product>
    {
    }
}
