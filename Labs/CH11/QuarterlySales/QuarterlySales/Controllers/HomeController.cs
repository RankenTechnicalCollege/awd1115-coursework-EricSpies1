using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarterlySales.Data;

namespace QuarterlySales.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index(int? id)
        {
            var employees = await _db.Employees
                .OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToListAsync();
            ViewBag.EmployeeId = new SelectList(employees, "Id", "FullName", id);

            var query = _db.Sales.Include(s => s.Employee).AsQueryable();
            if (id.HasValue && id.Value > 0)
                query = query.Where(s => s.EmployeeId == id.Value);

            var rows = await query
                .OrderByDescending(s => s.Year)
                .ThenBy(s => s.Quarter)
                .ToListAsync();

            ViewBag.Total = rows.Sum(r => r.Amount);
            ViewBag.SelectedId = id ?? 0;
            return View(rows);
        }
    }
}
