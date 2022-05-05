using System.ComponentModel.DataAnnotations;

namespace blog_api_aspnet_6.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Slug { get; set; }
    }
}
