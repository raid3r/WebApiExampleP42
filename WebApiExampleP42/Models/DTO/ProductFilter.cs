namespace WebApiExampleP42.Models.DTO;

public class ProductFilter
{
    public decimal? MinPrice { get; set; } = null;
    public decimal? MaxPrice { get; set; } = null;
    public string? Name { get; set; } = null;
    public string? Description { get; set; } = null;

}
