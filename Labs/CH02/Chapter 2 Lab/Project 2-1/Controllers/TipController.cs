using Microsoft.AspNetCore.Mvc;
using PriceQuotation.Models;

namespace PriceQuotation.Controllers
{
    public class TipController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(new TipModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TipModel model, string? actionType)
        {
            if (actionType == "clear")
            {
                ModelState.Clear();
                return View(new TipModel());
            }

            if (!ModelState.IsValid)
                return View(model);

            return View(model);
        }
    }
}
