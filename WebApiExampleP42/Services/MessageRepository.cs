using WebApiExampleP42.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiExampleP42.Services;

public class MessageRepository(ApiDbContext context) : 
    BaseRepository<Message>(context)
{

}
