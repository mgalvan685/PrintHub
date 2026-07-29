namespace PrintHub.Legacy.Models;

public class LegacyMaterialImport : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public decimal InitialCost { get; set; }
    public string Units { get; set; } = string.Empty;
    public decimal TotalMaterial { get; set; }
    public decimal? CostPerUnit { get; set; }
    public string? Source { get; set; }
}
