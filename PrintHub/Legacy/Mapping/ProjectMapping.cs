using PrintHub.DTOs;
using PrintHub.Legacy.Models;

namespace PrintHub.Legacy.Mapping;

public class ProjectMapping
{
    public static NewProjectDto ToNewProjectDto(LegacyProjectImport legacy, int defaultPrinterId)
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

            FilamentIds = legacy.Filaments
                ?.Select(f => f.FilamentUsed.Id)
                .ToList() ?? new List<int>(),

            MaterialIds = legacy.Materials
                ?.Select(m => m.MaterialUsed.Id)
                .ToList() ?? new List<int>()
        };
    }

}
