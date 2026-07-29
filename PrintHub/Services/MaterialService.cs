using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;

namespace PrintHub.Services;

public class MaterialService : IMaterialService
{
    private readonly PrintHubContext _context;

    public MaterialService(PrintHubContext context)
    {
        _context = context;
    }

    public async Task<MaterialDto> CreateMaterialAsync(NewMaterialDto dto)
    {
        var material = new Material
        {
            Name = dto.Name,
            Initial_Cost = dto.Initial_Cost,
            Units = dto.Units,
            Total_Material = dto.Total_Material,
            Cost_Per_Unit = dto.Cost_Per_Unit,
            Source = dto.Source
        };

        _context.Materials.Add(material);
        await _context.SaveChangesAsync();

        return ToDto(material);
    }

    public async Task<MaterialDto?> GetByIdAsync(int id)
    {
        var material = await _context.Materials
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        return material == null ? null : ToDto(material);
    }

    public async Task<List<MaterialDto>> GetAllAsync()
    {
        return await _context.Materials
            .AsNoTracking()
            .Select(m => ToDto(m))
            .ToListAsync();
    }

    public async Task<MaterialDto?> UpdateMaterialAsync(int id, UpdateMaterialDto dto)
    {
        var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == id);
        if (material == null)
            return null;

        material.Name = dto.Name ?? material.Name;
        material.Initial_Cost = dto.Initial_Cost ?? material.Initial_Cost;
        material.Units = dto.Units ?? material.Units;
        material.Total_Material = dto.Total_Material ?? material.Total_Material;
        material.Cost_Per_Unit = dto.Cost_Per_Unit ?? material.Cost_Per_Unit;
        material.Source = dto.Source ?? material.Source;

        await _context.SaveChangesAsync();

        return ToDto(material);
    }

    public async Task<bool> DeleteMaterialAsync(int id)
    {
        var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == id);
        if (material == null)
            return false;

        _context.Materials.Remove(material);
        await _context.SaveChangesAsync();
        return true;
    }

    private static MaterialDto ToDto(Material m)
    {
        return new MaterialDto
        {
            Id = m.Id,
            Name = m.Name,
            Initial_Cost = m.Initial_Cost,
            Units = m.Units,
            Total_Material = m.Total_Material,
            Cost_Per_Unit = m.Cost_Per_Unit,
            Source = m.Source
        };
    }

    public async Task<Material?> FindByLegacyIdAsync(int legacyId)
    {
        return await _context.Materials.FirstOrDefaultAsync(m => m.Legacy_Id == legacyId);
    }

}

