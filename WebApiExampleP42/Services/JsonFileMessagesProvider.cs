using WebApiExampleP42.Models;

namespace WebApiExampleP42.Services;

public class JsonFileMessagesProvider : JsonFileDataProvider<Message>, IDataProvider<Message>
{
    public JsonFileMessagesProvider(string dataFilePath) : base(dataFilePath)
    {
    }
}
