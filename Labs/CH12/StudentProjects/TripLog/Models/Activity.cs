using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripLog.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<TripActivity> TripActivities { get; set; } = new List<TripActivity>();
    }
}
