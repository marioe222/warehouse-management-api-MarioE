using FluentValidation;
using Warehouse.Application.Suppliers.Commands.CreateSupplier;

namespace Warehouse.Application.Common.Validation.Suppliers;

public class CreateSupplierCommandValidator
    : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Supplier name is required")
            .MaximumLength(100)
            .WithMessage("Supplier name cannot exceed 100 characters");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Supplier country is required")
            .MaximumLength(100)
            .WithMessage("Supplier country cannot exceed 100 characters");

        RuleFor(x => x.ContactEmail)
            .NotEmpty()
            .WithMessage("Supplier email is required")
            .EmailAddress()
            .WithMessage("Supplier email format is invalid");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Supplier phone number is required")
            .MaximumLength(20)
            .WithMessage("Supplier phone number cannot exceed 20 characters");
    }
}