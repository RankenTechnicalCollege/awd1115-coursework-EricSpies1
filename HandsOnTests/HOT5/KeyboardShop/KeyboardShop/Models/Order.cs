using System.ComponentModel.DataAnnotations;

namespace KeyboardShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;

        [Required, StringLength(80)]
        public string CustomerName { get; set; } = "";

        [Required, EmailAddress, StringLength(120)]
        public string Email { get; set; } = "";

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal Total => Items.Sum(i => i.LineTotal);
    }

    public class OrderItem
    {
        public int Id { get; set; }

        public int KeyboardId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal LineTotal => Price * Qty;

        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
