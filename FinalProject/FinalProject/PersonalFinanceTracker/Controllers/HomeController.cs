using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _http;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory http)
        {
            _logger = logger;
            _http = http;
        }

        public async Task<IActionResult> Index()
        {
            string quoteText = "Welcome back! Track your finances one step at a time.";
            string quoteAuthor = "Personal Finance Tracker";

            try
            {
                var client = _http.CreateClient();

                var json = await client.GetStringAsync("https://api.quotable.io/random?tags=motivational|inspirational");
                using var doc = JsonDocument.Parse(json);

                var root = doc.RootElement;

                if (root.TryGetProperty("content", out var content))
                    quoteText = content.GetString() ?? quoteText;

                if (root.TryGetProperty("author", out var author))
                    quoteAuthor = author.GetString() ?? quoteAuthor;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Quote API call failed.");
            }

            ViewData["QuoteText"] = quoteText;
            ViewData["QuoteAuthor"] = quoteAuthor;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
