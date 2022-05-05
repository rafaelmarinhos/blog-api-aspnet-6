using blog_api_aspnet_6.Data;
using blog_api_aspnet_6.ExtensionMethods;
using blog_api_aspnet_6.Models;
using blog_api_aspnet_6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace blog_api_aspnet_6.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context, [FromServices] IMemoryCache cache)
        {
            try
            {
                var categories = cache.GetOrCreate("CategoriesCache", entry => 
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return context.Categories.ToListAsync();
                });
                
                return Ok(new ResultViewModel<List<Category>>(await categories));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    return NotFound(new ResultViewModel<List<Category>>("Registro não encontrado."));
                }

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Erro ao obter registro(s)."));
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));

            try
            {
                var category = new Category
                {
                    Name = model.Name,
                    Slug = model.Slug,
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
             
                return Created($"categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Erro ao criar categoria."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErros()));

            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    return NotFound(new ResultViewModel<List<Category>>("Registro não encontrado."));
                }

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Erro ao atualizar categoria."));
            }
        }
    }
}
