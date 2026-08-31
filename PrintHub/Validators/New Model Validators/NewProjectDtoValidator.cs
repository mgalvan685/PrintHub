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
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Finishing_Time)
            .GreaterThanOrEqualTo(0);

        RuleForEach(x => x.FilamentIds)
            .GreaterThan(0);

        RuleForEach(x => x.MaterialIds)
            .GreaterThan(0)
            .When(x => x.MaterialIds != null);
    }
}
