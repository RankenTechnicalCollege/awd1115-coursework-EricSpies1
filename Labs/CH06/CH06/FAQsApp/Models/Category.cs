using System.ComponentModel.DataAnnotations;

namespace FAQsApp.Models;

public class Category
{
    [Key]
    [Required]
    [MaxLength(32)]
    public string CategoryId { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Faq> Faqs { get; set; } = new List<Faq>();
}
