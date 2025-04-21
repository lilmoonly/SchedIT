using System.ComponentModel.DataAnnotations;


namespace MyMvcApp.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ім'я обов'язкове")]
    public string FullName { get; set; }
    
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Підтвердження пароля")]
    [Compare("Password", ErrorMessage = "Паролі не співпадають.")]
    public string ConfirmPassword { get; set; }
}