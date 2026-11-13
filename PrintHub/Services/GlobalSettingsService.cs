using AutoMapper;
using PrintHub.Database;
using PrintHub.Services.Interfaces;
using PrintHub.DTOs;
using PrintHub.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace PrintHub.Services;

public class GlobalSettingsService : IGlobalSettingsService
{
    private readonly PrintHubContext _db;
    private readonly IMapper _mapper;

    public GlobalSettingsService(PrintHubContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<GlobalSettingsDto?> GetAsync()
    {
        var settings = await _db.GlobalSettings.FirstOrDefaultAsync();
        return settings == null
            ? null
            : _mapper.Map<GlobalSettingsDto>(settings);
    }

    public async Task<GlobalSettingsDto> UpdateAsync(UpdateGlobalSettingsDto dto)
    {
        var settings = await _db.GlobalSettings.FirstAsync();

        // GlobalSettings should always exist, but if it doesn't, create a new one
        if (settings is null)
        {
            settings = new GlobalSettings();
            _db.GlobalSettings.Add(settings);
        }

        settings.Default_Markup = dto.Default_Markup;
        settings.Electricity_Rate = dto.Electricity_Rate;
        settings.Labor_Rate = dto.Labor_Rate;
        settings.Default_Waste_Multiplier = dto.Default_Waste_Multiplier;

        await _db.SaveChangesAsync();

        return _mapper.Map<GlobalSettingsDto>(settings);
    }
}