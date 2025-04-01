using System.ComponentModel.DataAnnotations;

namespace FoodAppMVC.WebMVC.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Ім'я користувача")]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [Display(Name = "Підтвердження пароля")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
