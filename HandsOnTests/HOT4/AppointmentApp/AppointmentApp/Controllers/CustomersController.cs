using AppointmentApp.Data;
using AppointmentApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AppDbContext _db;
        public CustomersController(AppDbContext db) => _db = db;

        public IActionResult Create() => View(new Customer());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer c)
        {
            if (!ModelState.IsValid) return View(c);

            _db.Customers.Add(c);
            await _db.SaveChangesAsync();
            TempData["Message"] = $"Customer '{c.Username}' created.";
            return RedirectToAction("Index", "Appointments");
        }
    }
}
