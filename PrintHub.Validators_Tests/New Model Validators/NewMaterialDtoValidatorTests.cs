using FluentValidation.TestHelper;
using PrintHub.DTOs;
using PrintHub.Helpers;
using PrintHub.Validators;

namespace PrintHub.Validators_Tests.Validators;

public class NewMaterialDtoValidatorTests
{
    private readonly NewMaterialDtoValidator _validator = new();

    #region Happy Path Tests

    [Fact]
    public void ValidMaterial_ShouldNotHaveAnyValidationErrors()
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
        dto.Name = new string('A', 101);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    #endregion

    #region Initial Cost Tests

    [Fact]
    public void InitialCost_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Initial_Cost);
    }

    [Fact]
    public void InitialCost_Zero_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Initial_Cost = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Initial_Cost);
    }

    [Fact]
    public void InitialCost_Negative_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Initial_Cost = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Initial_Cost);
    }

    #endregion

    #region Units Tests

    [Fact]
    public void Units_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Units);
    }

    [Fact]
    public void Units_InvalidValue_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Units = "InvalidUnit";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Units);
    }

    #endregion

    #region Total Material Tests

    [Fact]
    public void TotalMaterial_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Total_Material);
    }

    [Fact]
    public void TotalMaterial_Zero_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Total_Material = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Total_Material);
    }

    [Fact]
    public void TotalMaterial_Negative_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Total_Material = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Total_Material);
    }

    #endregion

    #region Cost Per Unit Tests

    [Fact]
    public void CostPerUnit_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Cost_Per_Unit);
    }

    [Fact]
    public void CostPerUnit_Zero_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Cost_Per_Unit = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Cost_Per_Unit);
    }

    [Fact]
    public void CostPerUnit_Negative_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Cost_Per_Unit = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Cost_Per_Unit);
    }

    #endregion

    #region Source Tests

    [Fact]
    public void Source_Null_ShouldNotHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Source = null;

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Source);
    }

    [Fact]
    public void Source_TooLong_ShouldHaveValidationError()
    {
        var dto = CreateValidDto();
        dto.Source = new string('A', 513);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Source);
    }

    #endregion

    #region Helper Methods

    private static NewMaterialDto CreateValidDto()
    {
        return new NewMaterialDto
        {
            Name = "Stainless Steel Keychain",
            Initial_Cost = 99.90m,
            Units = MaterialUnitConstants.ALL.First(),
            Total_Material = 50,
            Cost_Per_Unit = 1.998m,
            Source = "Amazon"
        };
    }

    #endregion
}