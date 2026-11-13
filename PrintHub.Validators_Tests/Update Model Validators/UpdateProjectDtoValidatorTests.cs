using FluentValidation.TestHelper;
using PrintHub.DTOs;
using PrintHub.Validators.Update_Model_Validators;

namespace PrintHub.Validators_Tests.Validators;

public class UpdateProjectDtoValidatorTests
{
    private readonly UpdateProjectDtoValidator _validator = new();

    #region Happy Path Tests

    [Fact]
    public void ValidProjectUpdate_ShouldNotHaveAnyValidationErrors()
    {
        var dto = new UpdateProjectDto
        {
            Name = "Dragon Test",
            Description = "Updated project",

            Printer_ID = 1,

            Print_Time = 12,
            Finishing_Time = 1,

            Filaments =
            [
                new NewProjectFilamentDto
                {
                    Filament_ID = 1,
                    Usage_G = 350
                }
            ],

            Materials =
            [
                new NewProjectMaterialDto
                {
                    Material_ID = 2,
                    Usage = 4
                }
            ]
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Empty Update Tests

    [Fact]
    public void EmptyUpdate_ShouldNotHaveAnyValidationErrors()
    {
        var dto = new UpdateProjectDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Name Tests

    [Fact]
    public void Name_TooLong_ShouldHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Name = new string('A', 129)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Name_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Name = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    #endregion

    #region Printer Tests

    [Fact]
    public void PrinterId_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Printer_ID = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Printer_ID);
    }

    [Fact]
    public void PrinterId_Zero_ShouldHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Printer_ID = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Printer_ID);
    }

    [Fact]
    public void PrinterId_Negative_ShouldHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Printer_ID = -1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Printer_ID);
    }

    [Fact]
    public void PrinterId_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Printer_ID = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Printer_ID);
    }

    #endregion

    #region Print Time Tests

    [Fact]
    public void PrintTime_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Print_Time = 12
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Print_Time);
    }

    [Fact]
    public void PrintTime_Zero_ShouldNotHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Print_Time = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Print_Time);
    }

    [Fact]
    public void PrintTime_Negative_ShouldHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Print_Time = -1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Print_Time);
    }

    [Fact]
    public void PrintTime_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Print_Time = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Print_Time);
    }

    #endregion

    #region Finishing Time Tests

    [Fact]
    public void FinishingTime_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Finishing_Time = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Finishing_Time);
    }

    [Fact]
    public void FinishingTime_Zero_ShouldNotHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Finishing_Time = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Finishing_Time);
    }

    [Fact]
    public void FinishingTime_Negative_ShouldHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Finishing_Time = -1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Finishing_Time);
    }

    [Fact]
    public void FinishingTime_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Finishing_Time = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Finishing_Time);
    }

    #endregion

    #region Filament Tests

    [Fact]
    public void Filament_WithValidValues_ShouldNotHaveValidationErrors()
    {
        var dto = new UpdateProjectDto
        {
            Filaments =
            [
                new NewProjectFilamentDto
            {
                Filament_ID = 1,
                Usage_G = 350
            }
            ]
        };

        var result = _validator.TestValidate(dto);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Filament_WithInvalidFilamentId_ShouldHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Filaments =
            [
                new NewProjectFilamentDto
            {
                Filament_ID = 0,
                Usage_G = 350
            }
            ]
        };

        var result = _validator.TestValidate(dto);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Filament_WithInvalidUsage_ShouldHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Filaments =
            [
                new NewProjectFilamentDto
            {
                Filament_ID = 1,
                Usage_G = 0
            }
            ]
        };

        var result = _validator.TestValidate(dto);

        Assert.False(result.IsValid);
    }

    #endregion

    #region Material Tests

    [Fact]
    public void Material_WithValidValues_ShouldNotHaveValidationErrors()
    {
        var dto = new UpdateProjectDto
        {
            Materials =
            [
                new NewProjectMaterialDto
            {
                Material_ID = 2,
                Usage = 4
            }
            ]
        };

        var result = _validator.TestValidate(dto);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Material_WithInvalidMaterialId_ShouldHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Materials =
            [
                new NewProjectMaterialDto
            {
                Material_ID = 0,
                Usage = 4
            }
            ]
        };

        var result = _validator.TestValidate(dto);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Material_WithInvalidUsage_ShouldHaveValidationError()
    {
        var dto = new UpdateProjectDto
        {
            Materials =
            [
                new NewProjectMaterialDto
            {
                Material_ID = 2,
                Usage = 0
            }
            ]
        };

        var result = _validator.TestValidate(dto);

        Assert.False(result.IsValid);
    }

    #endregion
}