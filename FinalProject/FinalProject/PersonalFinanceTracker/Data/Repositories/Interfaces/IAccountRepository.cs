namespace PersonalFinanceTracker.Data.Repositories
{
    using PersonalFinanceTracker.Models;

    public interface IAccountRepository
    {
        Task<List<Account>> GetForUserAsync(string userId);
        Task<Account?> GetAsync(int id, string userId);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(int id, string userId);
    }
}
