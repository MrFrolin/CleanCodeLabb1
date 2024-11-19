using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Model;

namespace Repository.Repositories.Products
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDbContext context, DbSet<Product> dbSet) : base(context, dbSet) { }
        public bool UpdateProductStock(Product product, int quantity)
        {
            throw new NotImplementedException();
        }

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            Add(product);
        }

    }
}
