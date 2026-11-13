using FluentValidation.TestHelper;
using PrintHub.DTOs;
using PrintHub.Validators.New_Model_Validators;

namespace PrintHub.Validators_Tests.Validators;

public class NewProjectDtoValidatorTests
{
    private readonly NewProjectDtoValidator _validator = new();

    #region Happy Path Tests

    [Fact]
    public void ValidProject_ShouldNotHaveAnyValidationErrors()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Name Tests

    [Fact]
    public void Name_Empty_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Name = string.Empty;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Name_TooLong_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Name = new string('A', 129);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    #endregion

    #region Printer Tests

    [Fact]
    public void PrinterId_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Printer_ID);
    }

    [Fact]
    public void PrinterId_Zero_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Printer_ID = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Printer_ID);
    }

    [Fact]
    public void PrinterId_Negative_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Printer_ID = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Printer_ID);
    }

    #endregion

    #region Print Time Tests

    [Fact]
    public void PrintTime_Zero_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Print_Time = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Print_Time);
    }

    [Fact]
    public void PrintTime_Negative_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Print_Time = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Print_Time);
    }

    #endregion

    #region Finishing Time Tests

    [Fact]
    public void FinishingTime_Zero_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Finishing_Time = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Finishing_Time);
    }

    [Fact]
    public void FinishingTime_Negative_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Finishing_Time = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Finishing_Time);
    }

    #endregion

    #region Filament Tests

    [Fact]
    public void Filaments_Empty_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Filaments.Clear();

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Filaments);
    }

    [Fact]
    public void Filaments_InvalidFilamentId_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();

        dto.Filaments =
        [
            new NewProjectFilamentDto
            {
                Filament_ID = 0,
                Usage_G = 100
            }
        ];

        var result = _validator.TestValidate(dto);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Filaments_InvalidUsage_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();

        dto.Filaments =
        [
            new NewProjectFilamentDto
            {
                Filament_ID = 1,
                Usage_G = 0
            }
        ];

        var result = _validator.TestValidate(dto);

        Assert.False(result.IsValid);
    }

    #endregion

    #region Material Tests

    [Fact]
    public void Materials_ValidValues_ShouldNotHaveValidationErrors()
    {
        var dto = CreateValidDto();

        dto.Materials =
        [
            new NewProjectMaterialDto
            {
                Material_ID = 2,
                Usage = 4
            }
        ];

        var result = _validator.TestValidate(dto);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Materials_InvalidMaterialId_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();

        dto.Materials =
        [
            new NewProjectMaterialDto
            {
                Material_ID = 0,
                Usage = 4
            }
        ];

        var result = _validator.TestValidate(dto);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Materials_InvalidUsage_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();

        dto.Materials =
        [
            new NewProjectMaterialDto
            {
                Material_ID = 2,
                Usage = 0
            }
        ];

        var result = _validator.TestValidate(dto);

        Assert.False(result.IsValid);
    }

    #endregion

    #region Helper Methods

    private static NewProjectDto CreateValidDto()
    {
        return new NewProjectDto
        {
            Name = "Dragon Test",
            Description = "Testing project validation",

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
    }

    #endregion
}