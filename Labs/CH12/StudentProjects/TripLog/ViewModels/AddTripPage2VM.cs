using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TripLog.ViewModels
{
    public class AddTripPage2VM
    {
        public int TripId { get; set; }

        [Display(Name = "Things To Do")]
        public List<int> SelectedActivityIds { get; set; } = new();

        public IEnumerable<SelectListItem>? Activities { get; set; }
    }
}
