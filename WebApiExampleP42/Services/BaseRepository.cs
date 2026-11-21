using WebApiExampleP42.Models;
using Microsoft.EntityFrameworkCore;
using WebApiExampleP42.Models.DTO;

namespace WebApiExampleP42.Services;

abstract public class BaseRepository<T>(ApiDbContext context) : IRepository<T> where T : class
{
    public async Task<ICollection<T>> GetAllAsync()
    {
        // SELECT * FROM Table
        return await context.Set<T>().ToListAsync();
    }

    public async Task<PaginatedResult<T>> GetPaginatedAsync(int page, int pageSize, 
        Func<IQueryable<T>, IQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null
        )
    {
        var query = context.Set<T>().AsQueryable();

        // + сортування
        if (orderBy != null)
        {
            // ORDER BY Name ASC
            query = orderBy(query);
        }
        // + фільтрація
        if (filter != null)
        {
            // WHERE Price > 100 AND Price< 500
            query = filter(query);
        }

        // + include вкладених об'єктів
        if (include != null)
        {
            query = include(query);
        }

        
        // + пагінація
        // SELECT COUNT(*) FROM Table WHERE Price > 100 AND Price < 500
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // SELECT * FROM Table LIMIT pageSize OFFSET (page - 1) * pageSize
        // WHERE Price > 100 AND Price < 500
        // ORDER BY Name ASC    
        var models = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<T>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Models = models
        };
    }


    public async Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        var query = context.Set<T>().AsQueryable();
        if (include != null)
        {
            query = include(query);
        }
        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }
    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
