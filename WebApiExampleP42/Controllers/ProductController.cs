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

    // GET /products/search?minPrice=10&maxPrice=100
    [HttpGet("search")]
    public async Task<ActionResult<ICollection<Product>>> SearchProductsAsync([FromQuery] SearchProduct searchProduct)
    {
        if (searchProduct.MaxPrice < searchProduct.MinPrice)
        {
            return BadRequest("maxPrice must be greater than or equal to minPrice.");
        }
        if (searchProduct.MinPrice < 0 || searchProduct.MaxPrice < 0)
        {
            return BadRequest("Prices must be non-negative.");
        }
        if (searchProduct.MaxPrice == 0)
        {
            searchProduct.MaxPrice = decimal.MaxValue; // If maxPrice is 0, treat it as no upper limit
        }


        var products = await productService.SearchProductsAsync(searchProduct);
        return Ok(products);
    }


    // GET /products/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
    {
        var product = await productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound(); // 404 Not Found
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> AddProductAsync([FromBody] Product newProduct)
    {
        var product = await productService.AddProductAsync(newProduct);
        return Ok(product);
    }
        
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProductAsync(int id, [FromBody] Product updatedProduct)
    {
        var product = await productService.UpdateProductAsync(id, updatedProduct);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
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
    public async Task<ActionResult<Product>> UpdateProductImageAsync(int id, [FromForm] IFormFile imageFile)
    {
        var product = await productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        var updatedProduct = await productService.UpdateProductImageAsync(id, imageFile);
        return Ok(updatedProduct);
    }
}
