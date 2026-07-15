namespace PrintHub.Database.Models;

public class ProjectMaterial : BaseEntity
{
    public int Project_ID { get; set; }
    public Project Project { get; set; } = null!;

    public int Material_ID { get; set; }
    public Material Material { get; set; } = null!;

    public decimal Usage { get; set; }
    public string Units { get; set; } = null!;
    public decimal Cost_At_Time { get; set; }
}
