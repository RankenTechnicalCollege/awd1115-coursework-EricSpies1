using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Data;

namespace PersonalFinanceTracker.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _db;
        public TransactionRepository(ApplicationDbContext db) => _db = db;

        public Task<Transaction?> GetAsync(int id, string userId) =>
            _db.Transactions
               .Include(t => t.Account)
               .Include(t => t.TransactionCategories).ThenInclude(tc => tc.Category)
               .FirstOrDefaultAsync(t => t.Id == id && t.Account!.UserId == userId);

        public Task<List<Transaction>> GetForMonthAsync(string userId, int year, int month) =>
            _db.Transactions
               .Include(t => t.Account)
               .Where(t => t.Account!.UserId == userId &&
                           t.Date.Year == year && t.Date.Month == month)
               .OrderByDescending(t => t.Date)
               .ToListAsync();

        public async Task AddAsync(Transaction tx, int[] categoryIds)
        {
            _db.Transactions.Add(tx);
            foreach (var cid in categoryIds.Distinct())
                _db.TransactionCategories.Add(new TransactionCategory { Transaction = tx, CategoryId = cid });

            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction tx, int[] categoryIds)
        {
            var existing = await _db.Transactions
                .Include(t => t.TransactionCategories)
                .FirstAsync(t => t.Id == tx.Id);

            _db.Entry(existing).CurrentValues.SetValues(tx);

            var target = categoryIds.Distinct().ToHashSet();

            foreach (var link in existing.TransactionCategories.Where(tc => !target.Contains(tc.CategoryId)).ToList())
                existing.TransactionCategories.Remove(link);

            foreach (var cid in target)
                if (!existing.TransactionCategories.Any(tc => tc.CategoryId == cid))
                    existing.TransactionCategories.Add(new TransactionCategory { TransactionId = existing.Id, CategoryId = cid });

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var tx = await GetAsync(id, userId);
            if (tx is null) return;
            _db.Transactions.Remove(tx);
            await _db.SaveChangesAsync();
        }
    }
}
