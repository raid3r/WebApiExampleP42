using System.ComponentModel.DataAnnotations;

namespace WebApiExampleP42.Models.DTO;

public class ProductUpdateRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required]
    public int CategoryId { get; set; }
}

/*
       "name": "Updated Product Name",
        "price": 19.99,
        "categoryId": 1
  }
 */

/*
 
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;

    public ImageFile? Image { get; set; }

    public virtual Category? Category { get; set; }
}


*/