using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TransactionsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("transactions/")]
        public async Task<IActionResult> Index()
        {
            var txs = await _db.Transactions
                .Include(t => t.Account)
                .Include(t => t.TransactionCategories)
                    .ThenInclude(tc => tc.Category)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .ToListAsync();

            return View(txs);
        }

        public async Task<IActionResult> Details(int id, string? slug)
        {
            var tx = await _db.Transactions
                .Include(t => t.Account)
                .Include(t => t.TransactionCategories)
                    .ThenInclude(tc => tc.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tx == null) return NotFound();

            if (!string.Equals(slug, tx.Slug, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToRoute("transaction_friendly", new { id = tx.Id, slug = tx.Slug });
            }

            return View(tx);
        }
    }
}
