namespace PrintHub.DTOs;

#region Output
public record PrinterDto
{
    public int Id { get; init; }
    public string Brand { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal Power_Per_Hour { get; init; } // kWh
}
#endregion

#region Input
public record NewPrinterDto
{
    public string Brand { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal Power_Per_Hour { get; init; } // kWh
}

public class UpdatePrinterDto
{
    public string? Brand { get; set; }
    public string? Type { get; set; }
    public string? Name { get; set; }
    public decimal? Power_Per_Hour { get; set; }
}
#endregion
