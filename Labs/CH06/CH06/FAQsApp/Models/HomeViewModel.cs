using FAQsApp.Models;

namespace FAQsApp.Models;

public class HomeViewModel
{
    public IEnumerable<Topic> Topics { get; set; } = Enumerable.Empty<Topic>();
    public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
    public IEnumerable<Faq> Faqs { get; set; } = Enumerable.Empty<Faq>();

    public string? SelectedTopicId { get; set; }
    public string? SelectedCategoryId { get; set; }
}
