using KeyboardShop.Data;
using KeyboardShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeyboardShop.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext _db;
        public StoreController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index(string brand = "all", string layout = "all")
        {
            ViewBag.Brand = brand;
            ViewBag.Layout = layout;

            var query = _db.Keyboards.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand) && brand != "all")
                query = query.Where(k => k.Brand == brand);

            if (!string.IsNullOrWhiteSpace(layout) && layout != "all")
                query = query.Where(k => k.Layout == layout);

            var model = await query.OrderBy(k => k.Brand).ThenBy(k => k.Name).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(string slug)
        {
            var kbd = await _db.Keyboards.FirstOrDefaultAsync(k => k.Slug == slug);
            if (kbd == null) return NotFound();
            return View(kbd);
        }
    }
}
