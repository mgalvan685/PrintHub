using FluentValidation.TestHelper;
using PrintHub.DTOs;
using PrintHub.Helpers;
using PrintHub.Validators;

namespace PrintHub.Validators_Tests.Validators;

public class UpdateFilamentDtoValidatorTests
{
    private readonly UpdateFilamentDtoValidator _validator = new();

    #region Happy Path Tests

    [Fact]
    public void ValidFilamentUpdate_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var dto = new UpdateFilamentDto
        {
            Brand = "iSANMATE",
            Material = FilamentMaterialConstants.ALL.First(),
            Color = "White",
            Weight_Grams = 1000,
            Cost = 21.69m
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Emtpy Update Tests

    [Fact]
    public void EmptyUpdate_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var dto = new UpdateFilamentDto();

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Brand Tests

    [Fact]
    public void Brand_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Brand = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Brand);
    }

    [Fact]
    public void Brand_TooLong_ShouldHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Brand = new string('A', 51)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Brand);
    }

    #endregion

    #region Material Tests

    [Fact]
    public void Material_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Material = FilamentMaterialConstants.ALL.First()
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Material);
    }

    [Fact]
    public void Material_InvalidValue_ShouldHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Material = "NotARealMaterial"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Material);
    }

    [Fact]
    public void Material_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Material = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Material);
    }

    #endregion

    #region Color Tests

    [Fact]
    public void Color_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Color = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Color);
    }

    [Fact]
    public void Color_TooLong_ShouldHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Color = new string('A', 51)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Color);
    }

    #endregion

    #region Weight Tests

    [Fact]
    public void WeightGrams_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Weight_Grams = 1000
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Weight_Grams);
    }

    [Fact]
    public void WeightGrams_Zero_ShouldHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Weight_Grams = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Weight_Grams);
    }

    [Fact]
    public void WeightGrams_Negative_ShouldHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Weight_Grams = -100
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Weight_Grams);
    }

    [Fact]
    public void WeightGrams_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Weight_Grams = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Weight_Grams);
    }

    #endregion

    #region Cost Tests

    [Fact]
    public void Cost_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Cost = 19.99m
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Cost);
    }

    [Fact]
    public void Cost_Zero_ShouldHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Cost = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Cost);
    }

    [Fact]
    public void Cost_Negative_ShouldHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Cost = -1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Cost);
    }

    [Fact]
    public void Cost_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateFilamentDto
        {
            Cost = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Cost);
    }

    #endregion
}