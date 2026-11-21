using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;
using WebApiExampleP42.Services;
using Microsoft.EntityFrameworkCore;
using WebApiExampleP42.Models.DTO;
using AutoMapper;
namespace WebApiExampleP42.Controllers;

[ApiController]
[Route("categories")]
public class CategoryController(CategoryService categoryService, IMapper mapper) : ControllerBase
{

    
    [HttpGet] // GET /categories?page=1&pageSize=10?orderBy=name
    public async Task<ActionResult<PaginatedResult<Category>>> GetCategoriesByPageAsync(
        [FromQuery] PageResultRequest<CategoryFilter> pageResultRequest
        )
    {
        // + пагінація
        // + сортування
        // + фільтрація

        
        var pageResult = await categoryService.GetCategoriesByPageAsync(pageResultRequest);

        var pageResultDto = new PaginatedResult<CategoryView> // тип DTO, якщо потрібно
        {
            Page = pageResult.Page,
            PageSize = pageResult.PageSize,
            TotalCount = pageResult.TotalCount,
            TotalPages = pageResult.TotalPages,
            Models = pageResult.Models
            .Select(model => mapper.Map<CategoryView>(model)).ToList() // тут можна додати мапінг на DTO, якщо потрібно
        };

        return Ok(pageResultDto);
    }
    
    // GET /categories/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryView>> GetCategoryByIdAsync(int id)
    {
        var category = await categoryService.GetCategoryByIdAsync(id);
        if (category  == null)
        {
            return NotFound(); // 404 Not Found
        }
        return Ok(mapper.Map<CategoryView>(category));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryView>> AddCategoryAsync([FromBody] Category newCategory)
    {
        var category = await categoryService.AddCategoryAsync(newCategory);
        return Ok(mapper.Map<CategoryView>(category));
    }
        
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryView>> UpdateCategoryAsync(int id, [FromBody] Category updatedCategory)
    {
        var category = await categoryService.UpdateCategoryAsync(id, updatedCategory);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<CategoryView>(category));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategoryAsync(int id)
    {
        var result = await categoryService.DeleteCategoryAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }
}
