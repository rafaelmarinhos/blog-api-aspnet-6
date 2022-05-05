using blog_api_aspnet_6.Data;
using blog_api_aspnet_6.ExtensionMethods;
using blog_api_aspnet_6.Models;
using blog_api_aspnet_6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_api_aspnet_6.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context, int page = 1, int pageSize = 3)
        {
            try
            {
                var posts = await context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new 
                { 
                    page, 
                    pageSize, 
                    posts 
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Erro ao obter registro(s)."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, [FromRoute] int id)
        {
            try
            {
                var post = await context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .ThenInclude(y => y.Roles)
                    .FirstOrDefaultAsync(x => x.Id == id);

                return Ok(new ResultViewModel<Post>(post));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Post>>("Erro ao obter registro(s)."));
            }
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context, [FromRoute] string category, int page = 1, int pageSize = 3)
        {
            try
            {
                var posts = await context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Where(x => x.Category.Slug == category)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    page,
                    pageSize,
                    posts
                }));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>(e.Message));
            }
        }
    }
}
