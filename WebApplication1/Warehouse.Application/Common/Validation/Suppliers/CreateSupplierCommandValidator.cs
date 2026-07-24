using FluentValidation;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Suppliers.Commands.CreateSupplier;

namespace Warehouse.Application.Common.Validation.Suppliers;

public class CreateSupplierCommandValidator
    : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator(ILocalizationService localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer.GetString("SupplierNameRequired"))
            .MaximumLength(100)
            .WithMessage(localizer.GetString("SupplierNameMaxLength"));

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage(localizer.GetString("SupplierCountryRequired"))
            .MaximumLength(100)
            .WithMessage(localizer.GetString("SupplierCountryMaxLength"));

        RuleFor(x => x.ContactEmail)
            .NotEmpty()
            .WithMessage(localizer.GetString("SupplierEmailRequired"))
            .EmailAddress()
            .WithMessage(localizer.GetString("SupplierEmailInvalid"));

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(localizer.GetString("SupplierPhoneRequired"))
            .MaximumLength(20)
            .WithMessage(localizer.GetString("SupplierPhoneMaxLength"));
    }
}