using FluentValidation;
using PrintHub.DTOs;
using PrintHub.Helpers;

namespace PrintHub.Validators;

public class NewFilamentDtoValidator : AbstractValidator<NewFilamentDto>
{
    public NewFilamentDtoValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required.")
            .MaximumLength(50);

        RuleFor(x => x.Material)
            .NotEmpty().WithMessage("Material is required.")
            .MaximumLength(50);

        RuleFor(x => x.Texture)
            .Must(FilamentTextureConstants.ALL.Contains)
            .WithMessage("Invalid filament texture.");


        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Color is required.")
            .MaximumLength(50);

        RuleFor(x => x.Weight_Grams)
            .GreaterThan(0).WithMessage("Weight must be greater than zero.");

        RuleFor(x => x.Cost)
            .GreaterThan(0).WithMessage("Cost must be greater than zero.");
    }
}
