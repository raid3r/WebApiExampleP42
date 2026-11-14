using WebApiExampleP42.Models;

namespace WebApiExampleP42.Services;

public class MessageService(IRepository<Message> messageRepository)
{
    public async Task<ICollection<Message>> GetAllMessagesAsync()
    {
        return await messageRepository.GetAllAsync();
    }


    public async Task<ICollection<Message>> GetMessagesForEmailAsync(string email)
    {
        // TODO  не ефективно!!!
        var messages = await messageRepository.GetAllAsync();
        return messages.Where(m => m.To == email).ToList();
    }

    public async Task<Message> AddMessageAsync(Message newMessage)
    {
        messageRepository.Add(newMessage);
        await messageRepository.SaveChangesAsync();

        return newMessage;
    }


}
