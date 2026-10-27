using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;
using WebApiExampleP42.Services;

namespace WebApiExampleP42.Controllers;

[ApiController]
[Route("products")]
public class ProductController: ControllerBase
{
    private readonly ProductService productService;

    public ProductController(ProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Product>>> GetAllProductsAsync()
    {
        var products = await productService.GetAllProductsAsync();
        return Ok(products);
    }

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
