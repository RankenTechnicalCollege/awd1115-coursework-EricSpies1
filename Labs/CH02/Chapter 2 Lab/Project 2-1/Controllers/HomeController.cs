using Microsoft.AspNetCore.Mvc;
using PriceQuotation.Models;

namespace PriceQuotation.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(new PriceQuote());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(PriceQuote model, string? actionType)
        {
            if (actionType == "clear")
            {
                ModelState.Clear();
                return View(new PriceQuote());
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return View(model);
        }
    }
}
