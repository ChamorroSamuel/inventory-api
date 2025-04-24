using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryApi.Controllers
{
    [ApiController, Route("api")]
    public class AuthController : ControllerBase
    {
        // Mock de usuarios en memoria
        private static readonly List<(string Username, string Password, string Role)> Users
            = new()
        {
            ("admin","admin123","Administrador"),
            ("empleado","empleado123","Empleado")
        };

        private readonly IConfiguration _cfg;
        public AuthController(IConfiguration cfg) => _cfg = cfg;

        public record AuthRequest(string Username, string Password, string? Role);

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequest req)
        {
            var u = Users.FirstOrDefault(x =>
                x.Username == req.Username && x.Password == req.Password);
            if (u == default) return Unauthorized("Credenciales inválidas");

            var key   = Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, u.Username),
                new Claim(ClaimTypes.Role, u.Role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthRequest req)
        {
            if (Users.Any(x => x.Username == req.Username))
                return BadRequest("Usuario ya existe");
            Users.Add((req.Username, req.Password, req.Role!));
            // devolvemos token mock (puedes repetir lógica de login aquí)
            return Ok(new { token = "mock-token" });
        }
    }
}
