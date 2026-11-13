using AutoMapper;
using Moq;
using Xunit;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services;

namespace PrintHub.Services_Tests.Services;

public class ProjectServiceTests : IDisposable
{
    private readonly PrintHubContext _context;
    private readonly Mock<IValidator<NewProjectDto>> _mockNewValidator;
    private readonly Mock<IValidator<UpdateProjectDto>> _mockUpdateValidator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ProjectService _service;
    private readonly Printer _testPrinter;

    // Common test DTOs
    private readonly NewProjectDto _validNewProjectDto;
    private readonly NewProjectDto _invalidNewProjectDto = new()
    {
        Name = "",
        Description = "",
        Printer_ID = 9999, // Non-existent printer
        Print_Time = -5,
        Finishing_Time = -10
    };

    private readonly UpdateProjectDto _validUpdateProjectDto = new()
    {
        Name = "Updated Project",
        Description = "Updated Description",
        Print_Time = 3.5m,
        Finishing_Time = 1.5m
    };

    private readonly UpdateProjectDto _partialUpdateProjectDto = new()
    {
        Name = "Partial Update",
        Description = null,
        Printer_ID = null,
        Print_Time = null,
        Finishing_Time = null
    };

    public ProjectServiceTests()
    {
        var options = new DbContextOptionsBuilder<PrintHubContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PrintHubContext(options);

        // Setup mapper with mocking
        _mockMapper = new Mock<IMapper>();

        // Create a test printer
        _testPrinter = new Printer
        {
            Brand = "Test Brand",
            Type = "FDM",
            Name = "Test Printer",
            Power_Per_Hour = 0.5m,
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        _context.Printers.Add(_testPrinter);
        _context.SaveChanges();

        // Setup valid DTO with existing printer
        _validNewProjectDto = new()
        {
            Name = "Test Project",
            Description = "Test Description",
            Printer_ID = _testPrinter.Id,
            Print_Time = 2.5m,
            Finishing_Time = 1.0m,
            Filaments = new List<NewProjectFilamentDto>(),
            Materials = new List<NewProjectMaterialDto>()
        };

        _mockNewValidator = new Mock<IValidator<NewProjectDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdateProjectDto>>();

        // Setup mapper to return proper DTOs
        _mockMapper.Setup(m => m.Map<ProjectDto>(It.IsAny<Project>()))
            .Returns((Project project) => new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Printer_ID = project.Printer_ID,
                Printer_Name = project.Printer?.Name ?? "",
                Print_Time = project.Print_Time,
                Finishing_Time = project.Finishing_Time,
                Filaments = project.ProjectFilaments?.Select(pf => new ProjectFilamentDto
                {
                    Filament_ID = pf.Filament_ID,
                    Brand = pf.Filament.Brand,
                    Material = pf.Filament.Material,
                    Texture = pf.Filament.Texture,
                    Color = pf.Filament.Color,
                    Usage_G = pf.Usage_G
                }).ToList() ?? new List<ProjectFilamentDto>(),
                Materials = project.ProjectMaterials?.Select(pm => new ProjectMaterialDto
                {
                    Material_ID = pm.Material_ID,
                    Name = pm.Material.Name,
                    Units = pm.Units,
                    Usage = pm.Usage
                }).ToList() ?? new List<ProjectMaterialDto>()
            });

        _mockMapper.Setup(m => m.Map<List<ProjectDto>>(It.IsAny<List<Project>>()))
            .Returns((List<Project> projects) => projects.Select(project => new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Printer_ID = project.Printer_ID,
                Printer_Name = project.Printer?.Name ?? "",
                Print_Time = project.Print_Time,
                Finishing_Time = project.Finishing_Time,
                Filaments = project.ProjectFilaments?.Select(pf => new ProjectFilamentDto
                {
                    Filament_ID = pf.Filament_ID,
                    Brand = pf.Filament.Brand,
                    Material = pf.Filament.Material,
                    Texture = pf.Filament.Texture,
                    Color = pf.Filament.Color,
                    Usage_G = pf.Usage_G
                }).ToList() ?? new List<ProjectFilamentDto>(),
                Materials = project.ProjectMaterials?.Select(pm => new ProjectMaterialDto
                {
                    Material_ID = pm.Material_ID,
                    Name = pm.Material.Name,
                    Units = pm.Units,
                    Usage = pm.Usage
                }).ToList() ?? new List<ProjectMaterialDto>()
            }).ToList());

        _service = new ProjectService(_context, _mockMapper.Object, _mockNewValidator.Object, _mockUpdateValidator.Object);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    #region CreateProjectAsync Tests

    [Fact]
    public async Task CreateProjectAsync_WithValidDto_ReturnsProjectDto()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(_validNewProjectDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateProjectAsync(_validNewProjectDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_validNewProjectDto.Name, result.Name);
        Assert.Equal(_validNewProjectDto.Description, result.Description);
        Assert.Equal(_validNewProjectDto.Printer_ID, result.Printer_ID);
        Assert.Equal(_validNewProjectDto.Print_Time, result.Print_Time);
        Assert.Equal(_validNewProjectDto.Finishing_Time, result.Finishing_Time);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task CreateProjectAsync_WithInvalidDto_ThrowsValidationException()
    {
        // Arrange
        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Name", "Name is required")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);

        _mockNewValidator.Setup(v => v.ValidateAsync(_invalidNewProjectDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.CreateProjectAsync(_invalidNewProjectDto));
    }

    [Fact]
    public async Task CreateProjectAsync_WithNonExistentPrinter_ThrowsValidationException()
    {
        // Arrange
        var dtoWithInvalidPrinter = new NewProjectDto
        {
            Name = "Test Project",
            Description = "Test Description",
            Printer_ID = 9999, // Non-existent printer
            Print_Time = 2.5m,
            Finishing_Time = 1.0m,
            Filaments = new List<NewProjectFilamentDto>(),
            Materials = new List<NewProjectMaterialDto>()
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(dtoWithInvalidPrinter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.CreateProjectAsync(dtoWithInvalidPrinter));
    }

    [Fact]
    public async Task CreateProjectAsync_SetsCreatedFields()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(_validNewProjectDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateProjectAsync(_validNewProjectDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);

        // Verify it was persisted
        var retrieved = await _service.GetByIdAsync(result.Id);
        Assert.NotNull(retrieved);
    }

    [Fact]
    public async Task CreateProjectAsync_WithLegacyId_SetsProperly()
    {
        // Arrange
        var legacyId = 12345;
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(_validNewProjectDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateProjectAsync(_validNewProjectDto, legacyId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);

        // Verify legacy ID was set
        var dbProject = await _context.Projects.FirstOrDefaultAsync(p => p.Id == result.Id);
        Assert.NotNull(dbProject);
        Assert.Equal(legacyId, dbProject.Legacy_Id);
    }

    [Fact]
    public async Task CreateProjectAsync_WithFilaments_CreatesAssociations()
    {
        // Arrange
        var filament = new Filament
        {
            Brand = "Test Brand",
            Material = "PLA",
            Texture = "Matte",
            Color = "Red",
            Weight_Grams = 1000,
            Cost = 25.00m,
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };
        _context.Filaments.Add(filament);
        await _context.SaveChangesAsync();

        var dtoWithFilament = new NewProjectDto
        {
            Name = "Project With Filament",
            Description = "Test",
            Printer_ID = _testPrinter.Id,
            Print_Time = 2.5m,
            Finishing_Time = 1.0m,
            Filaments = new List<NewProjectFilamentDto>
            {
                new NewProjectFilamentDto { Filament_ID = filament.Id, Usage_G = 100 }
            },
            Materials = new List<NewProjectMaterialDto>()
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(dtoWithFilament, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateProjectAsync(dtoWithFilament);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Filaments);
        Assert.Single(result.Filaments);
        Assert.Equal(filament.Id, result.Filaments[0].Filament_ID);
    }

    [Fact]
    public async Task CreateProjectAsync_WithMaterials_CreatesAssociations()
    {
        // Arrange
        var material = new Material
        {
            Name = "Test Material",
            Initial_Cost = 50.00m,
            Units = "Liters",
            Total_Material = 1.0m,
            Cost_Per_Unit = 50.00m,
            Source = "Test Supplier",
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };
        _context.Materials.Add(material);
        await _context.SaveChangesAsync();

        var dtoWithMaterial = new NewProjectDto
        {
            Name = "Project With Material",
            Description = "Test",
            Printer_ID = _testPrinter.Id,
            Print_Time = 2.5m,
            Finishing_Time = 1.0m,
            Filaments = new List<NewProjectFilamentDto>(),
            Materials = new List<NewProjectMaterialDto>
            {
                new NewProjectMaterialDto { Material_ID = material.Id, Usage = 0.5m }
            }
        };

        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(dtoWithMaterial, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.CreateProjectAsync(dtoWithMaterial);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Materials!);
        Assert.Single(result.Materials!);
        Assert.Equal(material.Id, result.Materials![0].Material_ID);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsProjectDto()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var created = await _service.CreateProjectAsync(_validNewProjectDto);

        // Act
        var result = await _service.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.Name, result.Name);
        Assert.Equal(created.Printer_ID, result.Printer_ID);
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
    public async Task GetByIdAsync_IncludesPrinterInformation()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var created = await _service.CreateProjectAsync(_validNewProjectDto);

        // Act
        var result = await _service.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_testPrinter.Name, result.Printer_Name);
    }

    #endregion

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ReturnsAllProjects()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var dto1 = new NewProjectDto
        {
            Name = "Project 1",
            Description = "Description 1",
            Printer_ID = _testPrinter.Id,
            Print_Time = 2.0m,
            Finishing_Time = 0.5m,
            Filaments = new List<NewProjectFilamentDto>(),
            Materials = new List<NewProjectMaterialDto>()
        };

        var dto2 = new NewProjectDto
        {
            Name = "Project 2",
            Description = "Description 2",
            Printer_ID = _testPrinter.Id,
            Print_Time = 3.0m,
            Finishing_Time = 1.0m,
            Filaments = new List<NewProjectFilamentDto>(),
            Materials = new List<NewProjectMaterialDto>()
        };

        await _service.CreateProjectAsync(dto1);
        await _service.CreateProjectAsync(dto2);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "Project 1");
        Assert.Contains(result, p => p.Name == "Project 2");
    }

    [Fact]
    public async Task GetAllAsync_WithNoProjects_ReturnsEmptyList()
    {
        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region UpdateProjectAsync Tests

    [Fact]
    public async Task UpdateProjectAsync_WithValidDto_ReturnsUpdatedProjectDto()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var created = await _service.CreateProjectAsync(_validNewProjectDto);

        var updateValidationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateValidationResult);

        // Act
        var result = await _service.UpdateProjectAsync(created.Id, _validUpdateProjectDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_validUpdateProjectDto.Name, result.Name);
        Assert.Equal(_validUpdateProjectDto.Description, result.Description);
        Assert.Equal(_validUpdateProjectDto.Print_Time, result.Print_Time);
        Assert.Equal(_validUpdateProjectDto.Finishing_Time, result.Finishing_Time);
    }

    [Fact]
    public async Task UpdateProjectAsync_WithPartialUpdate_OnlyUpdatesProvidedFields()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var created = await _service.CreateProjectAsync(_validNewProjectDto);

        var updateValidationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateValidationResult);

        // Act
        var result = await _service.UpdateProjectAsync(created.Id, _partialUpdateProjectDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Partial Update", result.Name);           // Should be updated
        Assert.Equal(_validNewProjectDto.Description, result.Description);  // Should remain unchanged
        Assert.Equal(_validNewProjectDto.Print_Time, result.Print_Time);     // Should remain unchanged
    }

    [Fact]
    public async Task UpdateProjectAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _service.UpdateProjectAsync(9999, _validUpdateProjectDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateProjectAsync_WithInvalidDto_ThrowsValidationException()
    {
        // Arrange
        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new FluentValidation.Results.ValidationFailure("Name", "Name cannot be empty")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);

        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var invalidDto = new UpdateProjectDto { Name = "" };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(
            () => _service.UpdateProjectAsync(1, invalidDto));
    }

    [Fact]
    public async Task UpdateProjectAsync_PersistsChanges()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var created = await _service.CreateProjectAsync(_validNewProjectDto);

        var updateValidationResult = new FluentValidation.Results.ValidationResult();
        _mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updateValidationResult);

        // Act
        await _service.UpdateProjectAsync(created.Id, _validUpdateProjectDto);

        // Assert
        var retrieved = await _service.GetByIdAsync(created.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(_validUpdateProjectDto.Name, retrieved.Name);
    }

    #endregion

    #region DeleteProjectAsync Tests

    [Fact]
    public async Task DeleteProjectAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var created = await _service.CreateProjectAsync(_validNewProjectDto);

        // Act
        var result = await _service.DeleteProjectAsync(created.Id);

        // Assert
        Assert.True(result);
        var retrieved = await _service.GetByIdAsync(created.Id);
        Assert.Null(retrieved);
    }

    [Fact]
    public async Task DeleteProjectAsync_WithInvalidId_ReturnsFalse()
    {
        // Act
        var result = await _service.DeleteProjectAsync(9999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteProjectAsync_RemovesFromDatabase()
    {
        // Arrange
        var newValidationResult = new FluentValidation.Results.ValidationResult();
        _mockNewValidator.Setup(v => v.ValidateAsync(It.IsAny<NewProjectDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newValidationResult);

        var created = await _service.CreateProjectAsync(_validNewProjectDto);
        var allBefore = await _service.GetAllAsync();

        // Act
        await _service.DeleteProjectAsync(created.Id);

        // Assert
        var allAfter = await _service.GetAllAsync();
        Assert.Equal(allBefore.Count - 1, allAfter.Count);
        Assert.DoesNotContain(allAfter, p => p.Id == created.Id);
    }

    #endregion
}
