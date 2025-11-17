namespace TripLog.Models
{
    public class TripActivity
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        public int ActivityId { get; set; }
        public TripLog.Models.Activity Activity { get; set; } = null!;
    }
}
