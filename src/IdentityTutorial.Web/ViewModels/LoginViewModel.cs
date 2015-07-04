namespace IdentityTutorial.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNet.Http.Authentication;

    public class LoginViewModel : ViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public IList<AuthenticationDescription> ExternalProviders { get; set; }
    }
}