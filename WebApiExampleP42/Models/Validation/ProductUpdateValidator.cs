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
            

        RuleFor(x => x).CustomAsync(ValidateProductByCategory);

    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        return category != null;
    }

    // в щалежності від категорії різні правила валідації для продукту
    private async Task ValidateProductByCategory(
        ProductUpdateRequest product, 
        ValidationContext<ProductUpdateRequest> context,
        CancellationToken cancellationToken
        )
    {
        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);
        if (category == null)
        {
            context.AddFailure("CategoryId", $"Category with ID {product.CategoryId} does not exist.");
            return;
        }
        // Приклад: якщо категорія "Electronics", то ціна повинна бути більше 100
        if (category!.Name == "Electronics" && product.Price <= 100)
        {
            context.AddFailure("Price", "For Electronics category, price must be greater than 100.");
            return;
        }

        if (category.Name == "Food" && product.Price > 50)
        {
            context.AddFailure("Price", "For Food category, price must be less than or equal to 50.");
            return;
        }

        // Додайте інші правила валідації для різних категорій за потреби
    }

}
