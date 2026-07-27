namespace PrintHub.DTOs;

public record FilamentDto
{
    public int Id { get; init; }
    public string Brand { get; init; } = string.Empty;
    public string Material { get; init; } = string.Empty;   // PLA, PETG, ABS, etc.
    public string Color { get; init; } = string.Empty;      // color of the filament
    public decimal Weight_Grams { get; init; }
    public decimal Cost { get; init; }                      // total cost of the spool
}

public class NewFilamentDto
{
    public string Brand { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;   // PLA, PETG, ABS, etc.
    public string Color { get; set; } = string.Empty;      // color of the filament
    public decimal Weight_Grams { get; set; }
    public decimal Cost { get; set; }                      // total cost of the spool
}

public class UpdateFilamentDto
{
    public string? Brand { get; set; }
    public string? Material { get; set; }
    public string? Color { get; set; }
    public decimal? Weight_Grams { get; set; }
    public decimal? Cost { get; set; }
}
