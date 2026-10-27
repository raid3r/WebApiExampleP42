using WebApiExampleP42.Models;

namespace WebApiExampleP42.Services;

public class JsonFileProductsProvider : IDataProvider<Product>
{
    private readonly string dataFilePath;

    public JsonFileProductsProvider(string dataFilePath)
    {
        this.dataFilePath = dataFilePath;
    }

    public async Task<ICollection<Product>> GetAllAsync()
    {
        try
        {
            if (File.Exists(dataFilePath))
            {
                var jsonData = await File.ReadAllTextAsync(dataFilePath);
                return System.Text.Json.JsonSerializer.Deserialize<List<Product>>(jsonData) ?? new List<Product>();
            }
            else
            {
                return new List<Product>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading products: {ex.Message}");
            return new List<Product>();
        }
    }

    public async Task SaveAsync(ICollection<Product> items)
    {
        try
        {
            var jsonData = System.Text.Json.JsonSerializer.Serialize(items);
            await File.WriteAllTextAsync(dataFilePath, jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving products: {ex.Message}");
        }
    }
}
