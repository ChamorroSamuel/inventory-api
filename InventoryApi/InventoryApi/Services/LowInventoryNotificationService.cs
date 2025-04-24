using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services
{
    public class LowInventoryNotificationService : BackgroundService
    {
        private readonly IServiceProvider _prov;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public LowInventoryNotificationService(IServiceProvider prov)
        {
            _prov = prov;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _prov.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // cantidad < 5
                var low = await db.Products
                    .Where(p => p.Quantity < 5)
                    .ToListAsync(stoppingToken);

                foreach (var p in low)
                {
                    // notificar 1 vez por producto
                    var exists = await db.Notifications
                        .AnyAsync(n => n.ProductId == p.Id, stoppingToken);
                    if (exists) continue;

                    db.Notifications.Add(new Notification {
                        Id          = Guid.NewGuid(),
                        ProductId   = p.Id,
                        ProductName = p.Name,
                        Quantity    = p.Quantity,
                        CreatedAt   = DateTime.UtcNow
                    });
                }

                await db.SaveChangesAsync(stoppingToken);
                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}