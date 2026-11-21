using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiExampleP42.Models;
using WebApiExampleP42.Models.DTO;
using Microsoft.EntityFrameworkCore.Query;

namespace WebApiExampleP42.Services;

public class ProductService(
    IRepository<Product> productRepository, 
    IRepository<Category> categoryRepository,
    FileUploadService fileUploadService,
    IMapper mapper
    )
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

    private static Func<IQueryable<Product>, IQueryable<Product>>? GetApplyIncludeFunc()
    {
        return query => query.Include(p => p.Category);
    }


    public async Task<PaginatedResult<ProductView>> GetProductsByPageAsync(PageResultRequest<ProductFilter> pageResultRequest)
    {
        // + пагінація
        // + сортування
        // + фільтрація

        var result = await productRepository.GetPaginatedAsync(
            pageResultRequest.Page,
            pageResultRequest.PageSize,
            GetApplyOrderFunc(pageResultRequest.OrderBy),
            GetApplyFilterFunc(pageResultRequest.Filter),
            GetApplyIncludeFunc()
            );

        return mapper.Map<PaginatedResult<ProductView>>(result);
    }

        
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await productRepository.GetByIdAsync(id, GetApplyIncludeFunc());
    }

    public async Task<Product> AddProductAsync(ProductUpdateRequest request)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            throw new ArgumentException($"Category with ID {request.CategoryId} does not exist.");
        }

        var newProduct = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = category
        };

        productRepository.Add(newProduct);
        await productRepository.SaveChangesAsync();
        return newProduct;

    }

    public async Task<Product?> UpdateProductAsync(int id, ProductUpdateRequest updatedProduct)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return null;
        }
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;

        var newCategoryId = updatedProduct.CategoryId;
        var category = await categoryRepository.GetByIdAsync(newCategoryId);
        if (category == null)
        {
            throw new ArgumentException($"Category with ID {newCategoryId} does not exist.");
        }

        product.Category = category;
        await productRepository.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProductImageAsync(int id, IFormFile imageFile)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return null;
        }
        var newImage = await fileUploadService.UploadFileAsync(imageFile);
        var oldImage = product.Image;
        if (oldImage != null)
        {
            await fileUploadService.DeleteImageFileAsync(oldImage);
        }
        product.Image = newImage;
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
        var oldImage = product.Image;
        if (oldImage != null)
        {
            await fileUploadService.DeleteImageFileAsync(oldImage);
        }
        productRepository.Remove(product);
        await productRepository.SaveChangesAsync();
        return true;
    }
}
