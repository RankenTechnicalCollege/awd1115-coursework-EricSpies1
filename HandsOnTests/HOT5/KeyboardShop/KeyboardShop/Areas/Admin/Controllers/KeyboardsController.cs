using KeyboardShop.Data;
using KeyboardShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeyboardShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KeyboardsController : Controller
    {
        private readonly AppDbContext _db;
        public KeyboardsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var list = await _db.Keyboards
                .OrderBy(k => k.Brand).ThenBy(k => k.Name)
                .ToListAsync();
            return View(list);
        }

        public IActionResult Create() => View(new Keyboard());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Keyboard k)
        {
            if (!ModelState.IsValid) return View(k);

            k.SetSlug();
            _db.Keyboards.Add(k);
            await _db.SaveChangesAsync();

            TempData["Message"] = $"“{k.Name}” created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var k = await _db.Keyboards.FindAsync(id);
            return k == null ? NotFound() : View(k);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Keyboard form)
        {
            if (id != form.Id) return NotFound();
            if (!ModelState.IsValid) return View(form);

            var k = await _db.Keyboards.FindAsync(id);
            if (k == null) return NotFound();

            k.Name = form.Name;
            k.Brand = form.Brand;
            k.SwitchType = form.SwitchType;
            k.Layout = form.Layout;
            k.Connectivity = form.Connectivity;
            k.Price = form.Price;
            k.ImageFile = form.ImageFile;
            k.SetSlug();

            await _db.SaveChangesAsync();
            TempData["Message"] = $"“{k.Name}” updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var k = await _db.Keyboards.FindAsync(id);
            return k == null ? NotFound() : View(k);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var k = await _db.Keyboards.FindAsync(id);
            if (k != null)
            {
                _db.Keyboards.Remove(k);
                await _db.SaveChangesAsync();
                TempData["Message"] = $"“{k.Name}” deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var k = await _db.Keyboards.FindAsync(id);
            return k == null ? NotFound() : View(k);
        }
    }
}
