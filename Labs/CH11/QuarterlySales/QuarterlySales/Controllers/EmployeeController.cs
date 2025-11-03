using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarterlySales.Data;
using QuarterlySales.Models;

namespace QuarterlySales.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;
        public EmployeeController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            await PopulateManagersAsync();
            return View(new Employee { DateOfHire = DateTime.Today, DOB = DateTime.Today.AddYears(-25) });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Employee e)
        {
            ValidateEmployeeRules(e);

            bool duplicate = await _db.Employees
                .AnyAsync(x => x.FirstName == e.FirstName && x.LastName == e.LastName && x.DOB == e.DOB);
            if (duplicate)
            {
                ModelState.AddModelError(string.Empty, $"{e.FullName} (DOB: {e.DOB:d}) is already in the database.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateManagersAsync(e.ManagerId);
                return View(e);
            }

            _db.Employees.Add(e);
            await _db.SaveChangesAsync();
            TempData["Message"] = $"Employee {e.FullName} added.";
            return RedirectToAction("Index", "Home");
        }

        private void ValidateEmployeeRules(Employee e)
        {
            var founded = new DateTime(1995, 1, 1);

            if (e.DOB >= DateTime.Today)
                ModelState.AddModelError(nameof(Employee.DOB), "Birth date must be a valid date in the past.");

            if (e.DateOfHire >= DateTime.Today)
                ModelState.AddModelError(nameof(Employee.DateOfHire), "Hire date must be in the past.");

            if (e.DateOfHire < founded)
                ModelState.AddModelError(nameof(Employee.DateOfHire), "Date of hire must not be before 1/1/1995.");

            if (e.ManagerId.HasValue && e.ManagerId.Value == e.Id)
                ModelState.AddModelError(nameof(Employee.ManagerId), "Manager and employee can't be the same person.");
        }

        private async Task PopulateManagersAsync(int? selected = null)
        {
            var mgrs = await _db.Employees
                .OrderBy(m => m.LastName).ThenBy(m => m.FirstName).ToListAsync();

            ViewBag.ManagerId = new SelectList(mgrs, "Id", "FullName", selected);
        }
    }
}
