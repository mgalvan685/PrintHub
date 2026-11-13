using Moq;
using Xunit;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.DTOs;
using PrintHub.Services;

namespace PrintHub.Services_Tests.Services;

public class PrinterServiceTests : IDisposable
{
    private readonly PrintHubContext _context;
    private readonly Mock<IValidator<NewPrinterDto>> _mockNewValidator;
    private readonly Mock<IValidator<UpdatePrinterDto>> _mockUpdateValidator;
    private readonly PrinterService _service;

    // Common test DTOs
    private readonly NewPrinterDto _validNewPrinterDto = new()
    {
        Brand = "Prusa",
        Type = "FDM",
        Name = "Prusa i3 MK3S+",
        Power_Per_Hour = 0.5m
    };

    private readonly NewPrinterDto _invalidNewPrinterDto = new()
    {
        Brand = "",
        Type = "",
        Name = "",
        Power_Per_Hour = -5m
    };

    private readonly UpdatePrinterDto _validUpdatePrinterDto = new()
    {
        Brand = "Ultimaker",
        Type = "FDM",
        Name = "Ultimaker S5",
        Power_Per_Hour = 0.8m
    };

    private readonly UpdatePrinterDto _partialUpdatePrinterDto = new()
    {
        Brand = "Formlabs",
        Type = null,
        Name = null,
        Power_Per_Hour = null
    };

    private readonly UpdatePrinterDto _invalidUpdatePrinterDto = new()
    {
        Brand = "",
        Power_Per_Hour = -10m
    };

    public PrinterServiceTests()
    {
        var options = new DbContextOptionsBuilder<PrintHubContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PrintHubContext(options);
        _mockNewValidator = new Mock<IValidator<NewPrinterDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdatePrinterDto>>();

        _service = new PrinterService(_context, _mockNewValidator.Object, _mockUpdateValidator.Object);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    #region CreatePrinterAsync Tests

    [Fact]
    public async Task CreatePrinterAsync_WithValidDto_ReturnsPrinterModel()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(_validNewPrinterDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreatePrinterAsync(_validNewPrinterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_validNewPrinterDto.Brand, result.Brand);
        Assert.Equal(_validNewPrinterDto.Type, result.Type);
        Assert.Equal(_validNewPrinterDto.Name, result.Name);
        Assert.Equal(_validNewPrinterDto.Power_Per_Hour, result.Power_Per_Hour);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task CreatePrinterAsync_WithInvalidDto_ThrowsValidationException()
    {
        // Arrange
        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Brand", "Brand is required")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);

        _mockNewValidator.Setup(v => v.ValidateAsync(_invalidNewPrinterDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.CreatePrinterAsync(_invalidNewPrinterDto));
    }

    [Fact]
    public async Task CreatePrinterAsync_SetsCreatedFields()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(_validNewPrinterDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreatePrinterAsync(_validNewPrinterDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);

        // Verify it was persisted
        var retrieved = await _service.GetPrinterByIdAsync(result.Id);
        Assert.NotNull(retrieved);
    }

    #endregion

    #region GetPrinterByIdAsync Tests

    [Fact]
    public async Task GetPrinterByIdAsync_WithValidId_ReturnsPrinterDto()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewPrinterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var created = await _service.CreatePrinterAsync(_validNewPrinterDto);

        // Act
        var result = await _service.GetPrinterByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.Brand, result.Brand);
        Assert.Equal(created.Type, result.Type);
    }

    [Fact]
    public async Task GetPrinterByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _service.GetPrinterByIdAsync(9999);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetPrintersAsync Tests

    [Fact]
    public async Task GetPrintersAsync_ReturnsAllPrinters()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewPrinterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var dto1 = new NewPrinterDto
        {
            Brand = "Prusa",
            Type = "FDM",
            Name = "Prusa i3 MK3S+",
            Power_Per_Hour = 0.5m
        };

        var dto2 = new NewPrinterDto
        {
            Brand = "Formlabs",
            Type = "SLA",
            Name = "Form 3",
            Power_Per_Hour = 1.2m
        };

        await _service.CreatePrinterAsync(dto1);
        await _service.CreatePrinterAsync(dto2);

        // Act
        var result = await _service.GetPrintersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Brand == "Prusa");
        Assert.Contains(result, p => p.Brand == "Formlabs");
    }

    [Fact]
    public async Task GetPrintersAsync_WithNoPrinters_ReturnsEmptyList()
    {
        // Act
        var result = await _service.GetPrintersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region UpdatePrinterAsync Tests

    [Fact]
    public async Task UpdatePrinterAsync_WithValidDto_ReturnsUpdatedPrinterDto()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewPrinterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var newPrinterDto = new NewPrinterDto
        {
            Brand = "Old Brand",
            Type = "FDM",
            Name = "Old Printer",
            Power_Per_Hour = 0.5m
        };

        var created = await _service.CreatePrinterAsync(newPrinterDto);

        var updateValidationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdatePrinterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateValidationResult);

        // Act
        var result = await _service.UpdatePrinterAsync(created.Id, _validUpdatePrinterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_validUpdatePrinterDto.Brand, result.Brand);
        Assert.Equal(_validUpdatePrinterDto.Type, result.Type);
        Assert.Equal(_validUpdatePrinterDto.Name, result.Name);
        Assert.Equal(_validUpdatePrinterDto.Power_Per_Hour, result.Power_Per_Hour);
    }

    [Fact]
    public async Task UpdatePrinterAsync_WithPartialUpdate_OnlyUpdatesProvidedFields()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewPrinterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var newPrinterDto = new NewPrinterDto
        {
            Brand = "Old Brand",
            Type = "FDM",
            Name = "Old Printer",
            Power_Per_Hour = 0.5m
        };

        var created = await _service.CreatePrinterAsync(newPrinterDto);

        var updateValidationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdatePrinterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateValidationResult);

        // Act
        var result = await _service.UpdatePrinterAsync(created.Id, _partialUpdatePrinterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Formlabs", result.Brand);
        Assert.Equal("FDM", result.Type);         // Should remain unchanged
        Assert.Equal("Old Printer", result.Name); // Should remain unchanged
        Assert.Equal(0.5m, result.Power_Per_Hour); // Should remain unchanged
    }

    [Fact]
    public async Task UpdatePrinterAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdatePrinterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.UpdatePrinterAsync(9999, _validUpdatePrinterDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdatePrinterAsync_WithInvalidDto_ThrowsValidationException()
    {
        // Arrange
        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Power_Per_Hour", "Power per hour must be positive")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);

        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdatePrinterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.UpdatePrinterAsync(1, _invalidUpdatePrinterDto));
    }

    #endregion

    #region DeletePrinterAsync Tests

    [Fact]
    public async Task DeletePrinterAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewPrinterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var created = await _service.CreatePrinterAsync(_validNewPrinterDto);

        // Act
        var result = await _service.DeletePrinterAsync(created.Id);

        // Assert
        Assert.True(result);
        var retrieved = await _service.GetPrinterByIdAsync(created.Id);
        Assert.Null(retrieved);
    }

    [Fact]
    public async Task DeletePrinterAsync_WithInvalidId_ReturnsFalse()
    {
        // Act
        var result = await _service.DeletePrinterAsync(9999);

        // Assert
        Assert.False(result);
    }

    #endregion
}
