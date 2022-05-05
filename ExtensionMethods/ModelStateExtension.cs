using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace blog_api_aspnet_6.ExtensionMethods
{
    public static class ModelStateExtension
    {
        public static List<string> GetErros(this ModelStateDictionary modelState)
        {
            var result = new List<string>();

            foreach (var item in modelState.Values)
            {
                result.AddRange(item.Errors.Select(error => error.ErrorMessage));
            }

            return result;
        }
    }
}
