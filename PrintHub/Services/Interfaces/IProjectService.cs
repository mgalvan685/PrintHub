using PrintHub.DTOs;

namespace PrintHub.Services.Interfaces;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(NewProjectDto dto, int legacyId);
    Task<ProjectDto> CreateProjectAsync(NewProjectDto dto);
    Task<ProjectDto?> GetByIdAsync(int id);
    Task<List<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto dto);
    Task<bool> DeleteProjectAsync(int id);
    Task AddFilamentToProjectAsync(int projectId, int filamentId);
    Task AddMaterialToProjectAsync(int projectId, int materialId);
}
