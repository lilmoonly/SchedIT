using System.ComponentModel.DataAnnotations;


namespace MyMvcApp.Models;

public class LoginViewModel
{
        [Required(ErrorMessage = "Будь ласка, введіть ваш email")]
        [EmailAddress(ErrorMessage = "Будь ласка, введіть правильний email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть ваш пароль")]
        public string Password { get; set; }
}