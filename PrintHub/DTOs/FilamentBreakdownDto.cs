namespace PrintHub.DTOs;

public class FilamentBreakdownDto
{
    public int FilamentId { get; set; }
    public string Brand { get; set; } = null!;
    public string Color { get; set; } = null!;
    public decimal UsageG { get; set; }
    public decimal CostAtTime { get; set; }
    public decimal Total { get; set; }
}
