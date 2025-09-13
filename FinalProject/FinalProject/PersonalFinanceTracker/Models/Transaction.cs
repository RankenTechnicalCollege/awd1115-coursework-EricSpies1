using System.ComponentModel.DataAnnotations;
using PersonalFinanceTracker.Validation;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        [Required, DateNotInFuture]
        public DateTime Date { get; set; }

        [Required, PositiveAmount]
        public decimal Amount { get; set; }

        [Required, StringLength(120)]
        public string Payee { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Notes { get; set; }

        [Required]
        public TransactionType Type { get; set; } = TransactionType.Expense;

        public ICollection<TransactionCategory> TransactionCategories { get; set; } = new List<TransactionCategory>();
    }

    public enum TransactionType { Expense = 0, Deposit = 1 }
}
