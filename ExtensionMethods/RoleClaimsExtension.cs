using blog_api_aspnet_6.Models;
using System.Security.Claims;

namespace blog_api_aspnet_6.ExtensionMethods
{
    public static class RoleClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>();

            result.Add(new Claim(ClaimTypes.Name, user.Email));
            result.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Slug)));

            return result;
        }
    }
}
