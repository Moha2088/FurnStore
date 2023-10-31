namespace FurnStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Material { get; set; }

        public decimal Price {  get; set; }
        
        public string? Rentee { get; set; }
    }
}