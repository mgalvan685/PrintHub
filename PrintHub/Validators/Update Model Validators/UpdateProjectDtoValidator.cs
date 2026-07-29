using FluentValidation;
using PrintHub.DTOs;

namespace PrintHub.Validators.Update_Model_Validators;

public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
{
    public UpdateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(128)
            .When(x => x.Name != null);

        RuleFor(x => x.Printer_ID)
            .GreaterThan(0)
            .When(x => x.Printer_ID != null);

        RuleFor(x => x.Print_Time)
            .Matches(@"^\d{2}:\d{2}:\d{2}$")
            .When(x => x.Print_Time != null);

        RuleFor(x => x.Labor)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Labor != null);

        RuleForEach(x => x.FilamentIds)
            .GreaterThan(0)
            .When(x => x.FilamentIds != null);

        RuleForEach(x => x.MaterialIds)
            .GreaterThan(0)
            .When(x => x.MaterialIds != null);
    }
}
