using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesOrders.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        public string ProductDescShort { get; set; } = "";
        public string ProductDescLong { get; set; } = "";

        [Required(ErrorMessage = "Product image is required.")]
        public string ProductImage { get; set; } = "";

        [Range(1, 100000, ErrorMessage = "Price must be between 1 and 100000.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProductPrice { get; set; } = 0m;

        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
        public int ProductQty { get; set; } = 0;

        public int CategoryID { get; set; }
        public Category? Category { get; set; }

        [NotMapped]
        public string Slug => (ProductName ?? "").Trim().ToLower().Replace(' ', '-');
    }
}
