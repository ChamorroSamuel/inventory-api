namespace InventoryApi.Models
{
    public record RegisterRequest(string Username, string Password, string Role);
}