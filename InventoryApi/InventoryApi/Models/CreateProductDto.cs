namespace InventoryApi.Models
{
    public class CreateProductDto
    {
        public string Name         { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price       { get; set; }
        public int    Quantity     { get; set; }
        public string Category     { get; set; } = null!;
    }
}