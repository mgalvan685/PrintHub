using FluentValidation;
using PrintHub.DTOs;
using PrintHub.Legacy.Handlers;
using PrintHub.Legacy.Mapping;
using PrintHub.Legacy.Models;
using PrintHub.Legacy.Services.Interfaaces;
using PrintHub.Services.Interfaces;
using System.Text.Json;

namespace PrintHub.Legacy.Services;

public class ImportService : IImportService
{
    private readonly IFilamentService _filamentService;
    private readonly IMaterialService _materialService;
    private readonly IProjectService _projectService;
    private readonly IValidator<NewFilamentDto> _filamentValidator;
    private readonly IValidator<NewMaterialDto> _materialValidator;
    private readonly IValidator<NewProjectDto> _projectValidator;

    public ImportService(
        IFilamentService filamentService,
        IMaterialService materialService,
        IProjectService projectService,
        IValidator<NewFilamentDto> filamentValidator,
        IValidator<NewMaterialDto> materialValidator,
        IValidator<NewProjectDto> projectValidator)
    {
        _filamentService = filamentService;
        _materialService = materialService;
        _projectService = projectService;
        _filamentValidator = filamentValidator;
        _materialValidator = materialValidator;
        _projectValidator = projectValidator;
    }

    #region Filament Import
    public async Task<FilamentImportResult> ImportFilamentsAsync(string json)
    {
        var legacyItems = JsonSerializer.Deserialize<List<LegacyFilamentImport>>(json);

        var result = new FilamentImportResult();

        if (legacyItems == null)
        {
            result.Errors.Add(new ImportError
            {
                ItemId = 0,
                Message = "Invalid JSON format."
            });
            return result;
        }

        foreach (var legacy in legacyItems)
        {
            var dto = FilamentMapping.ToNewFilamentDto(legacy);

            var validation = _filamentValidator.Validate(dto);
            if (!validation.IsValid)
            {
                result.Errors.Add(new ImportError
                {
                    ItemId = legacy.Id,
                    Message = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))
                });
                continue;
            }

            var created = await _filamentService.CreateFilamentAsync(dto);
            result.Imported.Add(created);
        }

        return result;
    }
    #endregion

    #region Material Import
    public async Task<MaterialImportResult> ImportMaterialsAsync(string json)
    {
        var legacyItems = JsonSerializer.Deserialize<List<LegacyMaterialImport>>(json);

        var result = new MaterialImportResult();

        if (legacyItems == null)
        {
            result.Errors.Add(new ImportError
            {
                ItemId = 0,
                Message = "Invalid JSON format."
            });
            return result;
        }

        foreach (var legacy in legacyItems)
        {
            var dto = MaterialMapping.ToNewMaterialDto(legacy);

            var validation = _materialValidator.Validate(dto);
            if (!validation.IsValid)
            {
                result.Errors.Add(new ImportError
                {
                    ItemId = legacy.Id,
                    Message = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))
                });
                continue;
            }

            var created = await _materialService.CreateMaterialAsync(dto);
            result.Imported.Add(created);
        }

        return result;
    }
    #endregion

    #region Project Import
    public async Task<ProjectImportResult> ImportProjectsAsync(string json, int defaultPrinterId)
    {
        var legacyItems = JsonSerializer.Deserialize<List<LegacyProjectImport>>(json);

        var result = new ProjectImportResult();

        if (legacyItems == null)
        {
            result.Errors.Add(new ImportError
            {
                ItemId = 0,
                Message = "Invalid JSON format."
            });
            return result;
        }

        foreach (var legacy in legacyItems)
        {
            var dto = new NewProjectDto
            {
                Name = legacy.Name,
                Description = legacy.Description,
                Printer_ID = defaultPrinterId,
                Print_Time = (decimal)TimeSpan.Parse(legacy.PrintTime).TotalHours,
                Finishing_Time = legacy.LaborMinutes,
                FilamentIds = new List<int>(),
                MaterialIds = new List<int>()
            };

            var validation = _projectValidator.Validate(dto);
            if (!validation.IsValid)
            {
                result.Errors.Add(new ImportError
                {
                    ItemId = legacy.Id,
                    Message = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))
                });
                continue;
            }

            // Create project with Legacy_Id
            var project = await _projectService.CreateProjectAsync(dto, legacy.Id);

            //
            // FILAMENT LINKS
            //
            foreach (var f in legacy.Filaments)
            {
                var filament = await _filamentService.FindByLegacyIdAsync(f.FilamentUsed.Id);

                if (filament != null)
                {
                    await _projectService.AddFilamentToProjectAsync(project.Id, filament.Id);
                }
                else
                {
                    result.Errors.Add(new ImportError
                    {
                        ItemId = legacy.Id,
                        Message = $"Filament Legacy_Id {f.FilamentUsed.Id} not found."
                    });
                }
            }

            //
            // MATERIAL LINKS
            //
            if (legacy.Materials != null)
            {
                foreach (var m in legacy.Materials)
                {
                    var material = await _materialService.FindByLegacyIdAsync(m.MaterialUsed.Id);

                    if (material != null)
                    {
                        await _projectService.AddMaterialToProjectAsync(project.Id, material.Id);
                    }
                    else
                    {
                        result.Errors.Add(new ImportError
                        {
                            ItemId = legacy.Id,
                            Message = $"Material Legacy_Id {m.MaterialUsed.Id} not found."
                        });
                    }
                }
            }

            result.Imported.Add(project);
        }

        return result;
    }
    #endregion
}

