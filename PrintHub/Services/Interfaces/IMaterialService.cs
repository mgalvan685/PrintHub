using PrintHub.DTOs;

namespace PrintHub.Services.Interfaces;

public interface IMaterialService
{
    Task<MaterialDto> CreateMaterialAsync(NewMaterialDto dto);
    Task<MaterialDto?> GetByIdAsync(int id);
    Task<List<MaterialDto>> GetAllAsync();
    Task<MaterialDto?> UpdateMaterialAsync(int id, UpdateMaterialDto dto);
    Task<bool> DeleteMaterialAsync(int id);
}
