using Microsoft.AspNetCore.Mvc;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;

namespace PrintHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialsController : ControllerBase
{
    private readonly IMaterialService _service;

    public MaterialsController(IMaterialService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] NewMaterialDto dto)
    {
        var result = await _service.CreateMaterialAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var material = await _service.GetByIdAsync(id);
        return material == null ? NotFound() : Ok(material);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<MaterialDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var materials = await _service.GetAllAsync();
        return Ok(materials);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MaterialDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMaterialDto dto)
    {
        var updated = await _service.UpdateMaterialAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteMaterialAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
