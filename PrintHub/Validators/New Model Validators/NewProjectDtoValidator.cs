using FluentValidation;
using PrintHub.DTOs;

namespace PrintHub.Validators.New_Model_Validators;

public class NewProjectDtoValidator : AbstractValidator<NewProjectDto>
{
    public NewProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.Printer_ID)
            .GreaterThan(0);

        RuleFor(x => x.Print_Time)
            .Matches(@"^\d{2}:\d{2}:\d{2}$")
            .WithMessage("Print_Time must be in HH:mm:ss format.");

        RuleFor(x => x.Labor)
            .GreaterThanOrEqualTo(0);

        RuleForEach(x => x.FilamentIds)
            .GreaterThan(0);

        RuleForEach(x => x.MaterialIds)
            .GreaterThan(0)
            .When(x => x.MaterialIds != null);
    }
}
