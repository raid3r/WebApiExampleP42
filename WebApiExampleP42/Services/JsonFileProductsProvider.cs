using WebApiExampleP42.Models;

namespace WebApiExampleP42.Services;

public class JsonFileProductsProvider : JsonFileDataProvider<Product>, IDataProvider<Product>
{
    public JsonFileProductsProvider(string dataFilePath) : base(dataFilePath)
    {
    }
}
