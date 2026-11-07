using WebApiExampleP42.Models;

namespace WebApiExampleP42.Services;

public class MessageService(IDataProvider<Message> dataProvider)
{
    public async Task<ICollection<Message>> GetAllMessagesAsync()
    {
        return await dataProvider.GetAllAsync();
    }


    public async Task<ICollection<Message>> GetMessagesForEmailAsync(string email)
    {
        var messages = await dataProvider.GetAllAsync();
        return messages.Where(m => m.To == email).ToList();
    }

    public async Task<Message> AddMessageAsync(Message newMessage)
    {
        var messages = await dataProvider.GetAllAsync();
        newMessage.Id = messages.Any() ? messages.Max(m => m.Id) + 1 : 1;
        newMessage.DateTime = DateTime.Now; // Set the current date and time for the message
        messages.Add(newMessage);
        await dataProvider.SaveAsync(messages);
        return newMessage;
    }


}
