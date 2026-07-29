using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;

namespace PrintHub.Services;

public class FilamentService : IFilamentService
{
    private readonly PrintHubContext _context;

    public FilamentService(PrintHubContext context)
    {
        _context = context;
    }

    public async Task<FilamentDto> CreateFilamentAsync(NewFilamentDto dto)
    {
        var filament = new Filament
        {
            Brand = dto.Brand,
            Material = dto.Material,
            Texture = dto.Texture,
            Color = dto.Color,
            Weight_Grams = dto.Weight_Grams,
            Cost = dto.Cost
        };

        _context.Filaments.Add(filament);
        await _context.SaveChangesAsync();

        return new FilamentDto
        {
            Id = filament.Id,
            Brand = filament.Brand,
            Material = filament.Material,
            Texture = filament.Texture,
            Color = filament.Color,
            Weight_Grams = filament.Weight_Grams,
            Cost = filament.Cost
        };
    }

    public async Task<FilamentDto?> GetByIdAsync(int id)
    {
        var filament = await _context.Filaments
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);

        if (filament == null)
            return null;

        return new FilamentDto
        {
            Id = filament.Id,
            Brand = filament.Brand,
            Material = filament.Material,
            Texture = filament.Texture,
            Color = filament.Color,
            Weight_Grams = filament.Weight_Grams,
            Cost = filament.Cost
        };
    }

    public async Task<List<FilamentDto>> GetAllAsync()
    {
        return await _context.Filaments
            .AsNoTracking()
            .Select(f => new FilamentDto
            {
                Id = f.Id,
                Brand = f.Brand,
                Material = f.Material,
                Texture = f.Texture,
                Color = f.Color,
                Weight_Grams = f.Weight_Grams,
                Cost = f.Cost
            })
            .ToListAsync();
    }

    public async Task<FilamentDto?> UpdateFilamentAsync(int id, UpdateFilamentDto dto)
    {
        var filament = await _context.Filaments.FirstOrDefaultAsync(f => f.Id == id);
        if (filament == null)
            return null;

        filament.Brand = dto.Brand ?? filament.Brand;
        filament.Material = dto.Material ?? filament.Material;
        filament.Texture = dto.Texture ?? filament.Texture;
        filament.Color = dto.Color ?? filament.Color;
        filament.Weight_Grams = dto.Weight_Grams ?? filament.Weight_Grams;
        filament.Cost = dto.Cost ?? filament.Cost;

        await _context.SaveChangesAsync();

        return new FilamentDto
        {
            Id = filament.Id,
            Brand = filament.Brand,
            Material = filament.Material,
            Texture = filament.Texture,
            Color = filament.Color,
            Weight_Grams = filament.Weight_Grams,
            Cost = filament.Cost
        };
    }

    public async Task<bool> DeleteFilamentAsync(int id)
    {
        var filament = await _context.Filaments.FirstOrDefaultAsync(f => f.Id == id);
        if (filament == null)
            return false;

        _context.Filaments.Remove(filament);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Filament?> FindByLegacyIdAsync(int legacyId)
    {
        return await _context.Filaments.FirstOrDefaultAsync(f => f.Legacy_Id == legacyId);
    }

}
