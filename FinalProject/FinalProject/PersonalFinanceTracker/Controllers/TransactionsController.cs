using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.ViewModels;

namespace PersonalFinanceTracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TransactionsController(ApplicationDbContext db) => _db = db;

        [HttpGet("transactions/")]
        public async Task<IActionResult> Index(int page = 1, string? search = null, int? pageSize = null)
        {
            const int defaultPageSize = 8;

            if (pageSize.HasValue && (pageSize == 5 || pageSize == 8 || pageSize == 10 || pageSize == 20))
            {
                HttpContext.Session.SetInt32("TxPageSize", pageSize.Value);
            }

            int resolvedPageSize = HttpContext.Session.GetInt32("TxPageSize") ?? defaultPageSize;

            var baseQuery = _db.Transactions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                baseQuery = baseQuery.Where(t =>
                    t.Payee.ToLower().Contains(term) ||
                    (t.Notes != null && t.Notes.ToLower().Contains(term))
                );
            }

            var totalCount = await baseQuery.CountAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)resolvedPageSize);
            if (totalPages < 1) totalPages = 1;

            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var pageIds = await baseQuery
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .Skip((page - 1) * resolvedPageSize)
                .Take(resolvedPageSize)
                .Select(t => t.Id)
                .ToListAsync();

            var txs = await _db.Transactions
                .Where(t => pageIds.Contains(t.Id))
                .Include(t => t.Account)
                .Include(t => t.TransactionCategories)
                    .ThenInclude(tc => tc.Category)
                .ToListAsync();

            var orderMap = pageIds
                .Select((id, idx) => new { id, idx })
                .ToDictionary(x => x.id, x => x.idx);

            txs = txs.OrderBy(t => orderMap[t.Id]).ToList();

            var vm = new TransactionListViewModel
            {
                Transactions = txs,
                PageNumber = page,
                TotalPages = totalPages,
                PageSize = resolvedPageSize,
                TotalCount = totalCount,
                SearchTerm = search ?? ""
            };

            return View(vm);
        }

        [HttpGet("transactions/{id:int}/{slug}/")]
        public async Task<IActionResult> Details(int id, string? slug)
        {
            var tx = await _db.Transactions
                .Include(t => t.Account)
                .Include(t => t.TransactionCategories)
                    .ThenInclude(tc => tc.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tx == null) return NotFound();

            if (!string.Equals(slug, tx.Slug, StringComparison.OrdinalIgnoreCase))
                return RedirectToAction(nameof(Details), new { id = tx.Id, slug = tx.Slug });

            return View(tx);
        }

        [HttpGet("transactions/create/")]
        public async Task<IActionResult> Create()
        {
            await PopulateLookupsAsync();
            return View();
        }

        [HttpPost("transactions/create/")]
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

        [HttpGet("transactions/edit/{id:int}/{slug}/")]
        public async Task<IActionResult> Edit(int id, string? slug)
        {
            var tx = await _db.Transactions
                .Include(t => t.TransactionCategories)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tx == null) return NotFound();

            if (!string.Equals(slug, tx.Slug, StringComparison.OrdinalIgnoreCase))
                return RedirectToAction(nameof(Edit), new { id = tx.Id, slug = tx.Slug });

            await PopulateLookupsAsync(
                selectedAccountId: tx.AccountId,
                selectedCategoryIds: tx.TransactionCategories.Select(tc => tc.CategoryId).ToArray()
            );

            return View(tx);
        }

        [HttpPost("transactions/edit/{id:int}/{slug}/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string? slug, Transaction form, int[]? categoryIds)
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

        [HttpGet("transactions/delete/{id:int}/{slug}/")]
        public async Task<IActionResult> Delete(int id, string? slug)
        {
            var tx = await _db.Transactions
                .Include(t => t.Account)
                .Include(t => t.TransactionCategories).ThenInclude(tc => tc.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tx == null) return NotFound();

            if (!string.Equals(slug, tx.Slug, StringComparison.OrdinalIgnoreCase))
                return RedirectToAction(nameof(Delete), new { id = tx.Id, slug = tx.Slug });

            return View(tx);
        }

        [HttpPost("transactions/delete/{id:int}/{slug}/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string? slug)
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
