using System.ComponentModel.DataAnnotations;

namespace QuarterlySales.Models
{
    public class SalesEntry
    {
        public int Id { get; set; }

        [Range(1, 4, ErrorMessage = "Quarter must be between 1 and 4.")]
        public int Quarter { get; set; }

        [Range(2001, 9999, ErrorMessage = "Year must be after the year 2000.")]
        public int Year { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required, Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
