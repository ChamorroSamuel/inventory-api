using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InventoryApi.Data;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _cfg;
    public AuthController(IConfiguration cfg, ApplicationDbContext db)
    {
        _cfg = cfg;
        _db  = db;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (await _db.Users.AnyAsync(u => u.Username == req.Username))
            return BadRequest("Usuario ya existe");

        var user = new User
        {
            Id           = Guid.NewGuid(),
            Username     = req.Username,
            PasswordHash = req.Password, 
            Role         = req.Role
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = CreateJwtToken(user.Username, user.Role);
        return Ok(new { token });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username == req.Username && u.PasswordHash == req.Password);
        if (user == null)
            return Unauthorized("Credenciales inválidas");

        var token = CreateJwtToken(user.Username, user.Role);
        return Ok(new { token });
    }

    private string CreateJwtToken(string username, string role)
    {
        var key    = Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!);
        var creds  = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}