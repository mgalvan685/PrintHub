namespace PrintHub.Legacy.Models;

public class LegacyFilamentImport : BaseModel
{
    public string Brand { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Texture { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal CostPerKg { get; set; }
    public string Description { get; set; } = string.Empty;
}

