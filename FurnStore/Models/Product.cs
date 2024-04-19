namespace FurnStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Material { get; set; } = null!;

        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string? Rentee { get; set; }

        public string? RenteeEmail { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}