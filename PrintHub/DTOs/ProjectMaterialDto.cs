namespace PrintHub.DTOs;

public class ProjectMaterialDto
{
    public int Material_ID { get; set; }
    public string Name { get; set; } = null!;
    public string Units { get; set; } = null!;

    public decimal Usage { get; set; }
    public decimal Cost_At_Time { get; set; }
}

public class NewProjectMaterialDto
{
    public int Material_ID { get; set; }
    public decimal Usage { get; set; }
}