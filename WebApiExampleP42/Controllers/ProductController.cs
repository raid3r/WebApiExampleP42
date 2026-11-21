using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;
using WebApiExampleP42.Services;
using Microsoft.EntityFrameworkCore;
using WebApiExampleP42.Models.DTO;
using AutoMapper;
namespace WebApiExampleP42.Controllers;

[ApiController]
[Route("products")]
public class ProductController(ProductService productService, IMapper mapper) : ControllerBase
{

    // price - по зростанню, -price - по спаданню
    // name - по алфавіту, -name - по зворотньому алфавіту
    [HttpGet] // GET /products?page=1&pageSize=10?orderBy=price
    public async Task<ActionResult<PaginatedResult<Product>>> GetProductsByPageAsync(
        [FromQuery] PageResultRequest<ProductFilter> pageResultRequest
        )
    {
        // + пагінація
        // + сортування
        // + фільтрація

        
        var pageResult = await productService.GetProductsByPageAsync(pageResultRequest);

        var pageResultDto = new PaginatedResult<ProductView> // тип DTO, якщо потрібно
        {
            Page = pageResult.Page,
            PageSize = pageResult.PageSize,
            TotalCount = pageResult.TotalCount,
            TotalPages = pageResult.TotalPages,
            Models = pageResult.Models
            .Select(model => mapper.Map<ProductView>(model)).ToList() // тут можна додати мапінг на DTO, якщо потрібно
        };

        return Ok(pageResultDto);
    }

    // GET /products/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductView>> GetProductByIdAsync(int id)
    {
        var product = await productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound(); // 404 Not Found
        }
        return Ok(mapper.Map<ProductView>(product));
    }

    [HttpPost]
    public async Task<ActionResult<ProductView>> AddProductAsync([FromBody] ProductUpdateRequest request)
    {
        var product = await productService.AddProductAsync(request);
        return Ok(mapper.Map<ProductView>(product));
    }
        
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductView>> UpdateProductAsync(int id, [FromBody] ProductUpdateRequest updatedProduct)
    {
        var product = await productService.UpdateProductAsync(id, updatedProduct);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<ProductView>(product));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        var result = await productService.DeleteProductAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpPost("{id:int}/image")]
    public async Task<ActionResult<ProductView>> UpdateProductImageAsync(int id, [FromForm] IFormFile imageFile)
    {
        var product = await productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        var updatedProduct = await productService.UpdateProductImageAsync(id, imageFile);
        return Ok(mapper.Map<ProductView>(updatedProduct));
    }
}
