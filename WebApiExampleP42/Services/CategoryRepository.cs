using WebApiExampleP42.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiExampleP42.Services;

public class CategoryRepository(ApiDbContext context) : BaseRepository<Category>(context)
{

}
