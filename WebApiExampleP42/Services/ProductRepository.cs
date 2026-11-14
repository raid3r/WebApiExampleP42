using WebApiExampleP42.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiExampleP42.Services;

public class ProductRepository(ApiDbContext context) : 
    BaseRepository<Product>(context)
{

}
