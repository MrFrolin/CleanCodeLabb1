using System.Text.Json.Serialization;

namespace Repository.Model
{
    // Produktmodellen representerar en produkt i webbshoppen
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Order> Orders { get; set; } = new();

        //// Example of navigation property
        //public Category Category { get; set; }
    }
}