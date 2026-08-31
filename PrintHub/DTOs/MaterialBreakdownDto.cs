namespace PrintHub.DTOs;

public class MaterialBreakdownDto
{
    public int MaterialId { get; set; }
    public string Name { get; set; } = null!;
    public string Units { get; set; } = null!;
    public decimal Usage { get; set; }
    public decimal CostAtTime { get; set; }
    public decimal Total { get; set; }
}
