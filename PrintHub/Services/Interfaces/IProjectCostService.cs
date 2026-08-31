using PrintHub.Database.Models;
using PrintHub.DTOs;

namespace PrintHub.Services.Interfaces;

public interface IProjectCostService
{
    ProjectCostDto Calculate(Project project);

}
