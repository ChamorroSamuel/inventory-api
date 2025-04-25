using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Models
{
    public class User
    {
        [Key]
        public Guid   Id           { get; set; }
        [Required]
        public string Username     { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;
        [Required]
        public string Role         { get; set; } = null!;
    }
}