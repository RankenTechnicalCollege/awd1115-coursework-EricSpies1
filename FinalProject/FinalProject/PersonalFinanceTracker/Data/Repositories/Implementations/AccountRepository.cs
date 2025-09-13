using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Data;

namespace PersonalFinanceTracker.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;
        public AccountRepository(ApplicationDbContext db) => _db = db;

        public Task<List<Account>> GetForUserAsync(string userId) =>
            _db.Accounts.Where(a => a.UserId == userId)
                        .OrderBy(a => a.Name)
                        .ToListAsync();

        public Task<Account?> GetAsync(int id, string userId) =>
            _db.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        public async Task AddAsync(Account account)
        {
            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var a = await GetAsync(id, userId);
            if (a is null) return;
            _db.Accounts.Remove(a);
            await _db.SaveChangesAsync();
        }
    }
}
