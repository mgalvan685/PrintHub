namespace PrintHub.Database.Models;

public class CostBreakdown : BaseEntity
{
    public int Project_ID { get; set; }
    public Project Project { get; set; } = null!;

    public decimal Filament_Cost { get; set; }
    public decimal Material_Cost { get; set; }
    public decimal Power_Cost { get; set; }
    public decimal Labor_Cost { get; set; }
    public decimal Waste_Cost { get; set; }
    public decimal Total_Cost { get; set; }

    public DateTime Calculated_At { get; set; }
}
