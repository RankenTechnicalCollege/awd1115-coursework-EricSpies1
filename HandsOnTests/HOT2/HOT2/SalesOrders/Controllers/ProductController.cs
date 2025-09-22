using SalesOrders.Data;
using SalesOrders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SalesOrders.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext db) => _db = db;

        public async Task<IActionResult> List()
        {
            var products = await _db.Products
                .Include(p => p.Category)
                .OrderBy(p => p.ProductName)
                .ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int? id)
        {
            ViewBag.Categories = await _db.Categories.OrderBy(c => c.CategoryName).ToListAsync();

            if (id == null) return View(new Product());

            var p = await _db.Products.FindAsync(id.Value);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(Product model)
        {
            ViewBag.Categories = await _db.Categories.OrderBy(c => c.CategoryName).ToListAsync();

            if (!ModelState.IsValid) return View(model);

            if (model.ProductID == 0)
                _db.Products.Add(model);
            else
                _db.Products.Update(model);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = model.ProductID, slug = model.Slug });
        }

        public async Task<IActionResult> Details(int id, string? slug)
        {
            var p = await _db.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.ProductID == id);
            if (p == null) return NotFound();

            if (slug != p.Slug)
                return RedirectToAction(nameof(Details), new { id, slug = p.Slug });

            return View(p);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Products.Include(c => c.Category).FirstOrDefaultAsync(x => x.ProductID == id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p != null) _db.Products.Remove(p);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
    }
}
