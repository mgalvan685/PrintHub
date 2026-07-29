using PrintHub.Database.Models;
using PrintHub.DTOs;

namespace PrintHub.Services.Interfaces;

public interface IFilamentService
{
    Task<FilamentDto> CreateFilamentAsync(NewFilamentDto dto);
    Task<FilamentDto?> GetByIdAsync(int id);
    Task<List<FilamentDto>> GetAllAsync();
    Task<FilamentDto?> UpdateFilamentAsync(int id, UpdateFilamentDto dto);
    Task<bool> DeleteFilamentAsync(int id);
    Task<Filament?> FindByLegacyIdAsync(int legacyId);
}
