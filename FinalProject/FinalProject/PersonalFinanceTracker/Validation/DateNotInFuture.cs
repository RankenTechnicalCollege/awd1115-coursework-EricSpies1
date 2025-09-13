using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Validation
{
    public class DateNotInFutureAttribute : ValidationAttribute
    {
        public DateNotInFutureAttribute() : base("Date cannot be in the future.") { }

        public override bool IsValid(object? value)
        {
            if (value is not DateTime dt) return true;
            return dt.Date <= DateTime.Today;
        }
    }
}
