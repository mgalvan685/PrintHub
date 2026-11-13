using PrintHub.DTOs;

namespace PrintHub.Services.Interfaces;

public interface IGlobalSettingsService
{
    Task<GlobalSettingsDto?> GetAsync();
    Task<GlobalSettingsDto> UpdateAsync(UpdateGlobalSettingsDto dto);
}