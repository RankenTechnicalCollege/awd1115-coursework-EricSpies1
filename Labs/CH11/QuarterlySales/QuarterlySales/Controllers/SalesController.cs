using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarterlySales.Data;
using QuarterlySales.Models;

namespace QuarterlySales.Controllers
{
    public class SalesController : Controller
    {
        private readonly AppDbContext _db;
        public SalesController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            await PopulateEmployeesAsync();
            return View(new SalesEntry { Year = DateTime.Today.Year, Quarter = 1 });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SalesEntry entry)
        {
            if (entry.Year <= 2000)
                ModelState.AddModelError(nameof(SalesEntry.Year), "Year must be after the year 2000.");

            if (entry.Quarter is < 1 or > 4)
                ModelState.AddModelError(nameof(SalesEntry.Quarter), "Quarter must be between 1 and 4.");

            if (entry.Amount <= 0)
                ModelState.AddModelError(nameof(SalesEntry.Amount), "Amount must be greater than 0.");

            bool exists = await _db.Sales.AnyAsync(s =>
                s.EmployeeId == entry.EmployeeId && s.Year == entry.Year && s.Quarter == entry.Quarter);

            if (exists)
            {
                var emp = await _db.Employees.FindAsync(entry.EmployeeId);
                ModelState.AddModelError(string.Empty,
                    $"Sales for {emp?.FullName ?? "employee"} for {entry.Year} Q{entry.Quarter} are already in the database.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateEmployeesAsync(entry.EmployeeId);
                return View(entry);
            }

            _db.Sales.Add(entry);
            await _db.SaveChangesAsync();
            TempData["Message"] = $"Added sales for {entry.Year} Q{entry.Quarter}.";
            return RedirectToAction("Index", "Home", new { id = entry.EmployeeId });
        }

        private async Task PopulateEmployeesAsync(int? selected = null)
        {
            var emps = await _db.Employees
                .OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToListAsync();
            ViewBag.EmployeeId = new SelectList(emps, "Id", "FullName", selected);
        }
    }
}
