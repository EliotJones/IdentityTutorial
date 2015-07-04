namespace IdentityTutorial.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel : ViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
}