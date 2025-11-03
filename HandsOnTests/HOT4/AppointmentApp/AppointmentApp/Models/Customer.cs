using System.ComponentModel.DataAnnotations;

namespace AppointmentApp.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required, StringLength(40)]
        public string Username { get; set; } = string.Empty;

        [Required, RegularExpression(@"^(\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4})$",
            ErrorMessage = "Enter a valid phone number (e.g., 555-123-4567).")]
        public string Phone { get; set; } = string.Empty;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
