using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
