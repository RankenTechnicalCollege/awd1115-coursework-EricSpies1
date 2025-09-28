using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FAQsApp.Data;
using FAQsApp.Models;

namespace FAQsApp.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    [HttpGet("topic/{topicId}/category/{categoryId}/")]
    public async Task<IActionResult> IndexFilteredBoth(string topicId, string categoryId)
        => await IndexInternal(topicId, categoryId);

    [HttpGet("category/{categoryId}/topic/{topicId}/")]
    public async Task<IActionResult> IndexFilteredBothAlt(string categoryId, string topicId)
        => await IndexInternal(topicId, categoryId);

    [HttpGet("topic/{topicId}/")]
    public async Task<IActionResult> IndexFilteredTopic(string topicId)
        => await IndexInternal(topicId, null);

    [HttpGet("category/{categoryId}/")]
    public async Task<IActionResult> IndexFilteredCategory(string categoryId)
        => await IndexInternal(null, categoryId);

    [HttpGet("")]
    public async Task<IActionResult> Index() => await IndexInternal(null, null);

    private async Task<IActionResult> IndexInternal(string? topicId, string? categoryId)
    {
        var topics = await _db.Topics.OrderBy(t => t.Name).ToListAsync();
        var categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();

        var query = _db.Faqs
            .Include(f => f.Topic)
            .Include(f => f.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(topicId))
            query = query.Where(f => f.TopicId == topicId);

        if (!string.IsNullOrWhiteSpace(categoryId))
            query = query.Where(f => f.CategoryId == categoryId);

        var faqs = await query
            .OrderBy(f => f.Topic!.Name)
            .ThenBy(f => f.Category!.Name)
            .ThenBy(f => f.Question)
            .ToListAsync();

        var vm = new HomeViewModel
        {
            Topics = topics,
            Categories = categories,
            Faqs = faqs,
            SelectedTopicId = topicId,
            SelectedCategoryId = categoryId
        };

        return View("Index", vm);
    }
}
