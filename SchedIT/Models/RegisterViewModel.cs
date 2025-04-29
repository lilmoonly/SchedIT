using System.ComponentModel.DataAnnotations;


namespace MyMvcApp.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ім'я обов'язкове")]
    [Display(Name = "Прізвище та ім'я ")]
    public string FullName { get; set; }
    
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Введіть пароль")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Підтвердіть пароль")]
    [Compare("Password", ErrorMessage = "Паролі не співпадають.")]
    public string ConfirmPassword { get; set; }
}