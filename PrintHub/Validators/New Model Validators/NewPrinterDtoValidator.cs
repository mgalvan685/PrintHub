using FluentValidation;
using PrintHub.DTOs;
using PrintHub.Validators; // adjust namespace

public class NewPrinterDtoValidator : AbstractValidator<NewPrinterDto>
{
    public NewPrinterDtoValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required.")
            .MaximumLength(50).WithMessage("Brand cannot exceed 50 characters.");

        RuleFor(x => x.Type)
            .Must(type => ModelValidators.IsValidPrinterType(type))
            .WithMessage("Invalid printer type. Allowed values: 3D, Laser, Cutter.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Power_Per_Hour)
            .GreaterThan(0).WithMessage("Power per hour must be greater than zero.");
    }
}
