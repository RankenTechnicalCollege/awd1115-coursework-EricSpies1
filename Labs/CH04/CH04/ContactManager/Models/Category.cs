using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required, StringLength(40)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
