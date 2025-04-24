﻿namespace InventoryApi.Models
{
    public class UpdateProductDto
    {
        public string Name         { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price       { get; set; }
        public int    Quantity     { get; set; }
        public string Category     { get; set; } = string.Empty;
    }
}