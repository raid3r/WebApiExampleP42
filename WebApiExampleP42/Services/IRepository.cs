namespace WebApiExampleP42.Services;

public interface IRepository<T> where T : class
{
    Task<ICollection<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    void Add(T entity);
    void Remove(T entity);
    Task SaveChangesAsync();
}
