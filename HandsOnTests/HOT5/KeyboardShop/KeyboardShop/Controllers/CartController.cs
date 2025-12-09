using System.Text.Json;
using KeyboardShop.Data;
using KeyboardShop.Models;
using KeyboardShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeyboardShop.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private const string CartKey = "cart";

        public CartController(AppDbContext db) => _db = db;

        private List<CartItem> GetCart()
        {
            if (HttpContext.Session.TryGetValue(CartKey, out var bytes))
            {
                var json = System.Text.Encoding.UTF8.GetString(bytes);
                return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new();
            }
            return new List<CartItem>();
        }

        private void SaveCart(List<CartItem> items)
        {
            var json = JsonSerializer.Serialize(items);
            HttpContext.Session.Set(CartKey, System.Text.Encoding.UTF8.GetBytes(json));
        }

        public IActionResult Index() => View(new CartViewModel { Items = GetCart() });

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(int id, int qty = 1)
        {
            var k = await _db.Keyboards.FirstOrDefaultAsync(x => x.Id == id);
            if (k == null) return NotFound();

            var cart = GetCart();
            var line = cart.FirstOrDefault(i => i.KeyboardId == id);
            if (line == null)
                cart.Add(new CartItem { KeyboardId = id, Name = k.Name, ImageFile = k.ImageFile, Price = k.Price, Qty = Math.Max(1, qty) });
            else
                line.Qty += Math.Max(1, qty);

            SaveCart(cart);
            TempData["Message"] = $"{k.Name} added to cart.";
            return RedirectToAction("Index", "Store");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var line = cart.FirstOrDefault(i => i.KeyboardId == id);
            if (line != null)
            {
                cart.Remove(line);
                SaveCart(cart);
                TempData["Message"] = "Item removed.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Checkout() => View(new Order());

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                ModelState.AddModelError("", "Your cart is empty.");
                return View(order);
            }
            if (!ModelState.IsValid) return View(order);

            foreach (var c in cart)
            {
                order.Items.Add(new OrderItem
                {
                    KeyboardId = c.KeyboardId,
                    Name = c.Name,
                    Price = c.Price,
                    Qty = c.Qty
                });
            }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            HttpContext.Session.Remove(CartKey);
            TempData["Message"] = $"Thanks {order.CustomerName}! Order #{order.Id} placed.";
            return RedirectToAction("Index", "Store");
        }
    }
}
