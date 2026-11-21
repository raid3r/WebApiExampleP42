using WebApiExampleP42.Models;
using WebApiExampleP42.Models.DTO;

namespace WebApiExampleP42.Services;

public class CategoryService(IRepository<Category> categoryRepository)
{
    public async Task<ICollection<Category>> GetAllCategoriesAsync()
    {
        return await categoryRepository.GetAllAsync();
    }

    private static Func<IQueryable<Category>, IQueryable<Category>>? GetApplyOrderFunc(string? orderBy)
    {
        return orderBy switch
        {
            "id" => query => query.OrderBy(c => c.Id),
            "-id" => query => query.OrderByDescending(c => c.Id),
            "name" => query => query.OrderBy(c => c.Name),
            "-name" => query => query.OrderByDescending(c => c.Name),
            _ => null
        };
    }

    private static Func<IQueryable<Category>, IQueryable<Category>>? GetApplyFilterFunc(CategoryFilter? filter)
    {
        if (filter == null)
        {
            return null;
        }
        return query =>
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(p => p.Name.Contains(filter.Name));
            }
            return query;
        };
    }


    public async Task<PaginatedResult<Category>> GetCategoriesByPageAsync(PageResultRequest<CategoryFilter> pageResultRequest)
    {
        // + пагінація
        // + сортування
        // + фільтрація
        return await categoryRepository.GetPaginatedAsync(
            pageResultRequest.Page, 
            pageResultRequest.PageSize, 
            GetApplyOrderFunc(pageResultRequest.OrderBy), 
            GetApplyFilterFunc(pageResultRequest.Filter));
    }


    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await categoryRepository.GetByIdAsync(id);
    }

    public async Task<Category> AddCategoryAsync(Category newCategory)
    {
        categoryRepository.Add(newCategory);
        await categoryRepository.SaveChangesAsync();
        return newCategory;

    }

    public async Task<Category?> UpdateCategoryAsync(int id, Category updatedCategory)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return null;
        }
        category.Name = updatedCategory.Name;
        await categoryRepository.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return false;
        }
        
        categoryRepository.Remove(category);
        await categoryRepository.SaveChangesAsync();
        return true;
    }
}
