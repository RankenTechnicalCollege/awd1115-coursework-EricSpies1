namespace PersonalFinanceTracker.Models
{
    public class TransactionCategory
    {
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
