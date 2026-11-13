using Moq;
using Xunit;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services;

namespace PrintHub.Services_Tests.Services;

public class FilamentServiceTests : IDisposable
{
    private readonly PrintHubContext _context;
    private readonly Mock<IValidator<NewFilamentDto>> _mockNewValidator;
    private readonly Mock<IValidator<UpdateFilamentDto>> _mockUpdateValidator;
    private readonly FilamentService _service;

    // Common test DTOs
    private readonly NewFilamentDto _validNewFilamentDto = new()
    {
        Brand = "Test Brand",
        Material = "PLA",
        Texture = "Matte",
        Color = "Red",
        Weight_Grams = 1000,
        Cost = 25.50m
    };

    private readonly NewFilamentDto _invalidNewFilamentDto = new()
    {
        Brand = "",
        Material = "",
        Texture = "",
        Color = "",
        Weight_Grams = -100,
        Cost = -10
    };

    private readonly UpdateFilamentDto _validUpdateFilamentDto = new()
    {
        Brand = "New Brand",
        Material = "PETG",
        Texture = "Silk",
        Color = "Blue",
        Weight_Grams = 500,
        Cost = 30.00m
    };

    private readonly UpdateFilamentDto _partialUpdateFilamentDto = new()
    {
        Brand = "New Brand",
        Material = null,
        Texture = null,
        Color = null,
        Weight_Grams = null,
        Cost = null
    };

    private readonly UpdateFilamentDto _invalidUpdateFilamentDto = new()
    {
        Brand = "",
        Cost = -50
    };

    public FilamentServiceTests()
    {
        var options = new DbContextOptionsBuilder<PrintHubContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PrintHubContext(options);
        _mockNewValidator = new Mock<IValidator<NewFilamentDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdateFilamentDto>>();

        _service = new FilamentService(_context, _mockNewValidator.Object, _mockUpdateValidator.Object);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    #region CreateFilamentAsync Tests

    [Fact]
    public async Task CreateFilamentAsync_WithValidDto_ReturnsFilamentDto()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(_validNewFilamentDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateFilamentAsync(_validNewFilamentDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_validNewFilamentDto.Brand, result.Brand);
        Assert.Equal(_validNewFilamentDto.Material, result.Material);
        Assert.Equal(_validNewFilamentDto.Texture, result.Texture);
        Assert.Equal(_validNewFilamentDto.Color, result.Color);
        Assert.Equal(_validNewFilamentDto.Weight_Grams, result.Weight_Grams);
        Assert.Equal(_validNewFilamentDto.Cost, result.Cost);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task CreateFilamentAsync_WithInvalidDto_ThrowsValidationException()
    {
        // Arrange
        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Brand", "Brand is required")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);

        _mockNewValidator.Setup(v => v.ValidateAsync(_invalidNewFilamentDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.CreateFilamentAsync(_invalidNewFilamentDto));
    }

    [Fact]
    public async Task CreateFilamentAsync_SetsCreatedFields()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(_validNewFilamentDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateFilamentAsync(_validNewFilamentDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);

        // Verify it was persisted
        var retrieved = await _service.GetByIdAsync(result.Id);
        Assert.NotNull(retrieved);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsFilamentDto()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewFilamentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var created = await _service.CreateFilamentAsync(_validNewFilamentDto);

        // Act
        var result = await _service.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.Brand, result.Brand);
        Assert.Equal(created.Material, result.Material);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _service.GetByIdAsync(9999);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ReturnsAllFilaments()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewFilamentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var dto1 = new NewFilamentDto
        {
            Brand = "Brand 1",
            Material = "PLA",
            Texture = "Matte",
            Color = "Red",
            Weight_Grams = 1000,
            Cost = 20.00m
        };

        var dto2 = new NewFilamentDto
        {
            Brand = "Brand 2",
            Material = "PETG",
            Texture = "Silk",
            Color = "Blue",
            Weight_Grams = 1000,
            Cost = 30.00m
        };

        await _service.CreateFilamentAsync(dto1);
        await _service.CreateFilamentAsync(dto2);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Brand == "Brand 1");
        Assert.Contains(result, f => f.Brand == "Brand 2");
    }

    [Fact]
    public async Task GetAllAsync_WithNoFilaments_ReturnsEmptyList()
    {
        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region UpdateFilamentAsync Tests

    [Fact]
    public async Task UpdateFilamentAsync_WithValidDto_ReturnsUpdatedFilamentDto()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewFilamentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var newFilamentDto = new NewFilamentDto
        {
            Brand = "Old Brand",
            Material = "PLA",
            Texture = "Matte",
            Color = "Red",
            Weight_Grams = 1000,
            Cost = 20.00m
        };

        var created = await _service.CreateFilamentAsync(newFilamentDto);

        var updateValidationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateFilamentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateValidationResult);

        // Act
        var result = await _service.UpdateFilamentAsync(created.Id, _validUpdateFilamentDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_validUpdateFilamentDto.Brand, result.Brand);
        Assert.Equal(_validUpdateFilamentDto.Material, result.Material);
        Assert.Equal(_validUpdateFilamentDto.Texture, result.Texture);
        Assert.Equal(_validUpdateFilamentDto.Color, result.Color);
        Assert.Equal(_validUpdateFilamentDto.Weight_Grams, result.Weight_Grams);
        Assert.Equal(_validUpdateFilamentDto.Cost, result.Cost);
    }

    [Fact]
    public async Task UpdateFilamentAsync_WithPartialUpdate_OnlyUpdatesProvidedFields()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewFilamentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var newFilamentDto = new NewFilamentDto
        {
            Brand = "Old Brand",
            Material = "PLA",
            Texture = "Matte",
            Color = "Red",
            Weight_Grams = 1000,
            Cost = 20.00m
        };

        var created = await _service.CreateFilamentAsync(newFilamentDto);

        var updateValidationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateFilamentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateValidationResult);

        // Act
        var result = await _service.UpdateFilamentAsync(created.Id, _partialUpdateFilamentDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Brand", result.Brand);
        Assert.Equal("PLA", result.Material);  // Should remain unchanged
        Assert.Equal("Matte", result.Texture); // Should remain unchanged
        Assert.Equal("Red", result.Color);     // Should remain unchanged
    }

    [Fact]
    public async Task UpdateFilamentAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateFilamentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.UpdateFilamentAsync(9999, _validUpdateFilamentDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateFilamentAsync_WithInvalidDto_ThrowsValidationException()
    {
        // Arrange
        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Cost", "Cost must be positive")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);

        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateFilamentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.UpdateFilamentAsync(1, _invalidUpdateFilamentDto));
    }

    #endregion

    #region DeleteFilamentAsync Tests

    [Fact]
    public async Task DeleteFilamentAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewFilamentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var created = await _service.CreateFilamentAsync(_validNewFilamentDto);

        // Act
        var result = await _service.DeleteFilamentAsync(created.Id);

        // Assert
        Assert.True(result);
        var retrieved = await _service.GetByIdAsync(created.Id);
        Assert.Null(retrieved);
    }

    [Fact]
    public async Task DeleteFilamentAsync_WithInvalidId_ReturnsFalse()
    {
        // Act
        var result = await _service.DeleteFilamentAsync(9999);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region FindByLegacyIdAsync Tests

    [Fact]
    public async Task FindByLegacyIdAsync_WithValidLegacyId_ReturnsFilament()
    {
        // Arrange
        var legacyId = 123;

        // Since CreateFilamentAsync doesn't handle Legacy_Id, we add it directly
        var filament = new Filament
        {
            Brand = "Test Brand",
            Material = "PLA",
            Texture = "Matte",
            Color = "Red",
            Weight_Grams = 1000,
            Cost = 20.00m,
            Legacy_Id = legacyId,
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        _context.Filaments.Add(filament);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.FindByLegacyIdAsync(legacyId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(legacyId, result.Legacy_Id);
        Assert.Equal("Test Brand", result.Brand);
    }

    [Fact]
    public async Task FindByLegacyIdAsync_WithInvalidLegacyId_ReturnsNull()
    {
        // Act
        var result = await _service.FindByLegacyIdAsync(9999);

        // Assert
        Assert.Null(result);
    }

    #endregion
}
