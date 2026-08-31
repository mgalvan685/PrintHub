using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;

namespace PrintHub.Services;

public class ProjectCostService : BaseService, IProjectCostService
{
    private readonly PrintHubContext _context;

    public ProjectCostService(PrintHubContext context)
    {
        _context = context;
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
            FilamentCost = filamentCost,
            MaterialCost = materialCost,
            PrinterCost = printerCost,
            LaborCost = laborCost,
            ModifierAdjustment = modifierAdjustment,
            TotalCost = totalCost,
            FinalPrice = finalPrice,

            Filaments = project.ProjectFilaments.Select(f => new FilamentBreakdownDto
            {
                FilamentId = f.Filament_ID,
                Brand = f.Filament.Brand,
                Color = f.Filament.Color,
                UsageG = f.Usage_G,
                CostAtTime = f.Cost_At_Time,
                Total = f.Usage_G * f.Cost_At_Time
            }).ToList(),

            Materials = project.ProjectMaterials.Select(m => new MaterialBreakdownDto
            {
                MaterialId = m.Material_ID,
                Name = m.Material.Name,
                Units = m.Units,
                Usage = m.Usage,
                CostAtTime = m.Cost_At_Time,
                Total = m.Usage * m.Cost_At_Time
            }).ToList()
        };

        return dto;
    }
}
