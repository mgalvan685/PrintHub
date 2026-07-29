using PrintHub.Legacy.Handlers;

namespace PrintHub.Legacy.Services.Interfaaces;

public interface IImportService
{
    Task<FilamentImportResult> ImportFilamentsAsync(string json);
    Task<MaterialImportResult> ImportMaterialsAsync(string json);
    Task<ProjectImportResult> ImportProjectsAsync(string json, int defaultPrinterId);
}