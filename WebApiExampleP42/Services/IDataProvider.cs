namespace WebApiExampleP42.Services;

public interface IDataProvider<T> where T : class
{
    public Task<ICollection<T>> GetAllAsync();

    public Task SaveAsync(ICollection<T> items);

}
