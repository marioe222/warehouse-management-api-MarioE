using FluentValidation;
using Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;

namespace Warehouse.Application.Common.Validation.StockAdjustments;

public class CreateStockAdjustmentValidator
    : AbstractValidator<CreateStockAdjustmentCommand>
{
    public CreateStockAdjustmentValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");

        RuleFor(x => x.QuantityChange)
            .NotEqual(0)
            .WithMessage("Quantity change cannot be zero.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason is required.");
    }
}