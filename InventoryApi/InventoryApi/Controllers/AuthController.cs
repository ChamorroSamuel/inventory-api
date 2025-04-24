using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController, Route("api")]
public class AuthController : ControllerBase
{
    private static readonly List<(string Username, string Password, string Role)> Users
        = new()
        {
            ("admin","admin123","Administrador"),
            ("empleado","empleado123","Empleado")
        };

    private readonly IConfiguration _cfg;
    public AuthController(IConfiguration cfg) => _cfg = cfg;

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        var u = Users.FirstOrDefault(x =>
            x.Username == req.Username && x.Password == req.Password);
        if (u == default)
            return Unauthorized("Credenciales inválidas");

        var key    = Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!);
        var creds  = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256);
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
    public IActionResult Register([FromBody] RegisterRequest req)
    {
        if (Users.Any(x => x.Username == req.Username))
            return BadRequest("Usuario ya existe");
        
        Users.Add((req.Username, req.Password, req.Role));
        
        var key    = Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!);
        var creds  = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, req.Username),
            new Claim(ClaimTypes.Role, req.Role)
        };
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
        return Ok(new { token = jwt });
    }
}