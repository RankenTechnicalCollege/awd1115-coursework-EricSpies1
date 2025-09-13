using System.ComponentModel.DataAnnotations;

namespace HOT1.Models
{
    public class OrderModel
    {
        [Required(ErrorMessage = "Please enter a quantity.")]
        [Range(1, 100, ErrorMessage = "Quantity must be at least 1 and no more than 100.")]
        public int? Quantity { get; set; }

        public string? DiscountCode { get; set; }

        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public void Calculate()
        {
            const decimal price = 15m;
            const decimal taxRate = 0.08m;

            if (Quantity is null) return;

            Subtotal = Quantity.Value * price;

            decimal discountPercent = DiscountCode?.ToUpper() switch
            {
                "6175" => 0.30m,
                "1390" => 0.20m,
                "BB88" => 0.10m,
                _ => 0m
            };

            DiscountAmount = Subtotal * discountPercent;
            var discounted = Subtotal - DiscountAmount;
            Tax = discounted * taxRate;
            Total = discounted + Tax;
        }
    }
}
