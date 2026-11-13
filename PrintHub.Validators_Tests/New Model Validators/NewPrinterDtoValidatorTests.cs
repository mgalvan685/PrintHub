using FluentValidation.TestHelper;
using PrintHub.DTOs;
using PrintHub.Helpers;

namespace PrintHub.Validators_Tests.Validators;

public class NewPrinterDtoValidatorTests
{
    private readonly NewPrinterDtoValidator _validator = new();

    #region Happy Path Tests

    [Fact]
    public void ValidPrinter_ShouldNotHaveAnyValidationErrors()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Brand Tests

    [Fact]
    public void Brand_Empty_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Brand = string.Empty;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    [Fact]
    public void Brand_TooLong_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Brand = new string('A', 51);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    #endregion

    #region Type Tests

    [Fact]
    public void Type_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Type);
    }

    [Fact]
    public void Type_InvalidValue_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Type = "InvalidPrinterType";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Type);
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
        dto.Name = new string('A', 101);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    #endregion

    #region Power Per Hour Tests

    [Fact]
    public void PowerPerHour_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Power_Per_Hour);
    }

    [Fact]
    public void PowerPerHour_Zero_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Power_Per_Hour = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Power_Per_Hour);
    }

    [Fact]
    public void PowerPerHour_Negative_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Power_Per_Hour = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Power_Per_Hour);
    }

    #endregion

    #region Helper Methods

    private static NewPrinterDto CreateValidDto()
    {
        return new NewPrinterDto
        {
            Brand = "Bambu Lab",
            Type = PrinterTypeConstants.ALL.First(),
            Name = "H2D",
            Power_Per_Hour = 0.35m
        };
    }

    #endregion
}