using Xunit;
using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services;

namespace PrintHub.Services_Tests.Services;

public class ProjectCostServiceTests : IDisposable
{
    private readonly PrintHubContext _context;
    private readonly ProjectCostService _service;
    private readonly Printer _testPrinter;
    private readonly GlobalSettings _testSettings;

    public ProjectCostServiceTests()
    {
        var options = new DbContextOptionsBuilder<PrintHubContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PrintHubContext(options);

        // Create test printer
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

        // Create test global settings
        _testSettings = new GlobalSettings
        {
            Default_Markup = 0.20m,           // 20% markup
            Electricity_Rate = 0.15m,         // $0.15 per kWh
            Labor_Rate = 25.00m,              // $25 per hour
            Default_Waste_Multiplier = 1.1m  // 10% waste
        };
        _context.GlobalSettings.Add(_testSettings);

        _context.SaveChanges();

        _service = new ProjectCostService(_context);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    #region Calculate Tests - Basic Cost Calculation

    [Fact]
    public void Calculate_WithEmptyProject_CalculatesCorrectly()
    {
        // Arrange
        var project = new Project
        {
            Name = "Empty Project",
            Description = "A project with no filaments or materials",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 2.0m,
            Finishing_Time = 0.5m,
            ProjectFilaments = new List<ProjectFilament>(),
            ProjectMaterials = new List<ProjectMaterial>(),
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0m, result.FilamentCost);
        Assert.Equal(0m, result.MaterialCost);
        Assert.Equal(0m, result.ModifierAdjustment);

        // Printer cost = 2.0 * 0.5 * 0.15 = 0.15
        Assert.Equal(0.15m, result.PrinterCost);

        // Labor cost = 0.5 * 25.00 = 12.50
        Assert.Equal(12.50m, result.LaborCost);

        // Total = 0 + 0 + 0.15 + 12.50 = 12.65
        Assert.Equal(12.65m, result.TotalCost);

        // Final price = 12.65 * 1.20 = 15.18
        Assert.Equal(15.18m, result.FinalPrice);
    }

    [Fact]
    public void Calculate_WithFilamentOnly_CalculatesFilamentCost()
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
        _context.SaveChanges();

        var projectFilament = new ProjectFilament
        {
            Filament_ID = filament.Id,
            Filament = filament,
            Usage_G = 100,                          // 100 grams used
            Cost_At_Time = 25.00m / 1000           // Cost per gram: $0.025
        };

        var project = new Project
        {
            Name = "Filament Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 1.0m,
            Finishing_Time = 0.5m,
            ProjectFilaments = new List<ProjectFilament> { projectFilament },
            ProjectMaterials = new List<ProjectMaterial>(),
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        // Filament cost = 100 * 0.025 = 2.50
        Assert.Equal(2.50m, result.FilamentCost);
        Assert.Equal(0m, result.MaterialCost);
        // Printer cost = 1.0 * 0.5 * 0.15 = 0.075
        Assert.Equal(0.075m, result.PrinterCost);
        // Labor cost = 0.5 * 25.00 = 12.50
        Assert.Equal(12.50m, result.LaborCost);
        // Total = 2.50 + 0 + 0.075 + 12.50 = 15.075
        Assert.Equal(15.075m, result.TotalCost);
        // Final price = 15.075 * 1.20 = 18.09
        Assert.Equal(18.09m, result.FinalPrice);
    }

    [Fact]
    public void Calculate_WithMaterialOnly_CalculatesMaterialCost()
    {
        // Arrange
        var material = new Material
        {
            Name = "Epoxy Resin",
            Initial_Cost = 50.00m,
            Units = "Liters",
            Total_Material = 1.0m,
            Cost_Per_Unit = 50.00m,
            Source = "Test Supplier",
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };
        _context.Materials.Add(material);
        _context.SaveChanges();

        var projectMaterial = new ProjectMaterial
        {
            Material_ID = material.Id,
            Material = material,
            Usage = 0.5m,                       // 0.5 liters used
            Units = "Liters",
            Cost_At_Time = 50.00m               // $50 per liter
        };

        var project = new Project
        {
            Name = "Material Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 1.0m,
            Finishing_Time = 0.5m,
            ProjectFilaments = new List<ProjectFilament>(),
            ProjectMaterials = new List<ProjectMaterial> { projectMaterial },
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0m, result.FilamentCost);
        // Material cost = 0.5 * 50.00 = 25.00
        Assert.Equal(25.00m, result.MaterialCost);
        // Printer cost = 1.0 * 0.5 * 0.15 = 0.075
        Assert.Equal(0.075m, result.PrinterCost);
        // Labor cost = 0.5 * 25.00 = 12.50
        Assert.Equal(12.50m, result.LaborCost);
        // Total = 0 + 25.00 + 0.075 + 12.50 = 37.575
        Assert.Equal(37.575m, result.TotalCost);
        // Final price = 37.575 * 1.20 = 45.09
        Assert.Equal(45.09m, result.FinalPrice);
    }

    [Fact]
    public void Calculate_WithFilamentAndMaterial_CalculatesBoth()
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

        var material = new Material
        {
            Name = "Epoxy Resin",
            Initial_Cost = 50.00m,
            Units = "Liters",
            Total_Material = 1.0m,
            Cost_Per_Unit = 50.00m,
            Source = "Test Supplier",
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        _context.Filaments.Add(filament);
        _context.Materials.Add(material);
        _context.SaveChanges();

        var projectFilament = new ProjectFilament
        {
            Filament_ID = filament.Id,
            Filament = filament,
            Usage_G = 100,
            Cost_At_Time = 25.00m / 1000
        };

        var projectMaterial = new ProjectMaterial
        {
            Material_ID = material.Id,
            Material = material,
            Usage = 0.5m,
            Units = "Liters",
            Cost_At_Time = 50.00m
        };

        var project = new Project
        {
            Name = "Combined Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 2.0m,
            Finishing_Time = 1.0m,
            ProjectFilaments = new List<ProjectFilament> { projectFilament },
            ProjectMaterials = new List<ProjectMaterial> { projectMaterial },
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2.50m, result.FilamentCost);      // 100 * 0.025
        Assert.Equal(25.00m, result.MaterialCost);     // 0.5 * 50.00
        Assert.Equal(0.15m, result.PrinterCost);       // 2.0 * 0.5 * 0.15
        Assert.Equal(25.00m, result.LaborCost);        // 1.0 * 25.00
        // Total = 2.50 + 25.00 + 0.15 + 25.00 = 52.65
        Assert.Equal(52.65m, result.TotalCost);
        // Final price = 52.65 * 1.20 = 63.18
        Assert.Equal(63.18m, result.FinalPrice);
    }

    #endregion

    #region Calculate Tests - Multiple Items

    [Fact]
    public void Calculate_WithMultipleFilaments_CalculatesCombinedCost()
    {
        // Arrange
        var filament1 = new Filament
        {
            Brand = "Brand 1",
            Material = "PLA",
            Texture = "Matte",
            Color = "Red",
            Weight_Grams = 1000,
            Cost = 20.00m,
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        var filament2 = new Filament
        {
            Brand = "Brand 2",
            Material = "PETG",
            Texture = "Silk",
            Color = "Blue",
            Weight_Grams = 1000,
            Cost = 30.00m,
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        _context.Filaments.AddRange(filament1, filament2);
        _context.SaveChanges();

        var projectFilament1 = new ProjectFilament
        {
            Filament_ID = filament1.Id,
            Filament = filament1,
            Usage_G = 100,
            Cost_At_Time = 20.00m / 1000        // 0.02 per gram
        };

        var projectFilament2 = new ProjectFilament
        {
            Filament_ID = filament2.Id,
            Filament = filament2,
            Usage_G = 50,
            Cost_At_Time = 30.00m / 1000        // 0.03 per gram
        };

        var project = new Project
        {
            Name = "Multi-Filament Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 1.0m,
            Finishing_Time = 0.25m,
            ProjectFilaments = new List<ProjectFilament> { projectFilament1, projectFilament2 },
            ProjectMaterials = new List<ProjectMaterial>(),
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        // Filament cost = (100 * 0.02) + (50 * 0.03) = 2.00 + 1.50 = 3.50
        Assert.Equal(3.50m, result.FilamentCost);
        Assert.Equal(2, result.Filaments.Count);
    }

    [Fact]
    public void Calculate_WithMultipleMaterials_CalculatesCombinedCost()
    {
        // Arrange
        var material1 = new Material
        {
            Name = "Material 1",
            Initial_Cost = 50.00m,
            Units = "Liters",
            Total_Material = 1.0m,
            Cost_Per_Unit = 50.00m,
            Source = "Supplier 1",
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        var material2 = new Material
        {
            Name = "Material 2",
            Initial_Cost = 30.00m,
            Units = "Kilograms",
            Total_Material = 1.0m,
            Cost_Per_Unit = 30.00m,
            Source = "Supplier 2",
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        _context.Materials.AddRange(material1, material2);
        _context.SaveChanges();

        var projectMaterial1 = new ProjectMaterial
        {
            Material_ID = material1.Id,
            Material = material1,
            Usage = 0.5m,
            Units = "Liters",
            Cost_At_Time = 50.00m
        };

        var projectMaterial2 = new ProjectMaterial
        {
            Material_ID = material2.Id,
            Material = material2,
            Usage = 0.75m,
            Units = "Kilograms",
            Cost_At_Time = 30.00m
        };

        var project = new Project
        {
            Name = "Multi-Material Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 1.0m,
            Finishing_Time = 0.5m,
            ProjectFilaments = new List<ProjectFilament>(),
            ProjectMaterials = new List<ProjectMaterial> { projectMaterial1, projectMaterial2 },
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        // Material cost = (0.5 * 50.00) + (0.75 * 30.00) = 25.00 + 22.50 = 47.50
        Assert.Equal(47.50m, result.MaterialCost);
        Assert.Equal(2, result.Materials.Count);
    }

    #endregion

    #region Calculate Tests - Breakdown Details

    [Fact]
    public void Calculate_IncludesFilamentBreakdown()
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
        _context.SaveChanges();

        var projectFilament = new ProjectFilament
        {
            Filament_ID = filament.Id,
            Filament = filament,
            Usage_G = 100,
            Cost_At_Time = 0.025m
        };

        var project = new Project
        {
            Name = "Filament Breakdown Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 1.0m,
            Finishing_Time = 0.5m,
            ProjectFilaments = new List<ProjectFilament> { projectFilament },
            ProjectMaterials = new List<ProjectMaterial>(),
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Filaments);

        var breakdown = result.Filaments[0];
        Assert.Equal(filament.Id, breakdown.FilamentId);
        Assert.Equal("Test Brand", breakdown.Brand);
        Assert.Equal("Red", breakdown.Color);
        Assert.Equal(100, breakdown.UsageG);
        Assert.Equal(0.025m, breakdown.CostAtTime);
        Assert.Equal(2.50m, breakdown.Total);  // 100 * 0.025
    }

    [Fact]
    public void Calculate_IncludesMaterialBreakdown()
    {
        // Arrange
        var material = new Material
        {
            Name = "Epoxy Resin",
            Initial_Cost = 50.00m,
            Units = "Liters",
            Total_Material = 1.0m,
            Cost_Per_Unit = 50.00m,
            Source = "Test Supplier",
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };
        _context.Materials.Add(material);
        _context.SaveChanges();

        var projectMaterial = new ProjectMaterial
        {
            Material_ID = material.Id,
            Material = material,
            Usage = 0.5m,
            Units = "Liters",
            Cost_At_Time = 50.00m
        };

        var project = new Project
        {
            Name = "Material Breakdown Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 1.0m,
            Finishing_Time = 0.5m,
            ProjectFilaments = new List<ProjectFilament>(),
            ProjectMaterials = new List<ProjectMaterial> { projectMaterial },
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Materials);

        var breakdown = result.Materials[0];
        Assert.Equal(material.Id, breakdown.MaterialId);
        Assert.Equal("Epoxy Resin", breakdown.Name);
        Assert.Equal("Liters", breakdown.Units);
        Assert.Equal(0.5m, breakdown.Usage);
        Assert.Equal(50.00m, breakdown.CostAtTime);
        Assert.Equal(25.00m, breakdown.Total);  // 0.5 * 50.00
    }

    #endregion

    #region Calculate Tests - Different Settings

    [Fact]
    public void Calculate_RespectsDifferentMarkup()
    {
        // Arrange
        _testSettings.Default_Markup = 0.50m;  // 50% markup instead of 20%
        _context.SaveChanges();

        var project = new Project
        {
            Name = "High Markup Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 0m,
            Finishing_Time = 0m,
            ProjectFilaments = new List<ProjectFilament>(),
            ProjectMaterials = new List<ProjectMaterial>(),
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0m, result.TotalCost);
        // Final price = 0 * 1.50 = 0
        Assert.Equal(0m, result.FinalPrice);

        // Now test with actual cost
        var project2 = new Project
        {
            Name = "High Markup Project 2",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 1.0m,
            Finishing_Time = 0m,
            ProjectFilaments = new List<ProjectFilament>(),
            ProjectMaterials = new List<ProjectMaterial>(),
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        var result2 = _service.Calculate(project2);
        // Printer cost = 1.0 * 0.5 * 0.15 = 0.075
        // Final price = 0.075 * 1.50 = 0.1125
        Assert.Equal(0.1125m, result2.FinalPrice);
    }

    [Fact]
    public void Calculate_RespectsDifferentElectricityRate()
    {
        // Arrange
        _testSettings.Electricity_Rate = 0.30m;  // $0.30 per kWh instead of $0.15
        _context.SaveChanges();

        var project = new Project
        {
            Name = "High Electricity Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 2.0m,
            Finishing_Time = 0m,
            ProjectFilaments = new List<ProjectFilament>(),
            ProjectMaterials = new List<ProjectMaterial>(),
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        // Printer cost = 2.0 * 0.5 * 0.30 = 0.30
        Assert.Equal(0.30m, result.PrinterCost);
    }

    [Fact]
    public void Calculate_RespectsDifferentLaborRate()
    {
        // Arrange
        _testSettings.Labor_Rate = 50.00m;  // $50 per hour instead of $25
        _context.SaveChanges();

        var project = new Project
        {
            Name = "High Labor Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 0m,
            Finishing_Time = 2.0m,
            ProjectFilaments = new List<ProjectFilament>(),
            ProjectMaterials = new List<ProjectMaterial>(),
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        // Labor cost = 2.0 * 50.00 = 100.00
        Assert.Equal(100.00m, result.LaborCost);
    }

    #endregion

    #region Calculate Tests - Edge Cases

    [Fact]
    public void Calculate_WithZeroTimes_CalculatesCorrectly()
    {
        // Arrange
        var project = new Project
        {
            Name = "Zero Time Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 0m,
            Finishing_Time = 0m,
            ProjectFilaments = new List<ProjectFilament>(),
            ProjectMaterials = new List<ProjectMaterial>(),
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0m, result.PrinterCost);
        Assert.Equal(0m, result.LaborCost);
        Assert.Equal(0m, result.TotalCost);
        Assert.Equal(0m, result.FinalPrice);
    }

    [Fact]
    public void Calculate_WithLargeValues_CalculatesCorrectly()
    {
        // Arrange
        var filament = new Filament
        {
            Brand = "Premium Brand",
            Material = "Nylon",
            Texture = "Glossy",
            Color = "Gold",
            Weight_Grams = 10000,
            Cost = 500.00m,
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };
        _context.Filaments.Add(filament);
        _context.SaveChanges();

        var projectFilament = new ProjectFilament
        {
            Filament_ID = filament.Id,
            Filament = filament,
            Usage_G = 5000,
            Cost_At_Time = 500.00m / 10000  // 0.05 per gram
        };

        var project = new Project
        {
            Name = "Large Project",
            Printer_ID = _testPrinter.Id,
            Printer = _testPrinter,
            Print_Time = 100.0m,
            Finishing_Time = 50.0m,
            ProjectFilaments = new List<ProjectFilament> { projectFilament },
            ProjectMaterials = new List<ProjectMaterial>(),
            Created_On = DateTime.UtcNow,
            Created_By = "TestUser"
        };

        // Act
        var result = _service.Calculate(project);

        // Assert
        Assert.NotNull(result);
        // Filament cost = 5000 * 0.05 = 250.00
        Assert.Equal(250.00m, result.FilamentCost);
        // Printer cost = 100.0 * 0.5 * 0.15 = 7.50
        Assert.Equal(7.50m, result.PrinterCost);
        // Labor cost = 50.0 * 25.00 = 1250.00
        Assert.Equal(1250.00m, result.LaborCost);
        // Total = 250 + 0 + 7.50 + 1250 = 1507.50
        Assert.Equal(1507.50m, result.TotalCost);
        // Final price = 1507.50 * 1.20 = 1809.00
        Assert.Equal(1809.00m, result.FinalPrice);
    }

    #endregion
}
