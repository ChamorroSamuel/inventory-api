using InventoryApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public ReportsController(ApplicationDbContext db) => _db = db;

    [HttpGet("low-inventory")]
    public async Task<IActionResult> LowInventoryPdf()
    {
        var items = await _db.Products
                             .Where(p => p.Quantity < 5)
                             .OrderBy(p => p.Name)
                             .ToListAsync();

        using var doc = new PdfDocument();
        var page = doc.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Verdana", 14, XFontStyle.Bold);

        // Título
        gfx.DrawString("Reporte de Inventario Bajo", font, XBrushes.Black,
                       new XRect(0, 20, page.Width, 40), XStringFormats.TopCenter);

        // Fecha
        gfx.DrawString(DateTime.UtcNow
                       .ToString("dddd, dd MMMM yyyy HH:mm", new CultureInfo("es-ES")),
                       new XFont("Verdana", 10), XBrushes.Gray,
                       new XRect(40, 60, page.Width, 20), XStringFormats.TopLeft);

        // Encabezados de tabla
        var y = 100.0;
        var headerFont = new XFont("Verdana", 10, XFontStyle.Bold);
        gfx.DrawString("Nombre", headerFont, XBrushes.Black, 40, y);
        gfx.DrawString("Categoría", headerFont, XBrushes.Black, 240, y);
        gfx.DrawString("Cantidad", headerFont, XBrushes.Black, 420, y);

        // Filas
        var rowFont = new XFont("Verdana", 10);
        y += 20;
        foreach (var p in items)
        {
            gfx.DrawString(p.Name,      rowFont, XBrushes.Black, 40,  y);
            gfx.DrawString(p.Category, rowFont, XBrushes.Black, 240, y);
            gfx.DrawString(p.Quantity.ToString(), rowFont, XBrushes.Black, 420, y);
            y += 20;
            if (y > page.Height - 40)
            {
                page = doc.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                y = 40;
            }
        }

        using var ms = new MemoryStream();
        doc.Save(ms);
        ms.Position = 0;
        return File(ms.ToArray(), "application/pdf",
                    "low-inventory.pdf");
    }
}