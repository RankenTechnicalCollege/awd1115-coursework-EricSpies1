using System.ComponentModel.DataAnnotations;

namespace HOT1.Models
{
    public class DistanceModel
    {
        [Required(ErrorMessage = "Please enter a distance in inches.")]
        [Range(1, 500, ErrorMessage = "Distance must be between 1 and 500 inches.")]
        public double? Inches { get; set; }

        public double Centimeters => Inches.HasValue ? Inches.Value * 2.54 : 0;
    }
}
