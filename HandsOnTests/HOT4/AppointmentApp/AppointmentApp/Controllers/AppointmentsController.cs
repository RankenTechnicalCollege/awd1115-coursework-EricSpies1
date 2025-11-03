using AppointmentApp.Data;
using AppointmentApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _db;
        public AppointmentsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var items = await _db.Appointments
                .Include(a => a.Customer)
                .OrderBy(a => a.Start)
                .ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateCustomersAsync();
            return View(new Appointment { Start = DateTime.Now.AddHours(1).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second) });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appt)
        {
            await ValidateSlotAvailability(appt);

            if (!ModelState.IsValid)
            {
                await PopulateCustomersAsync(appt.CustomerId);
                return View(appt);
            }

            _db.Appointments.Add(appt);
            await _db.SaveChangesAsync();
            TempData["Message"] = $"Appointment scheduled for {appt.Start:g}.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateCustomersAsync(int? selectedId = null)
        {
            var customers = await _db.Customers
                .OrderBy(c => c.Username)
                .ToListAsync();

            ViewBag.CustomerId = new SelectList(customers, "Id", "Username", selectedId);
        }

        private async Task ValidateSlotAvailability(Appointment appt)
        {
            if (appt.Start != default)
            {
                bool exists = await _db.Appointments.AnyAsync(a => a.Start == appt.Start && a.Id != appt.Id);
                if (exists)
                {
                    ModelState.AddModelError(nameof(Appointment.Start),
                        $"The {appt.Start:g} time slot is already taken. Please choose a different hour.");
                }
            }
        }
    }
}
