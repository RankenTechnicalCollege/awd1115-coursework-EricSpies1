using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyboardShop.Models
{
    public class Keyboard
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(120)]
        public string Slug { get; set; } = string.Empty;

        [Required, StringLength(120)]
        public string ImageFile { get; set; } = "placeholder.jpg";

        [Required, StringLength(40)]
        public string Brand { get; set; } = string.Empty;

        [Required, StringLength(40)]
        public string SwitchType { get; set; } = "Linear";

        [Required, StringLength(20)]
        public string Layout { get; set; } = "75%";

        [Range(10, 500)]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? Connectivity { get; set; } = "Wired";

        public void SetSlug() => Slug = Name
            .Trim().ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("/", "-")
            .Replace("—", "-");
    }
}
