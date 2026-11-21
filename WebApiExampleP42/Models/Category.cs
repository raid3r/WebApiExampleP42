namespace WebApiExampleP42.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
