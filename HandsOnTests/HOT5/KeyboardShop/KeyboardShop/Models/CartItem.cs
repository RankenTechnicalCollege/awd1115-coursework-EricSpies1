namespace KeyboardShop.Models
{
    public class CartItem
    {
        public int KeyboardId { get; set; }
        public string Name { get; set; } = "";
        public string ImageFile { get; set; } = "";
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal LineTotal => Price * Qty;
    }
}
