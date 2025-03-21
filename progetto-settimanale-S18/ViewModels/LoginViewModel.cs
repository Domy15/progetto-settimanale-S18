using System.ComponentModel.DataAnnotations;

namespace progetto_settimanale_S18.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email is not valid!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [MinLength(8, ErrorMessage = "Password must contain at least 8 characters!")]
        public string Password { get; set; }
    }
}
