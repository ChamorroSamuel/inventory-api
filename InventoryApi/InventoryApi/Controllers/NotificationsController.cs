using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InventoryApi.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize(Roles="Administrador")]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public NotificationsController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAll()
        {
            var list = await _db.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return Ok(list);
        }
    }
}