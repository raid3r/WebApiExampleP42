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
        Func<IQueryable<T>, IQueryable<T>>? orderBy = null
        //Func<IQueryable<T>, IQueryable<T>>? filter = null
        )
    {
        var query = context.Set<T>().AsQueryable();

        if (orderBy != null)
        {
            query = orderBy(query);
        }
        //if (filter != null)
        //{
        //    query = filter(query);
        //}


        // SELECT COUNT(*) FROM Table
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        // SELECT * FROM Table LIMIT pageSize OFFSET (page - 1) * pageSize     

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


    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
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
