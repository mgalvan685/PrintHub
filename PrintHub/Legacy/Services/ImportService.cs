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
                Legacy_Id = legacy.Id,
                Name = legacy.Name,
                Description = legacy.Description,

                Printer_ID = defaultPrinterId,

                Print_Time = (decimal)TimeSpan.Parse(legacy.PrintTime).TotalHours,
                Finishing_Time = legacy.LaborMinutes,

                Filaments = new List<NewProjectFilamentDto>(),
                Materials = new List<NewProjectMaterialDto>()
            };

            //
            // FILAMENTS
            //
            foreach (var f in legacy.Filaments)
            {
                var filament = await _filamentService
                    .FindByLegacyIdAsync(f.FilamentUsed.Id);

                if (filament != null)
                {
                    dto.Filaments.Add(new NewProjectFilamentDto
                    {
                        Filament_ID = filament.Id,
                        Usage_G = f.AmountUsed
                    });
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
            // MATERIALS
            //
            if (legacy.Materials != null)
            {
                foreach (var m in legacy.Materials)
                {
                    var material = await _materialService
                        .FindByLegacyIdAsync(m.MaterialUsed.Id);

                    if (material != null)
                    {
                        dto.Materials.Add(new NewProjectMaterialDto
                        {
                            Material_ID = material.Id,
                            Usage = m.AmountUsed
                        });
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

            var validation = _projectValidator.Validate(dto);

            if (!validation.IsValid)
            {
                result.Errors.Add(new ImportError
                {
                    ItemId = legacy.Id,
                    Message = string.Join("; ",
                        validation.Errors.Select(e => e.ErrorMessage))
                });

                continue;
            }

            var project = await _projectService
                .CreateProjectAsync(dto, legacy.Id);

            result.Imported.Add(project);
        }

        return result;
    }
    #endregion
}

