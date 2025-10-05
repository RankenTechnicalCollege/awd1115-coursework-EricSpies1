using Microsoft.AspNetCore.Mvc;

namespace MyWebsite.Areas.Help.Controllers
{
    [Area("Help")]
    public class TutorialController : Controller
    {
        public IActionResult Index(int id = 1)
        {
            ViewData["Page"] = id;
            // pick view based on id
            return View($"Page{id}");
        }
    }
}
