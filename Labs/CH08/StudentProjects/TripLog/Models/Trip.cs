using System;
using System.ComponentModel.DataAnnotations;

namespace TripLog.Models
{
    public class Trip
    {
        public int TripId { get; set; }

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public string Accommodation { get; set; } = string.Empty;

        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Phone]
        public string? AccommodationPhone { get; set; }

        [EmailAddress]
        public string? AccommodationEmail { get; set; }

        public string? Activity1 { get; set; }
        public string? Activity2 { get; set; }
        public string? Activity3 { get; set; }
    }
}
