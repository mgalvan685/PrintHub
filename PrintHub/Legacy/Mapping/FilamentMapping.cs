using PrintHub.DTOs;
using PrintHub.Helpers;
using PrintHub.Legacy.Models;

namespace PrintHub.Legacy.Mapping;

public class FilamentMapping
{
    public static NewFilamentDto ToNewFilamentDto(LegacyFilamentImport legacy)
    {
        return new NewFilamentDto
        {
            Legacy_Id = legacy.Id,
            Brand = legacy.Brand,
            Material = legacy.Type,
            Texture = legacy.Texture,
            Color = legacy.Color,
            Weight_Grams = FilamentWeightConstants.DEFAULT,
            Cost = legacy.CostPerKg
        };
    }
}
