using PrintHub.DTOs;
using PrintHub.Legacy.Models;

namespace PrintHub.Legacy.Mapping;

public class MaterialMapping
{
    public static NewMaterialDto ToNewMaterialDto(LegacyMaterialImport legacy)
    {
        return new NewMaterialDto
        {
            Legacy_Id = legacy.Id,
            Name = legacy.Name,
            Initial_Cost = legacy.InitialCost,
            Units = legacy.Units,
            Total_Material = legacy.TotalMaterial,
            Cost_Per_Unit = legacy.CostPerUnit ??
                            (legacy.TotalMaterial > 0
                                ? legacy.InitialCost / legacy.TotalMaterial
                                : 0),
            Source = legacy.Source
        };
    }

}
