using WebApiExampleP42.Models;
using WebApiExampleP42.Models.DTO;

namespace WebApiExampleP42.Services;

public class ProductService(IRepository<Product> productRepository)
{
    public async Task<ICollection<Product>> GetAllProductsAsync()
    {
        return await productRepository.GetAllAsync();
    }

    private static Func<IQueryable<Product>, IQueryable<Product>>? GetApplyOrderFunc(string? orderBy)
    {
        return orderBy switch
        {
            "id" => query => query.OrderBy(p => p.Id),
            "-id" => query => query.OrderByDescending(p => p.Id),
            "name" => query => query.OrderBy(p => p.Name),
            "-name" => query => query.OrderByDescending(p => p.Name),
            "price" => query => query.OrderBy(p => p.Price),
            "-price" => query => query.OrderByDescending(p => p.Price),
            _ => null
        };
    }

    private static Func<IQueryable<Product>, IQueryable<Product>>? GetApplyFilterFunc(ProductFilter? filter)
    {
        if (filter == null)
        {
            return null;
        }
        return query =>
        {
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(p => p.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrEmpty(filter.Description))
            {
                query = query.Where(p => p.Description.Contains(filter.Description));
            }
            return query;
        };
    }


    public async Task<PaginatedResult<Product>> GetProductsByPageAsync(PageResultRequest<ProductFilter> pageResultRequest)
    {
        // + пагінація
        // + сортування
        // + фільтрація
        return await productRepository.GetPaginatedAsync(
            pageResultRequest.Page, 
            pageResultRequest.PageSize, 
            GetApplyOrderFunc(pageResultRequest.OrderBy), 
            GetApplyFilterFunc(pageResultRequest.Filter));
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
