
// TODO: MapperConfiguration() has errors. This will need furter investigation.

//using AutoMapper;
//using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
//using PrintHub.Database.Models;
//using PrintHub.DTOs;
//using PrintHub.Mappers;
//using Xunit;

//namespace PrintHub.Validators_Tests;

//public class ProjectProfileTests
//{
//    #region Project Profile

//    [Fact]
//    public void ProjectProfile_ConfigurationIsValid()
//    {
//        // Arrange
//        var configuration = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile<ProjectProfile>();
//        });

//        // Act / Assert
//        configuration.AssertConfigurationIsValid();
//    }

//    #endregion

//    #region Projects

//    [Fact]
//    public void Project_MapsTo_ProjectDto()
//    {
//        var config = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile<ProjectProfile>();
//        });

//        var mapper = config.CreateMapper();

//        var project = new Project
//        {
//            Id = 1,
//            Name = "Dragon",
//            Description = "Test",
//            Printer_ID = 1,
//            Print_Time = 12,
//            Finishing_Time = 1,

//            Printer = new Printer
//            {
//                Name = "H2D"
//            },

//            ProjectFilaments = new List<ProjectFilament>(),

//            ProjectMaterials = new List<ProjectMaterial>()
//        };

//        var result = mapper.Map<ProjectDto>(project);

//        Assert.Equal(1, result.Id);
//        Assert.Equal("Dragon", result.Name);
//        Assert.Equal("Test", result.Description);
//        Assert.Equal("H2D", result.Printer_Name);
//        Assert.Equal(12, result.Print_Time);
//        Assert.Equal(1, result.Finishing_Time);
//    }

//    #endregion

//    #region Project Filaments

//    [Fact]
//    public void ProjectFilament_MapsTo_ProjectFilamentDto()
//    {
//        var config = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile<ProjectProfile>();
//        });

//        var mapper = config.CreateMapper();

//        var filament = new ProjectFilament
//        {
//            Filament_ID = 1,
//            Usage_G = 350,
//            Cost_At_Time = 0.02169m,

//            Filament = new Filament
//            {
//                Brand = "iSANMATE",
//                Material = "ASA",
//                Texture = "Basic",
//                Color = "White"
//            }
//        };

//        var result = mapper.Map<ProjectFilamentDto>(filament);

//        Assert.Equal(1, result.Filament_ID);
//        Assert.Equal("iSANMATE", result.Brand);
//        Assert.Equal("ASA", result.Material);
//        Assert.Equal("Basic", result.Texture);
//        Assert.Equal("White", result.Color);
//        Assert.Equal(350, result.Usage_G);
//        Assert.Equal(0.02169m, result.Cost_At_Time);
//    }

//    #endregion

//    #region Project Materials

//    [Fact]
//    public void ProjectMaterial_MapsTo_ProjectMaterialDto()
//    {
//        var config = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile<ProjectProfile>();
//        });

//        var mapper = config.CreateMapper();

//        var material = new ProjectMaterial
//        {
//            Material_ID = 2,
//            Usage = 4,
//            Cost_At_Time = 1.998m,

//            Material = new Material
//            {
//                Name = "Stainless Steel Keychain",
//                Units = "Each"
//            }
//        };

//        var result = mapper.Map<ProjectMaterialDto>(material);

//        Assert.Equal(2, result.Material_ID);
//        Assert.Equal("Stainless Steel Keychain", result.Name);
//        Assert.Equal("Each", result.Units);
//        Assert.Equal(4, result.Usage);
//        Assert.Equal(1.998m, result.Cost_At_Time);
//    }

//    #endregion

//    #region Global Settings

//    [Fact]
//    public void GlobalSettings_MapsTo_GlobalSettingsDto()
//    {
//        var mapper = CreateMapper();

//        var settings = new GlobalSettings
//        {
//            Electricity_Rate = 0.14m,
//            Labor_Rate = 25m,
//            Default_Markup = 1.5m,
//            Default_Waste_Multiplier = 1.1m
//        };

//        var result = mapper.Map<GlobalSettingsDto>(settings);

//        Assert.Equal(settings.Electricity_Rate, result.Electricity_Rate);
//        Assert.Equal(settings.Labor_Rate, result.Labor_Rate);
//        Assert.Equal(settings.Default_Markup, result.Default_Markup);
//        Assert.Equal(settings.Default_Waste_Multiplier, result.Default_Waste_Multiplier);
//    }

//    [Fact]
//    public void UpdateGlobalSettingsDto_MapsTo_GlobalSettings()
//    {
//        var mapper = CreateMapper();

//        var dto = new UpdateGlobalSettingsDto
//        {
//            Electricity_Rate = 0.14m,
//            Labor_Rate = 25m,
//            Default_Markup = 1.5m,
//            Default_Waste_Multiplier = 1.1m
//        };

//        var result = mapper.Map<GlobalSettings>(dto);

//        Assert.Equal(0.14m, result.Electricity_Rate);
//        Assert.Equal(25m, result.Labor_Rate);
//        Assert.Equal(1.5m, result.Default_Markup);
//        Assert.Equal(1.1m, result.Default_Waste_Multiplier);
//    }

//    #endregion

//    #region Helpers

//    private static IMapper CreateMapper()
//    {
//        var configuration = new MapperConfigurationExpression();

//        configuration.AddProfile<ProjectProfile>();

//        var mapperConfig = new MapperConfiguration(configuration);

//        return mapperConfig.CreateMapper();
//    }

//    #endregion
//}