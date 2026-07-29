using Microsoft.AspNetCore.Mvc;
using PrintHub.Legacy.Handlers;
using PrintHub.Legacy.Services.Interfaaces;

namespace PrintHub.Legacy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly IImportService _importService;

    public ImportController(IImportService importService)
    {
        _importService = importService;
    }

    [HttpPost("filaments")]
    [ProducesResponseType(typeof(FilamentImportResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ImportFilaments([FromBody] string json)
    {
        var result = await _importService.ImportFilamentsAsync(json);
        return Ok(result);
    }

    [HttpPost("materials")]
    [ProducesResponseType(typeof(MaterialImportResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ImportMaterials([FromBody] string json)
    {
        var result = await _importService.ImportMaterialsAsync(json);
        return Ok(result);
    }

    [HttpPost("projects")]
    public async Task<IActionResult> ImportProjects([FromBody] string json, [FromQuery] int defaultPrinterId)
    {
        var result = await _importService.ImportProjectsAsync(json, defaultPrinterId);
        return Ok(result);
    }
}
