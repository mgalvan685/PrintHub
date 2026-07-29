namespace PrintHub.Legacy.Models;

public class LegacyProjectImport : BaseModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }

    public required List<LegacyProjectFilamentUsage> Filaments { get; set; } = new();
    public List<LegacyProjectMaterialUsage>? Materials { get; set; } = null;

    public required string PrintTime { get; set; }
    public decimal LaborMinutes { get; set; }
}

public class LegacyProjectFilamentUsage
{
    public required LegacyFilamentImport FilamentUsed { get; set; }
    public decimal AmountUsed { get; set; }
    public decimal CostPerPrint { get; set; }
}

public class LegacyProjectMaterialUsage
{
    public required LegacyMaterialImport MaterialUsed { get; set; }
    public required string UsageUnit { get; set; }
    public decimal AmountUsed { get; set; }
    public decimal CostPerPrint { get; set; }
}
