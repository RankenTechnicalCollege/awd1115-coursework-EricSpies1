using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactManager.Data;
using ContactManager.Models;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly AppDbContext _db;
        public ContactsController(AppDbContext db) => _db = db;

        private async Task PopulateCategoriesAsync(int? selectedId = null)
        {
            var items = await _db.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                .ToListAsync();
            ViewBag.Categories = new SelectList(items, "Value", "Text", selectedId);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            await PopulateCategoriesAsync();
            return View("Upsert", new Contact());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Contact model)
        {
            if (ModelState.IsValid)
            {
                model.DateAdded = DateTime.UtcNow;
                _db.Contacts.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("Details", "Home", new { id = model.ContactId, slug = model.Slug });
            }
            await PopulateCategoriesAsync(model.CategoryId);
            return View("Upsert", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _db.Contacts.FindAsync(id);
            if (contact == null) return NotFound();

            await PopulateCategoriesAsync(contact.CategoryId);
            return View("Upsert", contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Contact model)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(model).Property(m => m.DateAdded).IsModified = false;

                _db.Update(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("Details", "Home", new { id = model.ContactId, slug = model.Slug });
            }
            await PopulateCategoriesAsync(model.CategoryId);
            return View("Upsert", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _db.Contacts
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.ContactId == id);
            if (contact == null) return NotFound();

            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _db.Contacts.FindAsync(id);
            if (contact != null)
            {
                _db.Contacts.Remove(contact);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult CancelAdd() => RedirectToAction("Index", "Home");

        [HttpPost]
        public IActionResult CancelEdit(int id, string? slug) =>
            RedirectToAction("Details", "Home", new { id, slug });
    }
}
