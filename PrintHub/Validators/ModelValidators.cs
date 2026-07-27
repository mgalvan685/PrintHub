using PrintHub.Helpers;

namespace PrintHub.Validators;

public class ModelValidators
{
    #region Event Validations
    public static bool IsValidEventType(string eventType)
    {
        return EventTypeConstants.ALL.Contains(eventType);
    }
    #endregion

    #region Filament Validations
    public static bool IsValidFilamentMaterial(string material)
    {
        return FilamentMaterialConstants.ALL.Contains(material);
    }
    #endregion

    #region Material Validations
    public static bool IsValidMaterialUnit(string unit)
    {
        return MaterialUnitConstants.ALL.Contains(unit);
    }
    #endregion

    #region Printer Validations
    public static bool IsValidPrinterType(string type)
    {
        return PrinterTypeConstants.ALL.Contains(type);
    }
    #endregion
}
