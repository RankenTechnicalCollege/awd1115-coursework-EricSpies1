using System.ComponentModel.DataAnnotations;

namespace KeyboardShop.ViewModels
{
    public class RegisterViewModel
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; } = "";

        [Required, StringLength(50)]
        public string LastName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required, DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
