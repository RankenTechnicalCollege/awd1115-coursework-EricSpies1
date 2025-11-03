using System.ComponentModel.DataAnnotations;
using AppointmentApp.Models.Validation;

namespace AppointmentApp.Models
{
    public class Appointment : IValidatableObject
    {
        public int Id { get; set; }

        [Required, Display(Name = "Start Date/Time")]
        [OnTheHour]
        public DateTime Start { get; set; }

        [Required, Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Start <= DateTime.Now)
            {
                yield return new ValidationResult(
                    "The appointment time must be in the future.",
                    new[] { nameof(Start) });
            }
        }
    }
}
