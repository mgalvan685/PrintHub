using Microsoft.AspNetCore.Mvc;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;

namespace PrintHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GlobalSettingsController : ControllerBase
{
    private readonly IGlobalSettingsService _service;

    public GlobalSettingsController(IGlobalSettingsService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GlobalSettingsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GlobalSettingsDto>> Get()
    {
        var settings = await _service.GetAsync();

        if (settings is null)
            return NotFound();

        return Ok(settings);
    }

    [HttpPut]
    [ProducesResponseType(typeof(GlobalSettingsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GlobalSettingsDto>> Update(
        [FromBody] UpdateGlobalSettingsDto dto)
    {
        var result = await _service.UpdateAsync(dto);
        return Ok(result);
    }
}