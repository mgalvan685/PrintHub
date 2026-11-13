using FluentValidation.TestHelper;
using PrintHub.DTOs;
using PrintHub.Helpers;
using PrintHub.Validators;

namespace PrintHub.Validators_Tests.Validators;

public class NewFilamentDtoValidatorTests
{
    private readonly NewFilamentDtoValidator _validator = new();

    #region Happy Path Tests

    [Fact]
    public void ValidFilament_ShouldNotHaveAnyValidationErrors()
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

    #region Material Tests

    [Fact]
    public void Material_Empty_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Material = string.Empty;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Material);
    }

    [Fact]
    public void Material_TooLong_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Material = new string('A', 51);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Material);
    }

    #endregion

    #region Texture Tests

    [Fact]
    public void Texture_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Texture = FilamentTextureConstants.ALL.First();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Texture);
    }

    [Fact]
    public void Texture_InvalidValue_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Texture = "InvalidTexture";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Texture);
    }

    #endregion

    #region Color Tests

    [Fact]
    public void Color_Empty_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Color = string.Empty;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Color);
    }

    [Fact]
    public void Color_TooLong_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Color = new string('A', 51);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Color);
    }

    #endregion

    #region Weight Tests

    [Fact]
    public void WeightGrams_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Weight_Grams);
    }

    [Fact]
    public void WeightGrams_Zero_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Weight_Grams = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Weight_Grams);
    }

    [Fact]
    public void WeightGrams_Negative_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Weight_Grams = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Weight_Grams);
    }

    #endregion

    #region Cost Tests

    [Fact]
    public void Cost_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Cost);
    }

    [Fact]
    public void Cost_Zero_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Cost = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Cost);
    }

    [Fact]
    public void Cost_Negative_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Cost = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Cost);
    }

    #endregion

    #region Helper Methods

    private static NewFilamentDto CreateValidDto()
    {
        return new NewFilamentDto
        {
            Brand = "iSANMATE",
            Material = "ASA",
            Texture = FilamentTextureConstants.ALL.First(),
            Color = "White",
            Weight_Grams = 1000,
            Cost = 21.69m
        };
    }

    #endregion
}