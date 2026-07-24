using FluentValidation;
using Warehouse.Application.Interfaces;

namespace Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;

public class CreateStockAdjustmentValidator
    : AbstractValidator<CreateStockAdjustmentCommand>
{
    public CreateStockAdjustmentValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage(localizer.GetString("ProductIdRequired"));

        RuleFor(x => x.QuantityChange)
            .NotEqual(0)
            .WithMessage(localizer.GetString("QuantityChangeCannotBeZero"));

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage(localizer.GetString("ReasonRequired"));
    }
}