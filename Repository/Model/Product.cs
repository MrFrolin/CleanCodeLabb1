namespace Repository.Model
{
    // Produktmodellen representerar en produkt i webbshoppen
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }

        //// Example of navigation property
        //public Category Category { get; set; }
    }
}