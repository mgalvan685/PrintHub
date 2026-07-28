using FluentValidation;
using PrintHub.DTOs;
using PrintHub.Helpers;

namespace PrintHub.Validators;

public class UpdateMaterialDtoValidator : AbstractValidator<UpdateMaterialDto>
{
    public UpdateMaterialDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => x.Name != null);

        RuleFor(x => x.Initial_Cost)
            .GreaterThan(0)
            .When(x => x.Initial_Cost.HasValue);

        RuleFor(x => x.Units)
            .Must(MaterialUnitConstants.ALL.Contains)
            .When(x => x.Units != null);

        RuleFor(x => x.Total_Material)
            .GreaterThan(0)
            .When(x => x.Total_Material.HasValue);

        RuleFor(x => x.Cost_Per_Unit)
            .GreaterThan(0)
            .When(x => x.Cost_Per_Unit.HasValue);

        RuleFor(x => x.Source)
            .MaximumLength(512)
            .When(x => x.Source != null);
    }
}

