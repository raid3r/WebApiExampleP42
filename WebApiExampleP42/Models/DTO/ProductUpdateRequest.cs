namespace WebApiExampleP42.Models.DTO;

public class ProductUpdateRequest
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }
}