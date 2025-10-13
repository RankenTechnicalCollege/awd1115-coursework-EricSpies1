using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.ViewModels;

namespace PersonalFinanceTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DashboardController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index(string accountId = "all", string categoryId = "all")
        {
            var vm = new DashboardViewModel
            {
                ActiveAccountId = accountId,
                ActiveCategoryId = categoryId,
                Accounts = await _db.Accounts.OrderBy(a => a.Name).ToListAsync(),
                Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync()
            };

            var q = _db.Transactions
                .Include(t => t.Account)
                .Include(t => t.TransactionCategories).ThenInclude(tc => tc.Category)
                .AsQueryable();

            if (int.TryParse(accountId, out var aId))
                q = q.Where(t => t.AccountId == aId);

            if (int.TryParse(categoryId, out var cId))
                q = q.Where(t => t.TransactionCategories.Any(tc => tc.CategoryId == cId));

            vm.RecentTransactions = await q.OrderByDescending(t => t.Date).ThenByDescending(t => t.Id).Take(20).ToListAsync();

            var first = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var month = q.Where(t => t.Date >= first);
            vm.MonthToDateSpending = await month.Where(t => t.Type == TransactionType.Expense).SumAsync(t => t.Amount);
            vm.MonthToDateDeposits = await month.Where(t => t.Type == TransactionType.Deposit).SumAsync(t => t.Amount);
            vm.TransactionCount = await month.CountAsync();

            return View(vm);
        }
    }
}
