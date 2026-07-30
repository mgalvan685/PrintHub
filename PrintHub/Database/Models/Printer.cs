namespace PrintHub.Database.Models;

public class Printer : BaseEntity
{
    public string Brand { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Power_Per_Hour { get; set; } // kWh

    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
