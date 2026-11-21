using FluentValidation;
using System.Data;
using WebApiExampleP42.Models.DTO;
using WebApiExampleP42.Services;

namespace WebApiExampleP42.Models.Validation;

public class ProductUpdateValidator: AbstractValidator<ProductUpdateRequest>
{
    private readonly IRepository<Category> _categoryRepository;

    public ProductUpdateValidator(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(3, 100).WithMessage("Name must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.")
            // те що така категорія існує в базі даних
            .MustAsync(CategoryExists).WithMessage("Category does not exist.");
            

        RuleFor(x => x)
            .MustAsync(ValidateProductByCategory)
            .WithMessage("Product does not meet category-specific validation rules.");

    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        return category != null;
    }

    // в щалежності від категорії різні правила валідації для продукту
    private async Task<bool> ValidateProductByCategory(ProductUpdateRequest product, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
        if (category == null)
        {
            return false; // категорія не існує
        }
        // Приклад: якщо категорія "Electronics", то ціна повинна бути більше 100
        if (category.Name == "Electronics" && product.Price <= 100)
        {
            return false;
        }

        if (category.Name == "Food" && product.Price > 50)
        {
            return false;
        }

        // Додайте інші правила валідації для різних категорій за потреби
        return true;
    }

}
