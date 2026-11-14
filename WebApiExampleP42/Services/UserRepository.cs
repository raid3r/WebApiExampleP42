using WebApiExampleP42.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiExampleP42.Services;

public class UserRepository(ApiDbContext context) : BaseRepository<User>(context)
{

}
