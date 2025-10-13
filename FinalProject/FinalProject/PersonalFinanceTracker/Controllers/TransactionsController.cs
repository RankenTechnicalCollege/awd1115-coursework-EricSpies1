using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TransactionsController(ApplicationDbContext db) => _db = db;

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
                return RedirectToAction(nameof(Details), new { id = tx.Id, slug = tx.Slug });
            }

            return View(tx);
        }

        [HttpGet("transactions/create")]
        public async Task<IActionResult> Create()
        {
            await PopulateLookupsAsync();
            return View();
        }

        [HttpPost("transactions/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaction tx, int[]? categoryIds)
        {
            if (!ModelState.IsValid)
            {
                await PopulateLookupsAsync(selectedAccountId: tx.AccountId, selectedCategoryIds: categoryIds);
                return View(tx);
            }

            if (categoryIds is { Length: > 0 })
            {
                tx.TransactionCategories ??= new List<TransactionCategory>();
                foreach (var cid in categoryIds.Distinct())
                    tx.TransactionCategories.Add(new TransactionCategory { CategoryId = cid, Transaction = tx });
            }

            _db.Transactions.Add(tx);
            await _db.SaveChangesAsync();

            TempData["Message"] = $"Transaction \"{tx.Payee}\" was added.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("transactions/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var tx = await _db.Transactions
                .Include(t => t.TransactionCategories)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tx == null) return NotFound();

            await PopulateLookupsAsync(
                selectedAccountId: tx.AccountId,
                selectedCategoryIds: tx.TransactionCategories.Select(tc => tc.CategoryId).ToArray()
            );

            return View(tx);
        }

        [HttpPost("transactions/edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Transaction form, int[]? categoryIds)
        {
            if (id != form.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                await PopulateLookupsAsync(form.AccountId, categoryIds);
                return View(form);
            }

            var tx = await _db.Transactions
                .Include(t => t.TransactionCategories)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tx == null) return NotFound();

            tx.Date = form.Date;
            tx.Amount = form.Amount;
            tx.AccountId = form.AccountId;
            tx.Payee = form.Payee;
            tx.Notes = form.Notes;
            tx.Type = form.Type;

            tx.TransactionCategories.Clear();
            if (categoryIds is { Length: > 0 })
            {
                foreach (var cid in categoryIds.Distinct())
                    tx.TransactionCategories.Add(new TransactionCategory { TransactionId = tx.Id, CategoryId = cid });
            }

            await _db.SaveChangesAsync();
            TempData["Message"] = $"Transaction \"{tx.Payee}\" was updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("transactions/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tx = await _db.Transactions
                .Include(t => t.Account)
                .Include(t => t.TransactionCategories).ThenInclude(tc => tc.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tx == null) return NotFound();
            return View(tx);
        }

        [HttpPost("transactions/delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tx = await _db.Transactions.FindAsync(id);
            if (tx != null)
            {
                _db.Transactions.Remove(tx);
                await _db.SaveChangesAsync();
                TempData["Message"] = $"Transaction \"{tx.Payee}\" was deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateLookupsAsync(int? selectedAccountId = null, int[]? selectedCategoryIds = null)
        {
            var accounts = await _db.Accounts.OrderBy(a => a.Name).ToListAsync();
            var categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();

            ViewBag.AccountId = new SelectList(accounts, "Id", "Name", selectedAccountId);
            ViewBag.Categories = new MultiSelectList(categories, "Id", "Name", selectedCategoryIds);
        }
    }
}
