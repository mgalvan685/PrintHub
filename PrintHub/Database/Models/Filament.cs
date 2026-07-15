namespace PrintHub.Database.Models;

public class Filament : BaseEntity
{
    public string Brand { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Texture { get; set; } = null!;
    public string Color { get; set; } = null!;
    public decimal Cost_Per_Kg { get; set; }

    public string? Material_Type { get; set; }
    public decimal? Density { get; set; }

    public ICollection<ProjectFilament> ProjectFilaments { get; set; } = new List<ProjectFilament>();
}
