using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) => _db = db;

        public Task<List<Category>> GetAllAsync() =>
            _db.Categories.OrderBy(c => c.Name).ToListAsync();

        public async Task AddAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var c = await _db.Categories.FindAsync(id);
            if (c is null) return;
            _db.Categories.Remove(c);
            await _db.SaveChangesAsync();
        }
    }
}
