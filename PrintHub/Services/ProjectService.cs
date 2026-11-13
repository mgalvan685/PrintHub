using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;

namespace PrintHub.Services;

public class ProjectService : BaseService, IProjectService
{
    private readonly PrintHubContext _db;
    private readonly IMapper _mapper;
    private readonly IValidator<NewProjectDto> _newValidator;
    private readonly IValidator<UpdateProjectDto> _updateValidator;

    public ProjectService(PrintHubContext db, IMapper mapper, IValidator<NewProjectDto> newValidator, IValidator<UpdateProjectDto> updateValidator)
    {
        _db = db;
        _mapper = mapper;
        _newValidator = newValidator;
        _updateValidator = updateValidator;
    }

    public async Task<ProjectDto> CreateProjectAsync(NewProjectDto dto)
    {
        var validationResult = await _newValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await CreateProjectInternalAsync(dto, null);
    }

    public async Task<ProjectDto> CreateProjectAsync(
        NewProjectDto dto,
        int legacyId)
    {
        return await CreateProjectInternalAsync(dto, legacyId);
    }

    private async Task<ProjectDto> CreateProjectInternalAsync(
    NewProjectDto dto,
    int? legacyId)
    {
        var printerExists = await _db.Printers
            .AnyAsync(p => p.Id == dto.Printer_ID);

        if (!printerExists)
        {
            throw new ValidationException(
                $"Printer {dto.Printer_ID} does not exist.");
        }

        var project = new Project
        {
            Legacy_Id = legacyId,
            Name = dto.Name,
            Description = dto.Description,
            Printer_ID = dto.Printer_ID,
            Print_Time = dto.Print_Time,
            Finishing_Time = dto.Finishing_Time
        };

        SetCreatedFields(project);

        _db.Projects.Add(project);

        await _db.SaveChangesAsync();

        //
        // FILAMENTS
        //
        foreach (var filamentDto in dto.Filaments)
        {
            var filament = await _db.Filaments
                .FindAsync(filamentDto.Filament_ID);

            if (filament == null)
                continue;

            _db.ProjectFilaments.Add(new ProjectFilament
            {
                Project_ID = project.Id,
                Filament_ID = filamentDto.Filament_ID,
                Usage_G = filamentDto.Usage_G,

                // Cost per gram snapshot
                Cost_At_Time = CostPerUnit(filament.Cost, filament.Weight_Grams)
            });
        }

        //
        // MATERIALS
        //
        foreach (var materialDto in dto.Materials)
        {
            var material = await _db.Materials
                .FindAsync(materialDto.Material_ID);

            if (material == null)
                continue;

            _db.ProjectMaterials.Add(new ProjectMaterial
            {
                Project_ID = project.Id,
                Material_ID = materialDto.Material_ID,
                Usage = materialDto.Usage,
                Units = material.Units,

                // Cost per unit snapshot
                Cost_At_Time = material.Cost_Per_Unit
            });
        }

        await _db.SaveChangesAsync();

        return await GetByIdAsync(project.Id)
            ?? throw new InvalidOperationException(
                $"Unable to load project {project.Id} after creation.");
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _db.Projects
            .Include(p => p.Printer)
            .Include(p => p.ProjectFilaments)
            .ThenInclude(pf => pf.Filament)
            .Include(p => p.ProjectMaterials)
            .ThenInclude(pm => pm.Material)
            .FirstOrDefaultAsync(p => p.Id == id);

        return project == null ? null : _mapper.Map<ProjectDto>(project);
    }

    public async Task<List<ProjectDto>> GetAllAsync()
    {
        var projects = await _db.Projects
            .Include(p => p.Printer)
            .Include(p => p.ProjectFilaments)
                .ThenInclude(pf => pf.Filament)
            .Include(p => p.ProjectMaterials)
                .ThenInclude(pm => pm.Material)
            .ToListAsync();

        return _mapper.Map<List<ProjectDto>>(projects);
    }

    public async Task<ProjectDto?> UpdateProjectAsync(
    int id,
    UpdateProjectDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var project = await _db.Projects.FindAsync(id);

        if (project == null)
            return null;

        project.Name = dto.Name ?? project.Name;
        project.Description = dto.Description ?? project.Description;
        project.Printer_ID = dto.Printer_ID ?? project.Printer_ID;
        project.Print_Time = dto.Print_Time ?? project.Print_Time;
        project.Finishing_Time = dto.Finishing_Time ?? project.Finishing_Time;

        //
        // FILAMENTS
        //
        if (dto.Filaments != null)
        {
            var existingFilaments = _db.ProjectFilaments
                .Where(x => x.Project_ID == id);

            _db.ProjectFilaments.RemoveRange(existingFilaments);

            foreach (var filamentDto in dto.Filaments)
            {
                var filament = await _db.Filaments
                    .FindAsync(filamentDto.Filament_ID);

                if (filament == null)
                    continue;

                _db.ProjectFilaments.Add(new ProjectFilament
                {
                    Project_ID = id,
                    Filament_ID = filamentDto.Filament_ID,

                    Usage_G = filamentDto.Usage_G,

                    Cost_At_Time = CostPerUnit(filament.Cost, filament.Weight_Grams)
                });
            }
        }

        //
        // MATERIALS
        //
        if (dto.Materials != null)
        {
            var existingMaterials = _db.ProjectMaterials
                .Where(x => x.Project_ID == id);

            _db.ProjectMaterials.RemoveRange(existingMaterials);

            foreach (var materialDto in dto.Materials)
            {
                var material = await _db.Materials
                    .FindAsync(materialDto.Material_ID);

                if (material == null)
                    continue;

                _db.ProjectMaterials.Add(new ProjectMaterial
                {
                    Project_ID = id,
                    Material_ID = materialDto.Material_ID,

                    Usage = materialDto.Usage,
                    Units = material.Units,

                    Cost_At_Time = material.Cost_Per_Unit
                });
            }
        }

        SetUpdatedFields(project);

        await _db.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null) return false;

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
        return true;
    }

    private decimal CostPerUnit(decimal totalCost, decimal quantity)
    {
        return quantity > 0 ? totalCost / quantity : 0;
    }
}
