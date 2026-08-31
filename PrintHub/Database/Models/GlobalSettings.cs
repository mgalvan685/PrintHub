namespace PrintHub.Database.Models;

public class GlobalSettings : BaseEntity
{
    public decimal Electricity_Rate { get; set; }
    public decimal Labor_Rate { get; set; }
    public decimal Default_Markup { get; set; }
    public decimal Default_Waste_Multiplier { get; set; }
}
