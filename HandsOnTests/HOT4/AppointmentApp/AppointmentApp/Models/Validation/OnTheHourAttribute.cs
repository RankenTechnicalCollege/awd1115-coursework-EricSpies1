using System.ComponentModel.DataAnnotations;

namespace AppointmentApp.Models.Validation
{
    public class OnTheHourAttribute : ValidationAttribute
    {
        public OnTheHourAttribute()
        {
            ErrorMessage = "Appointments must start on the exact hour (e.g., 08:00 AM).";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is not DateTime dt) return ValidationResult.Success;
            return (dt.Minute == 0 && dt.Second == 0)
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }
    }
}
