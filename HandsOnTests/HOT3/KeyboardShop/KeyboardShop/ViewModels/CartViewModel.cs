using KeyboardShop.Models;

namespace KeyboardShop.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new();
        public int TotalQty => Items.Sum(i => i.Qty);
        public decimal TotalPrice => Items.Sum(i => i.LineTotal);
    }
}
