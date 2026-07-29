using PrintHub.DTOs;

namespace PrintHub.Legacy.Handlers;

public class ImportHandlers
{
    public List<ImportError> Errors { get; set; } = new();
}

public class ImportError
{
    public int ItemId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class FilamentImportResult : ImportHandlers
{
    public List<FilamentDto> Imported { get; set; } = new();
}

public class MaterialImportResult : ImportHandlers
{
    public List<MaterialDto> Imported { get; set; } = new();
}

public class ProjectImportResult : ImportHandlers
{
    public List<ProjectDto> Imported { get; set; } = new();
}