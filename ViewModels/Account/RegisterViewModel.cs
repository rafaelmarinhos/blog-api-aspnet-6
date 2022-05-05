using System.ComponentModel.DataAnnotations;

namespace blog_api_aspnet_6.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
