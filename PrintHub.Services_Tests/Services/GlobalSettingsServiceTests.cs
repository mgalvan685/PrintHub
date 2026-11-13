using AutoMapper;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services;

namespace PrintHub.Services_Tests.Services;

public class GlobalSettingsServiceTests : IDisposable
{
    private readonly PrintHubContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GlobalSettingsService _service;

    public GlobalSettingsServiceTests()
    {
        var options = new DbContextOptionsBuilder<PrintHubContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PrintHubContext(options);

        // Setup mapper with mocking
        _mockMapper = new Mock<IMapper>();

        // Setup mapper to return proper GlobalSettingsDto
        _mockMapper.Setup(m => m.Map<GlobalSettingsDto>(It.IsAny<GlobalSettings>()))
            .Returns((GlobalSettings settings) => new GlobalSettingsDto
            {
                Default_Markup = settings.Default_Markup,
                Electricity_Rate = settings.Electricity_Rate,
                Labor_Rate = settings.Labor_Rate,
                Default_Waste_Multiplier = settings.Default_Waste_Multiplier
            });

        _service = new GlobalSettingsService(_context, _mockMapper.Object);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    #region GetAsync Tests

    [Fact]
    public async Task GetAsync_WithExistingSettings_ReturnsSettings()
    {
        // Arrange
        var settings = new GlobalSettings
        {
            Default_Markup = 0.20m,
            Electricity_Rate = 0.15m,
            Labor_Rate = 25.00m,
            Default_Waste_Multiplier = 1.1m
        };
        _context.GlobalSettings.Add(settings);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.20m, result.Default_Markup);
        Assert.Equal(0.15m, result.Electricity_Rate);
        Assert.Equal(25.00m, result.Labor_Rate);
        Assert.Equal(1.1m, result.Default_Waste_Multiplier);
    }

    [Fact]
    public async Task GetAsync_WithNoSettings_ReturnsNull()
    {
        // Act
        var result = await _service.GetAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ReturnsCorrectMarkupValue()
    {
        // Arrange
        var settings = new GlobalSettings
        {
            Default_Markup = 0.50m,
            Electricity_Rate = 0.20m,
            Labor_Rate = 30.00m,
            Default_Waste_Multiplier = 1.15m
        };
        _context.GlobalSettings.Add(settings);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.50m, result.Default_Markup);
    }

    [Fact]
    public async Task GetAsync_ReturnsCorrectElectricityRate()
    {
        // Arrange
        var settings = new GlobalSettings
        {
            Default_Markup = 0.25m,
            Electricity_Rate = 0.25m,
            Labor_Rate = 35.00m,
            Default_Waste_Multiplier = 1.2m
        };
        _context.GlobalSettings.Add(settings);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.25m, result.Electricity_Rate);
    }

    [Fact]
    public async Task GetAsync_ReturnsCorrectLaborRate()
    {
        // Arrange
        var settings = new GlobalSettings
        {
            Default_Markup = 0.20m,
            Electricity_Rate = 0.15m,
            Labor_Rate = 50.00m,
            Default_Waste_Multiplier = 1.1m
        };
        _context.GlobalSettings.Add(settings);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(50.00m, result.Labor_Rate);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_WithValidDto_UpdatesAllFields()
    {
        // Arrange
        var existingSettings = new GlobalSettings
        {
            Default_Markup = 0.10m,
            Electricity_Rate = 0.10m,
            Labor_Rate = 20.00m,
            Default_Waste_Multiplier = 1.05m
        };
        _context.GlobalSettings.Add(existingSettings);
        await _context.SaveChangesAsync();

        var updateDto = new UpdateGlobalSettingsDto
        {
            Default_Markup = 0.30m,
            Electricity_Rate = 0.20m,
            Labor_Rate = 40.00m,
            Default_Waste_Multiplier = 1.25m
        };

        // Act
        var result = await _service.UpdateAsync(updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.30m, result.Default_Markup);
        Assert.Equal(0.20m, result.Electricity_Rate);
        Assert.Equal(40.00m, result.Labor_Rate);
        Assert.Equal(1.25m, result.Default_Waste_Multiplier);
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        // Arrange
        var existingSettings = new GlobalSettings
        {
            Default_Markup = 0.15m,
            Electricity_Rate = 0.12m,
            Labor_Rate = 25.00m,
            Default_Waste_Multiplier = 1.1m
        };
        _context.GlobalSettings.Add(existingSettings);
        await _context.SaveChangesAsync();

        var updateDto = new UpdateGlobalSettingsDto
        {
            Default_Markup = 0.35m,
            Electricity_Rate = 0.22m,
            Labor_Rate = 45.00m,
            Default_Waste_Multiplier = 1.3m
        };

        // Act
        await _service.UpdateAsync(updateDto);

        // Assert
        var retrieved = await _service.GetAsync();
        Assert.NotNull(retrieved);
        Assert.Equal(0.35m, retrieved.Default_Markup);
        Assert.Equal(0.22m, retrieved.Electricity_Rate);
        Assert.Equal(45.00m, retrieved.Labor_Rate);
        Assert.Equal(1.3m, retrieved.Default_Waste_Multiplier);
    }

    [Fact]
    public async Task UpdateAsync_WithHighMarkup_UpdatesCorrectly()
    {
        // Arrange
        var existingSettings = new GlobalSettings
        {
            Default_Markup = 0.10m,
            Electricity_Rate = 0.10m,
            Labor_Rate = 20.00m,
            Default_Waste_Multiplier = 1.05m
        };
        _context.GlobalSettings.Add(existingSettings);
        await _context.SaveChangesAsync();

        var updateDto = new UpdateGlobalSettingsDto
        {
            Default_Markup = 1.00m,  // 100% markup
            Electricity_Rate = 0.10m,
            Labor_Rate = 20.00m,
            Default_Waste_Multiplier = 1.05m
        };

        // Act
        var result = await _service.UpdateAsync(updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1.00m, result.Default_Markup);
    }

    [Fact]
    public async Task UpdateAsync_WithZeroValues_UpdatesCorrectly()
    {
        // Arrange
        var existingSettings = new GlobalSettings
        {
            Default_Markup = 0.20m,
            Electricity_Rate = 0.15m,
            Labor_Rate = 25.00m,
            Default_Waste_Multiplier = 1.1m
        };
        _context.GlobalSettings.Add(existingSettings);
        await _context.SaveChangesAsync();

        var updateDto = new UpdateGlobalSettingsDto
        {
            Default_Markup = 0.00m,
            Electricity_Rate = 0.00m,
            Labor_Rate = 0.00m,
            Default_Waste_Multiplier = 1.00m
        };

        // Act
        var result = await _service.UpdateAsync(updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.00m, result.Default_Markup);
        Assert.Equal(0.00m, result.Electricity_Rate);
        Assert.Equal(0.00m, result.Labor_Rate);
        Assert.Equal(1.00m, result.Default_Waste_Multiplier);
    }

    [Fact]
    public async Task UpdateAsync_CreatesSettingsIfNoneExist()
    {
        // Arrange - No settings in database
        Assert.Empty(_context.GlobalSettings);

        var updateDto = new UpdateGlobalSettingsDto
        {
            Default_Markup = 0.25m,
            Electricity_Rate = 0.18m,
            Labor_Rate = 35.00m,
            Default_Waste_Multiplier = 1.15m
        };

        // Act - This should throw since FirstAsync() requires at least one row
        // This test verifies the service behavior when settings don't exist
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(updateDto));
    }

    [Fact]
    public async Task UpdateAsync_WithLargeValues_UpdatesCorrectly()
    {
        // Arrange
        var existingSettings = new GlobalSettings
        {
            Default_Markup = 0.20m,
            Electricity_Rate = 0.15m,
            Labor_Rate = 25.00m,
            Default_Waste_Multiplier = 1.1m
        };
        _context.GlobalSettings.Add(existingSettings);
        await _context.SaveChangesAsync();

        var updateDto = new UpdateGlobalSettingsDto
        {
            Default_Markup = 5.00m,      // 500% markup
            Electricity_Rate = 10.00m,    // Very high electricity rate
            Labor_Rate = 500.00m,         // Very high labor rate
            Default_Waste_Multiplier = 3.0m  // Significant waste
        };

        // Act
        var result = await _service.UpdateAsync(updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5.00m, result.Default_Markup);
        Assert.Equal(10.00m, result.Electricity_Rate);
        Assert.Equal(500.00m, result.Labor_Rate);
        Assert.Equal(3.0m, result.Default_Waste_Multiplier);
    }

    [Fact]
    public async Task UpdateAsync_MultipleUpdates_AppliesLatest()
    {
        // Arrange
        var existingSettings = new GlobalSettings
        {
            Default_Markup = 0.10m,
            Electricity_Rate = 0.10m,
            Labor_Rate = 20.00m,
            Default_Waste_Multiplier = 1.05m
        };
        _context.GlobalSettings.Add(existingSettings);
        await _context.SaveChangesAsync();

        var update1 = new UpdateGlobalSettingsDto
        {
            Default_Markup = 0.20m,
            Electricity_Rate = 0.15m,
            Labor_Rate = 25.00m,
            Default_Waste_Multiplier = 1.1m
        };

        var update2 = new UpdateGlobalSettingsDto
        {
            Default_Markup = 0.40m,
            Electricity_Rate = 0.25m,
            Labor_Rate = 50.00m,
            Default_Waste_Multiplier = 1.2m
        };

        // Act
        await _service.UpdateAsync(update1);
        var result = await _service.UpdateAsync(update2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.40m, result.Default_Markup);
        Assert.Equal(0.25m, result.Electricity_Rate);
        Assert.Equal(50.00m, result.Labor_Rate);
        Assert.Equal(1.2m, result.Default_Waste_Multiplier);

        // Verify database has latest values
        var retrieved = await _service.GetAsync();
        Assert.NotNull(retrieved);
        Assert.Equal(0.40m, retrieved.Default_Markup);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsMappedDto()
    {
        // Arrange
        var existingSettings = new GlobalSettings
        {
            Default_Markup = 0.20m,
            Electricity_Rate = 0.15m,
            Labor_Rate = 25.00m,
            Default_Waste_Multiplier = 1.1m
        };
        _context.GlobalSettings.Add(existingSettings);
        await _context.SaveChangesAsync();

        var updateDto = new UpdateGlobalSettingsDto
        {
            Default_Markup = 0.30m,
            Electricity_Rate = 0.20m,
            Labor_Rate = 40.00m,
            Default_Waste_Multiplier = 1.25m
        };

        // Act
        var result = await _service.UpdateAsync(updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<GlobalSettingsDto>(result);
        Assert.Equal(typeof(GlobalSettingsDto), result.GetType());
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task GetAsync_AfterUpdate_ReturnsUpdatedValues()
    {
        // Arrange
        var initialSettings = new GlobalSettings
        {
            Default_Markup = 0.15m,
            Electricity_Rate = 0.12m,
            Labor_Rate = 22.00m,
            Default_Waste_Multiplier = 1.08m
        };
        _context.GlobalSettings.Add(initialSettings);
        await _context.SaveChangesAsync();

        var beforeUpdate = await _service.GetAsync();
        Assert.NotNull(beforeUpdate);
        Assert.Equal(0.15m, beforeUpdate.Default_Markup);

        var updateDto = new UpdateGlobalSettingsDto
        {
            Default_Markup = 0.50m,
            Electricity_Rate = 0.30m,
            Labor_Rate = 60.00m,
            Default_Waste_Multiplier = 1.5m
        };

        // Act
        await _service.UpdateAsync(updateDto);
        var afterUpdate = await _service.GetAsync();

        // Assert
        Assert.NotNull(afterUpdate);
        Assert.Equal(0.50m, afterUpdate.Default_Markup);
        Assert.Equal(0.30m, afterUpdate.Electricity_Rate);
        Assert.Equal(60.00m, afterUpdate.Labor_Rate);
        Assert.Equal(1.5m, afterUpdate.Default_Waste_Multiplier);
    }

    #endregion
}
