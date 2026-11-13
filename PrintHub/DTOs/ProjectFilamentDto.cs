namespace PrintHub.DTOs;

public class ProjectFilamentDto
{
    public int Filament_ID { get; set; }
    public string Brand { get; set; } = null!;
    public string Material { get; set; } = null!;
    public string Texture { get; set; } = null!;
    public string Color { get; set; } = null!;

    public decimal Usage_G { get; set; }
    public decimal Cost_At_Time { get; set; }
}

public class NewProjectFilamentDto
{
    public int Filament_ID { get; set; }
    public decimal Usage_G { get; set; }
}