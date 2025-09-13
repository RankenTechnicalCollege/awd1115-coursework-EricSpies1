using Microsoft.AspNetCore.Mvc;
using HOT1.Models;

namespace HOT1.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(new OrderModel());

        [HttpPost]
        public IActionResult Index(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                model.Calculate();
                if (!string.IsNullOrWhiteSpace(model.DiscountCode) && model.DiscountAmount == 0)
                {
                    ViewBag.Error = "Invalid discount code. No discount applied.";
                }
            }
            return View(model);
        }
    }
}
