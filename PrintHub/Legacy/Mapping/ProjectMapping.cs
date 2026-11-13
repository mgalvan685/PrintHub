using PrintHub.DTOs;
using PrintHub.Legacy.Models;

namespace PrintHub.Legacy.Mapping;

public class ProjectMapping
{
    public static NewProjectDto ToNewProjectDto(
    LegacyProjectImport legacy,
    int defaultPrinterId)
    {
        var ts = TimeSpan.Parse(legacy.PrintTime);

        return new NewProjectDto
        {
            Legacy_Id = legacy.Id,
            Name = legacy.Name,
            Description = legacy.Description,

            Printer_ID = defaultPrinterId,

            Print_Time = (decimal)ts.TotalHours,
            Finishing_Time = legacy.LaborMinutes,

            Filaments = legacy.Filaments?
                .Select(f => new NewProjectFilamentDto
                {
                    Filament_ID = f.FilamentUsed.Id,
                    Usage_G = f.AmountUsed
                })
                .ToList() ?? new(),

            Materials = legacy.Materials?
                .Select(m => new NewProjectMaterialDto
                {
                    Material_ID = m.MaterialUsed.Id,
                    Usage = m.AmountUsed
                })
                .ToList() ?? new()
        };
    }
}
