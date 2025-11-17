using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripLog.Models
{
    public class Trip
    {
        public int TripId { get; set; }

        [Required]
        public int DestinationId { get; set; }

        [Required]
        public int AccommodationId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public Destination? Destination { get; set; }
        public Accommodation? Accommodation { get; set; }

        public ICollection<TripActivity> TripActivities { get; set; } = new List<TripActivity>();
    }
}
