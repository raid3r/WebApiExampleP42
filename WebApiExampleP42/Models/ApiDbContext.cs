using Microsoft.EntityFrameworkCore;

namespace WebApiExampleP42.Models;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<ImageFile> Images { get; set; }
}
