namespace PersonalFinanceTracker.Data.Repositories
{
    using PersonalFinanceTracker.Models;

    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task DeleteAsync(int id);
    }
}
