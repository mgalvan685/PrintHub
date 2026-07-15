namespace PrintHub.Database.Models;

public class Project : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public int Printer_ID { get; set; }
    public Printer Printer { get; set; } = null!;

    public decimal Print_Time { get; set; } // hours
    public decimal Labor { get; set; }      // minutes

    public ICollection<ProjectFilament> ProjectFilaments { get; set; } = new List<ProjectFilament>();
    public ICollection<ProjectMaterial> ProjectMaterials { get; set; } = new List<ProjectMaterial>();
    public ICollection<PriceModifier> PriceModifiers { get; set; } = new List<PriceModifier>();
    public ICollection<PrintEvent> PrintEvents { get; set; } = new List<PrintEvent>();
    public ICollection<CostBreakdown> CostBreakdowns { get; set; } = new List<CostBreakdown>();
}
