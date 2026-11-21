using WebApiExampleP42.Models.DTO;

namespace WebApiExampleP42.Services;

public interface IRepository<T> where T : class
{
    Task<ICollection<T>> GetAllAsync();
    Task<PaginatedResult<T>> GetPaginatedAsync(
        int page, 
        int pageSize, 
        Func<IQueryable<T>, IQueryable<T>>? orderBy,
        Func<IQueryable<T>, IQueryable<T>>? filter,
        Func<IQueryable<T>, IQueryable<T>>? include = null 
        );
    Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? include = null);
    void Add(T entity);
    void Remove(T entity);
    Task SaveChangesAsync();
}
