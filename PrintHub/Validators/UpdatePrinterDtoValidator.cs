namespace PrintHub.Validators;

using FluentValidation;
using PrintHub.DTOs;

public class UpdatePrinterDtoValidator : AbstractValidator<UpdatePrinterDto>
{
    public UpdatePrinterDtoValidator()
    {
        RuleFor(x => x.Brand)
            .MaximumLength(50)
            .When(x => x.Brand != null);

        RuleFor(x => x.Type)
            .Must(ModelValidators.IsValidPrinterType!)
            .When(x => x.Type != null);

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => x.Name != null);

        RuleFor(x => x.Power_Per_Hour)
            .GreaterThan(0)
            .When(x => x.Power_Per_Hour.HasValue);
    }
}
