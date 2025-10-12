using System.ComponentModel.DataAnnotations;

namespace TripLog.ViewModels
{
    public class AddTripPage2VM
    {
        [Phone]
        public string? AccommodationPhone { get; set; }

        [EmailAddress]
        public string? AccommodationEmail { get; set; }
    }
}
