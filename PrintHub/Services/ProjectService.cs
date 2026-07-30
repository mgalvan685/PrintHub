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

    public async Task<ProjectDto> CreateProjectAsync(NewProjectDto dto, int legacyId)
    {
        var project = new Project
        {
            Legacy_Id = legacyId,
            Name = dto.Name,
            Description = dto.Description,
            Printer_ID = dto.Printer_ID,
            Print_Time = dto.Print_Time,
            Labor = dto.Labor
        };

        SetCreatedFields(project);

        _db.Projects.Add(project);
        await _db.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> CreateProjectAsync(NewProjectDto dto)
    {
        var validationResult = await _newValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description,
            Printer_ID = dto.Printer_ID,
            Print_Time = dto.Print_Time,
            Labor = dto.Labor
        };

        SetCreatedFields(project);

        _db.Projects.Add(project);
        await _db.SaveChangesAsync();

        // Linking tables
        foreach (var filamentId in dto.FilamentIds)
        {
            _db.ProjectFilaments.Add(new ProjectFilament
            {
                Project_ID = project.Id,
                Filament_ID = filamentId
            });
        }

        if (dto.MaterialIds != null)
        {
            foreach (var materialId in dto.MaterialIds)
            {
                _db.ProjectMaterials.Add(new ProjectMaterial
                {
                    Project_ID = project.Id,
                    Material_ID = materialId
                });
            }
        }

        await _db.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _db.Projects
            .Include(p => p.ProjectFilaments)
            .Include(p => p.ProjectMaterials)
            .FirstOrDefaultAsync(p => p.Id == id);

        return project == null ? null : _mapper.Map<ProjectDto>(project);
    }

    public async Task<List<ProjectDto>> GetAllAsync()
    {
        var projects = await _db.Projects.ToListAsync();
        return _mapper.Map<List<ProjectDto>>(projects);
    }

    public async Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var project = await _db.Projects.FindAsync(id);
        if (project == null) return null;

        dto.Name = dto.Name ?? project.Name;
        dto.Description = dto.Description ?? project.Description;
        dto.Printer_ID = dto.Printer_ID ?? project.Printer_ID;
        dto.Print_Time = dto.Print_Time ?? project.Print_Time;
        dto.Labor = dto.Labor ?? project.Labor;

        SetUpdatedFields(project);

        await _db.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null) return false;

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task AddFilamentToProjectAsync(int projectId, int filamentId)
    {
        // TODO: Add validators
        _db.ProjectFilaments.Add(new ProjectFilament
        {
            Project_ID = projectId,
            Filament_ID = filamentId
        });

        await _db.SaveChangesAsync();
    }

    public async Task AddMaterialToProjectAsync(int projectId, int materialId)
    {
        // TODO: Add validators
        _db.ProjectMaterials.Add(new ProjectMaterial
        {
            Project_ID = projectId,
            Material_ID = materialId
        });

        await _db.SaveChangesAsync();
    }
}
