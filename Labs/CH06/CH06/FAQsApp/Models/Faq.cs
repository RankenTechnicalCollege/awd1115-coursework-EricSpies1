using System.ComponentModel.DataAnnotations;

namespace FAQsApp.Models;

public class Faq
{
    public int FaqId { get; set; }

    [Required, MaxLength(200)]
    public string Question { get; set; } = string.Empty;

    [Required]
    public string Answer { get; set; } = string.Empty;

    [Required]
    public string TopicId { get; set; } = string.Empty;

    [Required]
    public string CategoryId { get; set; } = string.Empty;

    public Topic? Topic { get; set; }
    public Category? Category { get; set; }
}
