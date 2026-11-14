using WebApiExampleP42.Models;
using WebApiExampleP42.Models.DTO;

namespace WebApiExampleP42.Services;

public class ProductService(IRepository<Product> productRepository)
{
    public async Task<ICollection<Product>> GetAllProductsAsync()
    {
        return await productRepository.GetAllAsync();
    }

    public async Task<PaginatedResult<Product>> GetProductsByPageAsync(int page, int pageSize, string? orderBy = null)
    {
        return await productRepository.GetPaginatedAsync(page, pageSize, query => 
        {
            return orderBy switch
            {
                "id" => query.OrderBy(p => p.Id),
                "-id" => query.OrderByDescending(p => p.Id),
                "name" => query.OrderBy(p => p.Name),
                "-name" => query.OrderByDescending(p => p.Name),
                "price" => query.OrderBy(p => p.Price),
                "-price" => query.OrderByDescending(p => p.Price),
                _ => query
            };
        });
    }


    public async Task<ICollection<Product>> SearchProductsAsync(SearchProduct searchProduct)
    {
        // TODO  не ефективно!!!
        return await productRepository.GetAllAsync()
            .ContinueWith(task => task.Result
                .Where(p => p.Price >= searchProduct.MinPrice && p.Price <= searchProduct.MaxPrice)
                .ToList());
    }


    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await productRepository.GetByIdAsync(id);
    }

    public async Task<Product> AddProductAsync(Product newProduct)
    {
        productRepository.Add(newProduct);
        await productRepository.SaveChangesAsync();
        return newProduct;

    }

    public async Task<Product?> UpdateProductAsync(int id, Product updatedProduct)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return null;
        }
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;
        await productRepository.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }
        productRepository.Remove(product);
        await productRepository.SaveChangesAsync();
        return true;
    }
}
