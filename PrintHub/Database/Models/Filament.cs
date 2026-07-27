namespace PrintHub.Database.Models;

public class Filament : BaseEntity
{
    public string Brand { get; set; } = null!;
    public string Material { get; set; } = null!;
    public string Color { get; set; } = null!;
    public decimal Weight_Grams { get; set; }
    public decimal Cost { get; set; }

    public ICollection<ProjectFilament> ProjectFilaments { get; set; } = new List<ProjectFilament>();
}
