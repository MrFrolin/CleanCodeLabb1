using System.Text.Json.Serialization;

namespace Repository.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}