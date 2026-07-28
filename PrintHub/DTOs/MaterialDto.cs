namespace PrintHub.DTOs;

public class MaterialDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Initial_Cost { get; set; }
    public string Units { get; set; } = string.Empty;
    public decimal Total_Material { get; set; }
    public decimal Cost_Per_Unit { get; set; }
    public string? Source { get; set; }
}


public class NewMaterialDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Initial_Cost { get; set; }
    public string Units { get; set; } = string.Empty;
    public decimal Total_Material { get; set; }
    public decimal Cost_Per_Unit { get; set; }
    public string? Source { get; set; }
}

public class UpdateMaterialDto
{
    public string? Name { get; set; }
    public decimal? Initial_Cost { get; set; }
    public string? Units { get; set; }
    public decimal? Total_Material { get; set; }
    public decimal? Cost_Per_Unit { get; set; }
    public string? Source { get; set; }
}
