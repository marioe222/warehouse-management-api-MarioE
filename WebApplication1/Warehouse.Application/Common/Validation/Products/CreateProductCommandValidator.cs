using FluentValidation;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Products.Commands.CreateProduct;

namespace Warehouse.Application.Common.Validation.Products;

public class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer.GetString("ProductNameRequired"))
            .MaximumLength(100)
            .WithMessage(localizer.GetString("ProductNameMaxLength"));

        RuleFor(x => x.Sku)
            .NotEmpty()
            .WithMessage(localizer.GetString("SkuRequired"))
            .MaximumLength(50)
            .WithMessage(localizer.GetString("SkuMaxLength"));

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(localizer.GetString("ProductDescriptionRequired"));

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage(localizer.GetString("ProductPriceMustBePositive"));

        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localizer.GetString("ProductQuantityCannotBeNegative"));

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage(localizer.GetString("ProductExpiryDateMustBeFuture"));
    }
}