using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(60)]
        public string Name { get; set; } = string.Empty;

        public ICollection<TransactionCategory> TransactionCategories { get; set; } = new List<TransactionCategory>();
    }
}
