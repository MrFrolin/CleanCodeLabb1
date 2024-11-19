using Microsoft.EntityFrameworkCore;
using Repository.Model;

namespace Repository.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Example: Configure relationships or constraints

            modelBuilder.Entity<Product>();

            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(oi => oi.Order)
            //    .WithMany(o => o.OrderItems)
            //    .HasForeignKey(oi => oi.OrderId);

            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(oi => oi.Product)
            //    .WithMany()
            //    .HasForeignKey(oi => oi.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
