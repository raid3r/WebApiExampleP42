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
    

    [HttpGet] // GET /products?page=1&pageSize=10
    public async Task<ActionResult<PaginatedResult<Product>>> GetProductsByPageAsync(
        int page = 1, int pageSize = 10
        )
    {
        // пагінація
        // сортування
        // фільтрація

        /*
         * 50 
         * 
         * 1-10
         * 11-20
         * 21-30
         * 4 page 31-40
         * 41-50
         * 
         */

        // усі 50 продуктів

        var pageResult = await productService.GetProductsByPageAsync(page, pageSize);

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
}
