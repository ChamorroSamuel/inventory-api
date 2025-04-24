using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ProductsController(ApplicationDbContext db) => _db = db;

        // —— GET /api/products  (cualquiera autenticado)
        [HttpGet]
        [Authorize] 
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
            => Ok(await _db.Products.ToListAsync());

        // —— GET /api/products/{id}  (cualquiera autenticado)
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        // —— POST /api/products  (SOLO Administrador)
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<Product>> Create([FromBody] CreateProductDto dto)
        {
            var p = new Product {
                Id          = Guid.NewGuid(),
                Name        = dto.Name,
                Description = dto.Description,
                Price       = dto.Price,
                Quantity    = dto.Quantity,
                Category    = dto.Category
            };
            _db.Products.Add(p);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
        }

        // —— PUT /api/products/{id}  (SOLO Administrador)
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name        = dto.Name;
            product.Description = dto.Description;
            product.Price       = dto.Price;
            product.Quantity    = dto.Quantity;
            product.Category    = dto.Category;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // —— DELETE /api/products/{id}  (SOLO Administrador)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            _db.Products.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}