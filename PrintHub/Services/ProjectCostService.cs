using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Helpers;
using PrintHub.Services.Interfaces;

namespace PrintHub.Services;

public class ProjectCostService : BaseService, IProjectCostService
{
    private readonly PrintHubContext _context;
    private readonly RoundingMode _roundingMode;

    public ProjectCostService(PrintHubContext context)
    {
        _context = context;
        _roundingMode = context.GlobalSettings.First().Rounding_Mode;
    }

    public ProjectCostDto Calculate(Project project)
    {
        var settings = _context.GlobalSettings.First();   // single row

        // 1. Filament cost
        var filamentCost = project.ProjectFilaments.Sum(f =>
            f.Usage_G * f.Cost_At_Time
        );

        // 2. Material cost
        var materialCost = project.ProjectMaterials.Sum(m =>
            m.Usage * m.Cost_At_Time
        );

        // 3. Printer electricity cost
        var printerCost = project.Print_Time
            * project.Printer.Power_Per_Hour
            * settings.Electricity_Rate;

        // 4. Labor cost
        var laborCost = project.Finishing_Time * settings.Labor_Rate;

        // 5. Price modifiers (optional) <---TODO
        var modifierAdjustment = 0;

        // 6. Total cost before markup
        var totalCost = filamentCost + materialCost + printerCost + laborCost + modifierAdjustment;

        // 7. Final price with markup
        var finalPrice = totalCost * (1 + settings.Default_Markup);

        // Build DTO
        var dto = new ProjectCostDto
        {
            FilamentCost = RoundCurrency(filamentCost),
            MaterialCost = RoundCurrency(materialCost),
            PrinterCost = RoundCurrency(printerCost),
            LaborCost = RoundCurrency(laborCost),
            ModifierAdjustment = RoundCurrency(modifierAdjustment),
            TotalCost = RoundCurrency(totalCost),
            FinalPrice = RoundCurrency(finalPrice),

            Filaments = project.ProjectFilaments.Select(f => new FilamentBreakdownDto
            {
                FilamentId = f.Filament_ID,
                Brand = f.Filament.Brand,
                Color = f.Filament.Color,
                UsageG = f.Usage_G,
                CostAtTime = f.Cost_At_Time,
                Total = RoundCurrency(f.Usage_G * f.Cost_At_Time)
            }).ToList(),

            Materials = project.ProjectMaterials.Select(m => new MaterialBreakdownDto
            {
                MaterialId = m.Material_ID,
                Name = m.Material.Name,
                Units = m.Units,
                Usage = m.Usage,
                CostAtTime = m.Cost_At_Time,
                Total = RoundCurrency(m.Usage * m.Cost_At_Time)
            }).ToList()
        };

        return dto;
    }

    private decimal RoundCurrency(
    decimal value)
    {
        return _roundingMode switch
        {
            RoundingMode.Bankers =>
                Math.Round(value, 2, MidpointRounding.ToEven),

            RoundingMode.AwayFromZero =>
                Math.Round(value, 2, MidpointRounding.AwayFromZero),

            RoundingMode.Up =>
                Math.Ceiling(value * 100) / 100,

            RoundingMode.Down =>
                Math.Floor(value * 100) / 100,

            _ =>
                Math.Round(value, 2, MidpointRounding.AwayFromZero)
        };
    }
}
