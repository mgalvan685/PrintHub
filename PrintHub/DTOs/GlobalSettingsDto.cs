using PrintHub.Helpers;

namespace PrintHub.DTOs;

public class GlobalSettingsDto
{
    public decimal Default_Markup { get; set; }
    public decimal Electricity_Rate { get; set; }
    public decimal Labor_Rate { get; set; }
    public decimal Default_Waste_Multiplier { get; set; }
    public RoundingMode Rounding_Mode { get; set; }
}

public class UpdateGlobalSettingsDto
{
    public decimal? Default_Markup { get; set; }
    public decimal? Electricity_Rate { get; set; }
    public decimal? Labor_Rate { get; set; }
    public decimal? Default_Waste_Multiplier { get; set; }
    public RoundingMode? Rounding_Mode { get; set; }
}
