using System.ComponentModel.DataAnnotations;

namespace blog_api_aspnet_6.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [Required]        
        public string Passoword { get; set; } = "";
    }
}
