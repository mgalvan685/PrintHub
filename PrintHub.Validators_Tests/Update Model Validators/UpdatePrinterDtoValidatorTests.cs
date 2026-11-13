using FluentValidation.TestHelper;
using PrintHub.DTOs;
using PrintHub.Helpers;
using PrintHub.Validators;

namespace PrintHub.Validators_Tests.Validators;

public class UpdatePrinterDtoValidatorTests
{
    private readonly UpdatePrinterDtoValidator _validator = new();

    #region Happy Path Tests

    [Fact]
    public void ValidPrinterUpdate_ShouldNotHaveAnyValidationErrors()
    {
        var dto = new UpdatePrinterDto
        {
            Brand = "Bambu Lab",
            Type = PrinterTypeConstants.ALL.First(),
            Name = "H2D",
            Power_Per_Hour = 0.35m
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Empty Update Tests

    [Fact]
    public void EmptyUpdate_ShouldNotHaveAnyValidationErrors()
    {
        var dto = new UpdatePrinterDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Brand Tests

    [Fact]
    public void Brand_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Brand = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Brand);
    }

    [Fact]
    public void Brand_TooLong_ShouldHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Brand = new string('A', 51)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    #endregion

    #region Type Tests

    [Fact]
    public void Type_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Type = PrinterTypeConstants.ALL.First()
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Type);
    }

    [Fact]
    public void Type_InvalidValue_ShouldHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Type = "InvalidPrinterType"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Type);
    }

    [Fact]
    public void Type_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Type = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Type);
    }

    #endregion

    #region Name Tests

    [Fact]
    public void Name_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Name = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Name_TooLong_ShouldHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Name = new string('A', 101)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    #endregion

    #region Power Per Hour Tests

    [Fact]
    public void PowerPerHour_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Power_Per_Hour = 0.35m
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Power_Per_Hour);
    }

    [Fact]
    public void PowerPerHour_Zero_ShouldHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Power_Per_Hour = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Power_Per_Hour);
    }

    [Fact]
    public void PowerPerHour_Negative_ShouldHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Power_Per_Hour = -1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Power_Per_Hour);
    }

    [Fact]
    public void PowerPerHour_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdatePrinterDto
        {
            Power_Per_Hour = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Power_Per_Hour);
    }

    #endregion

}