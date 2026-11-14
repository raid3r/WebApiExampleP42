using WebApiExampleP42.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiExampleP42.Services;

abstract public class BaseRepository<T>(ApiDbContext context) : IRepository<T> where T : class
{
    public async Task<ICollection<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
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
