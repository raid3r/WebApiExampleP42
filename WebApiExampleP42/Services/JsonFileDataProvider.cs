using WebApiExampleP42.Models;

namespace WebApiExampleP42.Services;

public class JsonFileDataProvider<T> : IDataProvider<T> where T : class
{
    private readonly string dataFilePath;

    public JsonFileDataProvider(string dataFilePath)
    {
        this.dataFilePath = dataFilePath;
    }

    public async Task<ICollection<T>> GetAllAsync()
    {
        try
        {
            if (File.Exists(dataFilePath))
            {
                var jsonData = await File.ReadAllTextAsync(dataFilePath);
                return System.Text.Json.JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
            }
            else
            {
                return new List<T>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
            return new List<T>();
        }
    }

    public async Task SaveAsync(ICollection<T> items)
    {
        try
        {
            var jsonData = System.Text.Json.JsonSerializer.Serialize(items);
            await File.WriteAllTextAsync(dataFilePath, jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }
}
