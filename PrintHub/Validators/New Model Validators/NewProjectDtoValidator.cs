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

        RuleFor(x => x.Filaments)
            .NotEmpty()
            .WithMessage("At least one filament is required.");

        RuleForEach(x => x.Filaments)
            .ChildRules(f =>
            {
                f.RuleFor(x => x.Filament_ID)
                    .GreaterThan(0);

                f.RuleFor(x => x.Usage_G)
                    .GreaterThan(0);
            });

        RuleForEach(x => x.Materials)
            .ChildRules(m =>
            {
                m.RuleFor(x => x.Material_ID)
                    .GreaterThan(0);

                m.RuleFor(x => x.Usage)
                    .GreaterThan(0);
            });
    }
}
