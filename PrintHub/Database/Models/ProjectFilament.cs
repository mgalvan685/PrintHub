namespace PrintHub.Database.Models;

public class ProjectFilament : BaseEntity
{
    public int Project_ID { get; set; }
    public Project Project { get; set; } = null!;

    public int Filament_ID { get; set; }
    public Filament Filament { get; set; } = null!;

    public decimal Usage_G { get; set; }
    public decimal Cost_At_Time { get; set; }
}
