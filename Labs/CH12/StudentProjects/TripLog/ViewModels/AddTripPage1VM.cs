using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TripLog.ViewModels
{
    public class AddTripPage1VM
    {
        [Required]
        [Display(Name = "Destination")]
        public int? DestinationId { get; set; }

        [Required]
        [Display(Name = "Accommodations")]
        public int? AccommodationId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public IEnumerable<SelectListItem>? Destinations { get; set; }
        public IEnumerable<SelectListItem>? Accommodations { get; set; }
    }
}
