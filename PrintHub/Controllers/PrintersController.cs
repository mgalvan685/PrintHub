using Microsoft.AspNetCore.Mvc;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class PrintersController : ControllerBase
{
    private readonly IPrinterService _service;

    public PrintersController(IPrinterService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PrinterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] NewPrinterDto newPrinter)
    {
        try
        {
            var result = await _service.CreatePrinterAsync(newPrinter);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetPrintersAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PrinterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var printer = await _service.GetPrinterByIdAsync(id);

        if (printer == null)
            return NotFound();

        return Ok(printer);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PrinterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePrinterDto dto)
    {
        try
        {
            var updated = await _service.UpdatePrinterAsync(id, dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeletePrinterAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

}
