using FluentValidation;
using PrintHub.DTOs;

namespace PrintHub.Validators;

public class UpdateFilamentDtoValidator : AbstractValidator<UpdateFilamentDto>
{
    public UpdateFilamentDtoValidator()
    {
        RuleFor(x => x.Brand)
            .MaximumLength(50)
            .When(x => x.Brand != null);

        RuleFor(x => x.Material)
            .Must(ModelValidators.IsValidFilamentMaterial!)
            .When(x => x.Material != null);

        RuleFor(x => x.Color)
            .MaximumLength(50)
            .When(x => x.Color != null);

        RuleFor(x => x.Weight_Grams)
            .GreaterThan(0)
            .When(x => x.Weight_Grams.HasValue);

        RuleFor(x => x.Cost)
            .GreaterThan(0)
            .When(x => x.Cost.HasValue);
    }
}
