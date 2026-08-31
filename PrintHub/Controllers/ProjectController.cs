using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;

namespace PrintHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly PrintHubContext _context;
    private readonly IProjectService _service;
    private readonly IProjectCostService _costService;

    public ProjectsController(PrintHubContext context, IProjectService service, IProjectCostService costService)
    {
        _context = context;
        _service = service;
        _costService = costService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] NewProjectDto dto)
    {
        var result = await _service.CreateProjectAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _service.GetByIdAsync(id);
        return project == null ? NotFound() : Ok(project);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _service.GetAllAsync();
        return Ok(projects);
    }

    [HttpGet("{id}/cost")]
    [ProducesResponseType(typeof(ProjectCostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ProjectCostDto> GetCost(int id)
    {
        var project = _context.Projects
            .Include(p => p.Printer)
            .Include(p => p.ProjectFilaments).ThenInclude(f => f.Filament)
            .Include(p => p.ProjectMaterials).ThenInclude(m => m.Material)
            .Include(p => p.PriceModifiers)
            .FirstOrDefault(p => p.Id == id);

        if (project == null)
            return NotFound();

        var result = _costService.Calculate(project);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
    {
        var updated = await _service.UpdateProjectAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteProjectAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
