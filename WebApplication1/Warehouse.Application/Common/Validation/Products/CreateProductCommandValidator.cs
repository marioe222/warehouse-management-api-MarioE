using FluentValidation;
using Warehouse.Application.Products.Commands.CreateProduct;

namespace Warehouse.Application.Common.Validation.Products;

public class CreateProductCommandValidator 
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(100)
            .WithMessage("Product name cannot exceed 100 characters");


        RuleFor(x => x.Sku)
            .NotEmpty()
            .WithMessage("SKU is required")
            .MaximumLength(50)
            .WithMessage("SKU cannot exceed 50 characters");


        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required");


        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero");


        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity cannot be negative");


        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be in the future");
    }
}