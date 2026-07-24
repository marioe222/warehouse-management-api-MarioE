using FluentValidation;
using Warehouse.Application.Interfaces;
using Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;

namespace Warehouse.Application.Common.Validation.StockAdjustments;

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