namespace InventoryApi.Models
{
    public class Notification
    {
        public Guid   Id           { get; set; }
        public Guid   ProductId    { get; set; }
        public string ProductName  { get; set; } = string.Empty;
        public int    Quantity     { get; set; }
        public DateTime CreatedAt  { get; set; }
    }
}