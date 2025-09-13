namespace PersonalFinanceTracker.Data.Repositories
{
    using PersonalFinanceTracker.Models;

    public interface ITransactionRepository
    {
        Task<Transaction?> GetAsync(int id, string userId);
        Task<List<Transaction>> GetForMonthAsync(string userId, int year, int month);
        Task AddAsync(Transaction tx, int[] categoryIds);
        Task UpdateAsync(Transaction tx, int[] categoryIds);
        Task DeleteAsync(int id, string userId);
    }
}
