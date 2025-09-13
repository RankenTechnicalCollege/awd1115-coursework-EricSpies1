using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Validation
{
    public class PositiveAmountAttribute : ValidationAttribute
    {
        public PositiveAmountAttribute() : base("Amount must be greater than 0.") { }
        public override bool IsValid(object? value)
        {
            if (value is not decimal d) return true;
            return d > 0m;
        }
    }
}
