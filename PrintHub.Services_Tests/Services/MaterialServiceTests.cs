using Moq;
using Xunit;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services;

namespace PrintHub.Services_Tests.Services;

public class MaterialServiceTests : IDisposable
{
    private readonly PrintHubContext _context;
    private readonly Mock<IValidator<NewMaterialDto>> _mockNewValidator;
    private readonly Mock<IValidator<UpdateMaterialDto>> _mockUpdateValidator;
    private readonly MaterialService _service;

    // Common test DTOs
    private readonly NewMaterialDto _validNewMaterialDto = new()
    {
        Name = "Epoxy Resin",
        Initial_Cost = 50.00m,
        Units = "Liters",
        Total_Material = 1.0m,
        Cost_Per_Unit = 50.00m,
        Source = "Online Supplier"
    };

    private readonly NewMaterialDto _invalidNewMaterialDto = new()
    {
        Name = "",
        Initial_Cost = -25.00m,
        Units = "",
        Total_Material = -1.0m,
        Cost_Per_Unit = -50.00m,
        Source = ""
    };

    private readonly UpdateMaterialDto _validUpdateMaterialDto = new()
    {
        Name = "UV Epoxy Resin",
        Initial_Cost = 55.00m,
        Units = "Liters",
        Total_Material = 1.5m,
        Cost_Per_Unit = 55.00m,
        Source = "New Supplier"
    };

    private readonly UpdateMaterialDto _partialUpdateMaterialDto = new()
    {
        Name = "UV Epoxy Resin",
        Initial_Cost = null,
        Units = null,
        Total_Material = null,
        Cost_Per_Unit = null,
        Source = null
    };

    private readonly UpdateMaterialDto _invalidUpdateMaterialDto = new()
    {
        Name = "",
        Initial_Cost = -100.00m,
        Cost_Per_Unit = -50.00m
    };

    public MaterialServiceTests()
    {
        var options = new DbContextOptionsBuilder<PrintHubContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PrintHubContext(options);
        _mockNewValidator = new Mock<IValidator<NewMaterialDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdateMaterialDto>>();

        _service = new MaterialService(_context, _mockNewValidator.Object, _mockUpdateValidator.Object);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    #region CreateMaterialAsync Tests

    [Fact]
    public async Task CreateMaterialAsync_WithValidDto_ReturnsMaterialDto()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(_validNewMaterialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateMaterialAsync(_validNewMaterialDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_validNewMaterialDto.Name, result.Name);
        Assert.Equal(_validNewMaterialDto.Initial_Cost, result.Initial_Cost);
        Assert.Equal(_validNewMaterialDto.Units, result.Units);
        Assert.Equal(_validNewMaterialDto.Total_Material, result.Total_Material);
        Assert.Equal(_validNewMaterialDto.Cost_Per_Unit, result.Cost_Per_Unit);
        Assert.Equal(_validNewMaterialDto.Source, result.Source);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task CreateMaterialAsync_WithInvalidDto_ThrowsValidationException()
    {
        // Arrange
        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Name", "Name is required"),
            new FluentValidation.Results.ValidationFailure("Initial_Cost", "Initial cost must be positive")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);

        _mockNewValidator.Setup(v => v.ValidateAsync(_invalidNewMaterialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.CreateMaterialAsync(_invalidNewMaterialDto));
    }

    [Fact]
    public async Task CreateMaterialAsync_SetsCreatedFields()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(_validNewMaterialDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateMaterialAsync(_validNewMaterialDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);

        // Verify it was persisted
        var retrieved = await _service.GetByIdAsync(result.Id);
        Assert.NotNull(retrieved);
    }

    [Fact]
    public async Task CreateMaterialAsync_WithMultipleValidDtos_CreatesMultipleMaterials()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var dto1 = new NewMaterialDto
        {
            Name = "Resin Type 1",
            Initial_Cost = 40.00m,
            Units = "Liters",
            Total_Material = 1.0m,
            Cost_Per_Unit = 40.00m,
            Source = "Supplier A"
        };

        var dto2 = new NewMaterialDto
        {
            Name = "Resin Type 2",
            Initial_Cost = 60.00m,
            Units = "Kilograms",
            Total_Material = 2.0m,
            Cost_Per_Unit = 30.00m,
            Source = "Supplier B"
        };

        // Act
        var result1 = await _service.CreateMaterialAsync(dto1);
        var result2 = await _service.CreateMaterialAsync(dto2);

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.NotEqual(result1.Id, result2.Id);
        Assert.Equal("Resin Type 1", result1.Name);
        Assert.Equal("Resin Type 2", result2.Name);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsMaterialDto()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var created = await _service.CreateMaterialAsync(_validNewMaterialDto);

        // Act
        var result = await _service.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.Name, result.Name);
        Assert.Equal(created.Initial_Cost, result.Initial_Cost);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _service.GetByIdAsync(9999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithZeroId_ReturnsNull()
    {
        // Act
        var result = await _service.GetByIdAsync(0);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ReturnsAllMaterials()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var dto1 = new NewMaterialDto
        {
            Name = "Material 1",
            Initial_Cost = 30.00m,
            Units = "Liters",
            Total_Material = 1.0m,
            Cost_Per_Unit = 30.00m,
            Source = "Supplier A"
        };

        var dto2 = new NewMaterialDto
        {
            Name = "Material 2",
            Initial_Cost = 40.00m,
            Units = "Kilograms",
            Total_Material = 2.0m,
            Cost_Per_Unit = 20.00m,
            Source = "Supplier B"
        };

        var dto3 = new NewMaterialDto
        {
            Name = "Material 3",
            Initial_Cost = 50.00m,
            Units = "Units",
            Total_Material = 3.0m,
            Cost_Per_Unit = 16.67m,
            Source = "Supplier C"
        };

        await _service.CreateMaterialAsync(dto1);
        await _service.CreateMaterialAsync(dto2);
        await _service.CreateMaterialAsync(dto3);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains(result, m => m.Name == "Material 1");
        Assert.Contains(result, m => m.Name == "Material 2");
        Assert.Contains(result, m => m.Name == "Material 3");
    }

    [Fact]
    public async Task GetAllAsync_WithNoMaterials_ReturnsEmptyList()
    {
        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_DoesNotModifyState()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        await _service.CreateMaterialAsync(_validNewMaterialDto);

        // Act
        var firstCall = await _service.GetAllAsync();
        var secondCall = await _service.GetAllAsync();

        // Assert
        Assert.Equal(firstCall.Count, secondCall.Count);
    }

    #endregion

    #region UpdateMaterialAsync Tests

    [Fact]
    public async Task UpdateMaterialAsync_WithValidDto_ReturnsUpdatedMaterialDto()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var newMaterialDto = new NewMaterialDto
        {
            Name = "Old Material",
            Initial_Cost = 20.00m,
            Units = "Liters",
            Total_Material = 1.0m,
            Cost_Per_Unit = 20.00m,
            Source = "Old Supplier"
        };

        var created = await _service.CreateMaterialAsync(newMaterialDto);

        var updateValidationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateValidationResult);

        // Act
        var result = await _service.UpdateMaterialAsync(created.Id, _validUpdateMaterialDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_validUpdateMaterialDto.Name, result.Name);
        Assert.Equal(_validUpdateMaterialDto.Initial_Cost, result.Initial_Cost);
        Assert.Equal(_validUpdateMaterialDto.Units, result.Units);
        Assert.Equal(_validUpdateMaterialDto.Total_Material, result.Total_Material);
        Assert.Equal(_validUpdateMaterialDto.Cost_Per_Unit, result.Cost_Per_Unit);
        Assert.Equal(_validUpdateMaterialDto.Source, result.Source);
    }

    [Fact]
    public async Task UpdateMaterialAsync_WithPartialUpdate_OnlyUpdatesProvidedFields()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var newMaterialDto = new NewMaterialDto
        {
            Name = "Original Material",
            Initial_Cost = 25.00m,
            Units = "Liters",
            Total_Material = 1.5m,
            Cost_Per_Unit = 16.67m,
            Source = "Original Source"
        };

        var created = await _service.CreateMaterialAsync(newMaterialDto);

        var updateValidationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateValidationResult);

        // Act
        var result = await _service.UpdateMaterialAsync(created.Id, _partialUpdateMaterialDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UV Epoxy Resin", result.Name);           // Should be updated
        Assert.Equal(25.00m, result.Initial_Cost);             // Should remain unchanged
        Assert.Equal("Liters", result.Units);                  // Should remain unchanged
        Assert.Equal(1.5m, result.Total_Material);             // Should remain unchanged
        Assert.Equal(16.67m, result.Cost_Per_Unit);            // Should remain unchanged
        Assert.Equal("Original Source", result.Source);        // Should remain unchanged
    }

    [Fact]
    public async Task UpdateMaterialAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.UpdateMaterialAsync(9999, _validUpdateMaterialDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateMaterialAsync_WithInvalidDto_ThrowsValidationException()
    {
        // Arrange
        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Initial_Cost", "Initial cost must be positive")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);

        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.UpdateMaterialAsync(1, _invalidUpdateMaterialDto));
    }

    [Fact]
    public async Task UpdateMaterialAsync_PersistsChanges()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var created = await _service.CreateMaterialAsync(_validNewMaterialDto);

        var updateValidationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateValidationResult);

        // Act
        await _service.UpdateMaterialAsync(created.Id, _validUpdateMaterialDto);

        // Assert
        var retrieved = await _service.GetByIdAsync(created.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(_validUpdateMaterialDto.Name, retrieved.Name);
    }

    #endregion

    #region DeleteMaterialAsync Tests

    [Fact]
    public async Task DeleteMaterialAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var created = await _service.CreateMaterialAsync(_validNewMaterialDto);

        // Act
        var result = await _service.DeleteMaterialAsync(created.Id);

        // Assert
        Assert.True(result);
        var retrieved = await _service.GetByIdAsync(created.Id);
        Assert.Null(retrieved);
    }

    [Fact]
    public async Task DeleteMaterialAsync_WithInvalidId_ReturnsFalse()
    {
        // Act
        var result = await _service.DeleteMaterialAsync(9999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteMaterialAsync_RemovesFromDatabase()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewMaterialDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var created = await _service.CreateMaterialAsync(_validNewMaterialDto);
        var allBefore = await _service.GetAllAsync();

        // Act
        await _service.DeleteMaterialAsync(created.Id);

        // Assert
        var allAfter = await _service.GetAllAsync();
        Assert.Equal(allBefore.Count - 1, allAfter.Count);
        Assert.DoesNotContain(allAfter, m => m.Id == created.Id);
    }

    [Fact]
    public async Task DeleteMaterialAsync_WithZeroId_ReturnsFalse()
    {
        // Act
        var result = await _service.DeleteMaterialAsync(0);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region FindByLegacyIdAsync Tests

    [Fact]
    public async Task FindByLegacyIdAsync_WithValidLegacyId_ReturnsMaterial()
    {
        // Arrange
        var legacyId = 456;

        var material = new Material
        {
            Name = "Legacy Material",
            Initial_Cost = 35.00m,
            Units = "Liters",
            Total_Material = 1.0m,
            Cost_Per_Unit = 35.00m,
            Source = "Legacy Source",
            Legacy_Id = legacyId,
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        _context.Materials.Add(material);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.FindByLegacyIdAsync(legacyId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(legacyId, result.Legacy_Id);
        Assert.Equal("Legacy Material", result.Name);
    }

    [Fact]
    public async Task FindByLegacyIdAsync_WithInvalidLegacyId_ReturnsNull()
    {
        // Act
        var result = await _service.FindByLegacyIdAsync(9999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindByLegacyIdAsync_WithMultipleMaterials_ReturnsCorrectOne()
    {
        // Arrange
        var legacyId1 = 100;
        var legacyId2 = 200;

        var material1 = new Material
        {
            Name = "Material 1",
            Initial_Cost = 20.00m,
            Units = "Liters",
            Total_Material = 1.0m,
            Cost_Per_Unit = 20.00m,
            Source = "Source 1",
            Legacy_Id = legacyId1,
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        var material2 = new Material
        {
            Name = "Material 2",
            Initial_Cost = 30.00m,
            Units = "Kilograms",
            Total_Material = 2.0m,
            Cost_Per_Unit = 15.00m,
            Source = "Source 2",
            Legacy_Id = legacyId2,
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        _context.Materials.AddRange(material1, material2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.FindByLegacyIdAsync(legacyId2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Material 2", result.Name);
        Assert.Equal(legacyId2, result.Legacy_Id);
    }

    #endregion
}
