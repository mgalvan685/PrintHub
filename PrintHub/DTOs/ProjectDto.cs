namespace PrintHub.DTOs;

public class ProjectDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public int Printer_ID { get; set; }
    public string Printer_Name { get; set; } = null!;

    public string Print_Time { get; set; } = "00:00:00"; // HH:mm:ss
    public decimal Labor { get; set; }                  // minutes

    public List<ProjectFilamentDto> Filaments { get; set; } = new();
    public List<ProjectMaterialDto>? Materials { get; set; } = new();
}


public class NewProjectDto
{
    public int? Legacy_Id { get; set; } = null;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public int Printer_ID { get; set; }

    public decimal Print_Time { get; set; } = 0;
    public decimal Finishing_Time { get; set; } = 0;

    public List<int> FilamentIds { get; set; } = new();
    public List<int>? MaterialIds { get; set; } = null;
}

public class UpdateProjectDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public int? Printer_ID { get; set; }

    public decimal? Print_Time { get; set; }
    public decimal? Finishing_Time { get; set; }

    public List<int>? FilamentIds { get; set; }
    public List<int>? MaterialIds { get; set; }
}
