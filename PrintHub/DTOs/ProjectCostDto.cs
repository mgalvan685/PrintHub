namespace PrintHub.DTOs;

public class ProjectCostDto
{
    public decimal FilamentCost { get; set; }
    public decimal MaterialCost { get; set; }
    public decimal PrinterCost { get; set; }
    public decimal LaborCost { get; set; }
    public decimal ModifierAdjustment { get; set; }
    public decimal TotalCost { get; set; }
    public decimal FinalPrice { get; set; }

    public List<FilamentBreakdownDto> Filaments { get; set; } = new();
    public List<MaterialBreakdownDto> Materials { get; set; } = new();
}
