using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactManager.Data;

namespace ContactManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var contacts = await _db.Contacts
                .Include(c => c.Category)
                .OrderBy(c => c.Lastname).ThenBy(c => c.Firstname)
                .ToListAsync();
            return View(contacts);
        }

        public async Task<IActionResult> Details(int id, string? slug)
        {
            var contact = await _db.Contacts
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.ContactId == id);

            if (contact == null) return NotFound();

            if (slug != contact.Slug)
            {
                return RedirectToAction(nameof(Details), new { id, slug = contact.Slug });
            }

            return View(contact);
        }
    }
}
