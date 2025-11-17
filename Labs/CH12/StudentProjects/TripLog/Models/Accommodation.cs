using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripLog.Models
{
    public class Accommodation
    {
        public int AccommodationId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
