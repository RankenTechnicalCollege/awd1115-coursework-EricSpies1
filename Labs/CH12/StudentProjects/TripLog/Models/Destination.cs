using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripLog.Models
{
    public class Destination
    {
        public int DestinationId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
