using System.ComponentModel.DataAnnotations;

namespace progetto_settimanale_S18.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Eemail is not valid!")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required!")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Bith Date is required!")]
        public required DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required!")]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role is required!")]
        public string Role {  get; set; }
    }
}
