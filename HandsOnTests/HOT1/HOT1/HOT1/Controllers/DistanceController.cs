using Microsoft.AspNetCore.Mvc;
using HOT1.Models;

namespace HOT1.Controllers
{
    public class DistanceController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View(new DistanceModel());

        [HttpPost]
        public IActionResult Index(DistanceModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Result = $"{model.Inches:F2} inches = {model.Centimeters:F2} cm";
            }
            return View(model);
        }
    }
}
