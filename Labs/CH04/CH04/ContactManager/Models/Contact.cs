using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ContactManager.Models
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Required, StringLength(40)]
        public string Firstname { get; set; } = string.Empty;

        [Required, StringLength(40)]
        public string Lastname { get; set; } = string.Empty;

        [Required, Phone, StringLength(30)]
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(80)]
        public string Email { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category? Category { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public string Slug => $"{Firstname}-{Lastname}".ToLower().Replace(' ', '-');
    }
}
