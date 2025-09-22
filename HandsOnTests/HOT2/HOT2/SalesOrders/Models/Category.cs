using System.ComponentModel.DataAnnotations;

namespace SalesOrders.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public Product? Product { get; set; }
    }
}
