using PrintHub.Helpers;
using PrintHub.Validators;
using Xunit;

namespace PrintHub.Validators_Tests.Validators;

public class ModelValidatorsTests
{
    #region Event Validation Tests

    [Fact]
    public void IsValidEventType_WithValidEventType_ReturnsTrue()
    {
        // Arrange
        var eventType = EventTypeConstants.ALL.First();

        // Act
        var result = ModelValidators.IsValidEventType(eventType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidEventType_WithInvalidEventType_ReturnsFalse()
    {
        // Arrange
        var eventType = "NotARealEventType";

        // Act
        var result = ModelValidators.IsValidEventType(eventType);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidEventType_WithEmptyString_ReturnsFalse()
    {
        var result = ModelValidators.IsValidEventType(string.Empty);

        Assert.False(result);
    }

    [Fact]
    public void IsValidEventType_WithNull_ReturnsFalse()
    {
        var result = ModelValidators.IsValidEventType(null!);

        Assert.False(result);
    }

    #endregion

    #region Filament Validation Tests

    [Fact]
    public void IsValidFilamentMaterial_WithValidMaterial_ReturnsTrue()
    {
        // Arrange
        var material = FilamentMaterialConstants.ALL.First();

        // Act
        var result = ModelValidators.IsValidFilamentMaterial(material);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidFilamentMaterial_WithInvalidMaterial_ReturnsFalse()
    {
        // Arrange
        var material = "InvalidMaterial";

        // Act
        var result = ModelValidators.IsValidFilamentMaterial(material);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidFilamentMaterial_WithEmptyString_ReturnsFalse()
    {
        var result = ModelValidators.IsValidFilamentMaterial(string.Empty);

        Assert.False(result);
    }

    [Fact]
    public void IsValidFilamentMaterial_WithNull_ReturnsFalse()
    {
        var result = ModelValidators.IsValidFilamentMaterial(null!);

        Assert.False(result);
    }

    #endregion

    #region Material Validation Tests

    [Fact]
    public void IsValidMaterialUnit_WithValidUnit_ReturnsTrue()
    {
        // Arrange
        var unit = MaterialUnitConstants.ALL.First();

        // Act
        var result = ModelValidators.IsValidMaterialUnit(unit);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidMaterialUnit_WithInvalidUnit_ReturnsFalse()
    {
        // Arrange
        var unit = "InvalidUnit";

        // Act
        var result = ModelValidators.IsValidMaterialUnit(unit);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMaterialUnit_WithEmptyString_ReturnsFalse()
    {
        var result = ModelValidators.IsValidMaterialUnit(string.Empty);

        Assert.False(result);
    }

    [Fact]
    public void IsValidMaterialUnit_WithNull_ReturnsFalse()
    {
        var result = ModelValidators.IsValidMaterialUnit(null!);

        Assert.False(result);
    }

    #endregion

    #region Printer Validation Tests

    [Fact]
    public void IsValidPrinterType_WithValidPrinterType_ReturnsTrue()
    {
        // Arrange
        var printerType = PrinterTypeConstants.ALL.First();

        // Act
        var result = ModelValidators.IsValidPrinterType(printerType);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidPrinterType_WithInvalidPrinterType_ReturnsFalse()
    {
        // Arrange
        var printerType = "InvalidPrinterType";

        // Act
        var result = ModelValidators.IsValidPrinterType(printerType);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValiddPrinterType_WithEmptyString_ReturnsFalse()
    {
        var result = ModelValidators.IsValidPrinterType(string.Empty);

        Assert.False(result);
    }

    [Fact]
    public void IsValiddPrinterType_WithNull_ReturnsFalse()
    {
        var result = ModelValidators.IsValidPrinterType(null!);

        Assert.False(result);
    }

    #endregion
}