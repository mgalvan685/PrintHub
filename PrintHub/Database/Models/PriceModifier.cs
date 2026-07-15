namespace PrintHub.Database.Models;

public class PriceModifier : BaseEntity
{
    public int Project_ID { get; set; }
    public Project Project { get; set; } = null!;

    public decimal Waste_Modifier { get; set; }
    public decimal Power_Usage { get; set; }   // cents per kWh
    public decimal Profit_Margin { get; set; } // e.g. 0.25 = 25%
    public decimal Labor_Per_Hour { get; set; }
    public decimal Labor_Time { get; set; }    // hours

    public DateTime Effective_Date { get; set; }
    public int? Version { get; set; }
}
