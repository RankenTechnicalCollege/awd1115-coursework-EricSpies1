using System.ComponentModel.DataAnnotations;

namespace PriceQuotation.Models
{
    public class PriceQuote
    {
        [Display(Name = "Sales price (subtotal)")]
        [Required(ErrorMessage = "Subtotal is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Subtotal must be greater than 0")]
        public decimal? Subtotal { get; set; }

        [Display(Name = "Discount percent")]
        [Required(ErrorMessage = "Discount percent is required")]
        [Range(0, 100, ErrorMessage = "Discount percent must be from 0 to 100")]
        public decimal? DiscountPercent { get; set; }

        public decimal DiscountAmount =>
            HasInputs ? Math.Round(Subtotal!.Value * (DiscountPercent!.Value / 100m), 2) : 0m;

        public decimal Total =>
            HasInputs ? Math.Round(Subtotal!.Value - DiscountAmount, 2) : 0m;

        private bool HasInputs => Subtotal.HasValue && DiscountPercent.HasValue;
    }
}
