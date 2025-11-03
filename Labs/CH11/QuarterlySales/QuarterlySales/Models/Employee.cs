using System.ComponentModel.DataAnnotations;

namespace QuarterlySales.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required, StringLength(40)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(40)]
        public string LastName { get; set; } = string.Empty;

        [Required, DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime DOB { get; set; }

        [Required, DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime DateOfHire { get; set; }

        [Display(Name = "Manager")]
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
