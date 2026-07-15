namespace PrintHub.Database.Models;

public class Material : BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal Initial_Cost { get; set; }
    public string Units { get; set; } = null!;
    public decimal Total_Material { get; set; }
    public decimal Cost_Per_Unit { get; set; }
    public string? Source { get; set; }

    public ICollection<ProjectMaterial> ProjectMaterials { get; set; } = new List<ProjectMaterial>();
}
