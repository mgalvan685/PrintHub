using Microsoft.AspNetCore.Mvc;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PrintHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilamentsController : ControllerBase
{
    private readonly IFilamentService _service;

    public FilamentsController(IFilamentService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(FilamentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] NewFilamentDto dto)
    {
        try
        {
            var result = await _service.CreateFilamentAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(FilamentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var filament = await _service.GetByIdAsync(id);
        if (filament == null)
            return NotFound();

        return Ok(filament);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<FilamentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var filaments = await _service.GetAllAsync();
        return Ok(filaments);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(FilamentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFilamentDto dto)
    {
        var updated = await _service.UpdateFilamentAsync(id, dto);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteFilamentAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
