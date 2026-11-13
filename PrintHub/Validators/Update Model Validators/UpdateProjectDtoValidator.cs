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
            .When(x => x.Printer_ID.HasValue);

        RuleFor(x => x.Print_Time)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Print_Time.HasValue);

        RuleFor(x => x.Finishing_Time)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Finishing_Time.HasValue);

        RuleForEach(x => x.Filaments)
            .ChildRules(f =>
            {
                f.RuleFor(x => x.Filament_ID)
                    .GreaterThan(0);

                f.RuleFor(x => x.Usage_G)
                    .GreaterThan(0);
            })
            .When(x => x.Filaments != null);

        RuleForEach(x => x.Materials)
            .ChildRules(m =>
            {
                m.RuleFor(x => x.Material_ID)
                    .GreaterThan(0);

                m.RuleFor(x => x.Usage)
                    .GreaterThan(0);
            })
            .When(x => x.Materials != null);
    }
}