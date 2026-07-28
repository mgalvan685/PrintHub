using FluentValidation;
using PrintHub.DTOs;
using PrintHub.Helpers;

namespace PrintHub.Validators;

public class NewMaterialDtoValidator : AbstractValidator<NewMaterialDto>
{
    public NewMaterialDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Initial_Cost)
            .GreaterThan(0);

        RuleFor(x => x.Units)
            .Must(MaterialUnitConstants.ALL.Contains)
            .WithMessage("Invalid unit type.");

        RuleFor(x => x.Total_Material)
            .GreaterThan(0);

        RuleFor(x => x.Cost_Per_Unit)
            .GreaterThan(0);

        // Nullable
        RuleFor(x => x.Source)
            .MaximumLength(512);
    }
}

