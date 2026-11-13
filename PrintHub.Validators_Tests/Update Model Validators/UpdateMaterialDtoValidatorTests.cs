using FluentValidation.TestHelper;
using PrintHub.DTOs;
using PrintHub.Helpers;
using PrintHub.Validators;

namespace PrintHub.Validators_Tests.Validators;

public class UpdateMaterialDtoValidatorTests
{
    private readonly UpdateMaterialDtoValidator _validator = new();

    #region Happy Path Tests

    [Fact]
    public void ValidMaterialUpdate_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var dto = new UpdateMaterialDto
        {
            Name = "Stainless Steel Keychain",

            Initial_Cost = 99.90m,

            Units = MaterialUnitConstants.ALL.First(),

            Total_Material = 50,

            Cost_Per_Unit = 1.998m,

            Source = "Amazon"
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
        var dto = new UpdateMaterialDto();

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Name Tests

    [Fact]
    public void Name_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Name = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Name_TooLong_ShouldHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Name = new string('A', 101)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    #endregion

    #region Initial Cost Tests

    [Fact]
    public void InitialCost_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Initial_Cost = 10.99m
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Initial_Cost);
    }

    [Fact]
    public void InitialCost_Zero_ShouldHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Initial_Cost = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Initial_Cost);
    }

    [Fact]
    public void InitialCost_Negative_ShouldHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Initial_Cost = -1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Initial_Cost);
    }

    [Fact]
    public void InitialCost_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Initial_Cost = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Initial_Cost);
    }

    #endregion

    #region Units Tests

    [Fact]
    public void Units_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Units = MaterialUnitConstants.ALL.First()
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Units);
    }

    [Fact]
    public void Units_InvalidValue_ShouldHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Units = "InvalidUnit"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Units);
    }

    [Fact]
    public void Units_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Units = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Units);
    }

    #endregion

    #region Total Material Tests

    [Fact]
    public void TotalMaterial_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Total_Material = 100
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Total_Material);
    }

    [Fact]
    public void TotalMaterial_Zero_ShouldHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Total_Material = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Total_Material);
    }

    [Fact]
    public void TotalMaterial_Negative_ShouldHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Total_Material = -1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Total_Material);
    }

    [Fact]
    public void TotalMaterial_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Total_Material = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Total_Material);
    }

    #endregion

    #region Cost Per Unit Tests

    [Fact]
    public void CostPerUnit_ValidValue_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Cost_Per_Unit = 0.50m
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Cost_Per_Unit);
    }

    [Fact]
    public void CostPerUnit_Zero_ShouldHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Cost_Per_Unit = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Cost_Per_Unit);
    }

    [Fact]
    public void CostPerUnit_Negative_ShouldHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Cost_Per_Unit = -1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Cost_Per_Unit);
    }

    [Fact]
    public void CostPerUnit_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Cost_Per_Unit = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Cost_Per_Unit);
    }

    #endregion

    #region Source Tests

    [Fact]
    public void Source_Null_ShouldNotHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Source = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Source);
    }

    [Fact]
    public void Source_TooLong_ShouldHaveValidationError()
    {
        var dto = new UpdateMaterialDto
        {
            Source = new string('A', 513)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Source);
    }

    #endregion
}