using Microsoft.AspNetCore.Mvc;

namespace MyWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }
        public IActionResult About()
        {
            ViewData["Title"] = "About";
            return View();
        }
        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact";
            ViewData["Phone"] = "555-123-4567";
            ViewData["Email"] = "me@ericwebsite.com";
            ViewData["Facebook"] = "facebook.com/ericwebsite";
            return View();
        }
    }
}
