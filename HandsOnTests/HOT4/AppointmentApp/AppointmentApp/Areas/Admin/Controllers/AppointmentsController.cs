using AppointmentApp.Data;
using AppointmentApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _db;
        public AppointmentsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var items = await _db.Appointments.Include(a => a.Customer)
                .OrderBy(a => a.Start).ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var appt = await _db.Appointments.FindAsync(id);
            if (appt == null) return NotFound();

            await PopulateCustomersAsync(appt.CustomerId);
            return View(appt);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment form)
        {
            if (id != form.Id) return NotFound();

            await ValidateSlotAvailability(form);
            if (!ModelState.IsValid)
            {
                await PopulateCustomersAsync(form.CustomerId);
                return View(form);
            }

            var appt = await _db.Appointments.FindAsync(id);
            if (appt == null) return NotFound();

            appt.Start = form.Start;
            appt.CustomerId = form.CustomerId;

            await _db.SaveChangesAsync();
            TempData["Message"] = $"Appointment for {appt.Start:g} updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var appt = await _db.Appointments.Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.Id == id);
            return appt == null ? NotFound() : View(appt);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appt = await _db.Appointments.FindAsync(id);
            if (appt != null)
            {
                _db.Appointments.Remove(appt);
                await _db.SaveChangesAsync();
                TempData["Message"] = "Appointment deleted.";
            }
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
                if (appt.Start <= DateTime.Now)
                {
                    ModelState.AddModelError(nameof(Appointment.Start),
                        "The appointment time must be in the future.");
                }

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
