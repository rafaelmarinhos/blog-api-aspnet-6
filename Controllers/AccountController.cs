using blog_api_aspnet_6.Data;
using blog_api_aspnet_6.ExtensionMethods;
using blog_api_aspnet_6.Models;
using blog_api_aspnet_6.Services;
using blog_api_aspnet_6.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace blog_api_aspnet_6.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model, [FromServices] BlogDataContext context, [FromServices] TokenService tokenService)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

            try
            {
                var user = await context.Users
                    .AsNoTracking()
                    .Include(x => x.Roles)
                    .FirstOrDefaultAsync(f => f.Email == model.Email);

                if (user == null)
                {
                    return NotFound(new ResultViewModel<List<Category>>("Usuário ou senha inválidos."));
                }

                if (!PasswordHasher.Verify(user.PasswordHash, model.Passoword))
                {
                    return NotFound(new ResultViewModel<List<Category>>("Usuário ou senha inválidos."));
                }

                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Erro ao obter usuário."));
            }
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model, [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

            try
            {
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Slug = model.Email.Replace("@", "-")
                };

                var passaword = PasswordGenerator.Generate(25);
                user.PasswordHash = PasswordHasher.Hash(passaword);

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new {
                    user = user.Name, passaword
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>($"Erro ao criar usuário. { ex.InnerException }"));
            }
        }
    }
}
