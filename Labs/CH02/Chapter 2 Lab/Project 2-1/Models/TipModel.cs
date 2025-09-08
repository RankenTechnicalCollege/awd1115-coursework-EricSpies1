using System.ComponentModel.DataAnnotations;

namespace PriceQuotation.Models
{
    public class TipModel
    {
        [Display(Name = "Cost of meal")]
        [Required(ErrorMessage = "Meal cost is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Meal cost must be greater than 0")]
        public decimal? MealCost { get; set; }

        public IEnumerable<(int percent, decimal amount)> GetTips()
        {
            var basis = MealCost.GetValueOrDefault(0m);
            int[] percents = new[] { 15, 18, 20 };
            foreach (var p in percents)
                yield return (p, basis == 0m ? 0m : Math.Round(basis * (p / 100m), 2));
        }
    }
}
