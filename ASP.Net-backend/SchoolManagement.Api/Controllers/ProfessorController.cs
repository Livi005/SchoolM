using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Extensions;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Professor;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("professor")]
[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Secretary}")]
public class ProfessorController : ControllerBase
{
    private readonly IProfessorService _service;
    public ProfessorController(IProfessorService service) => _service = service;

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ProfessorDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.CreateWithUserAsync(dto, ct), created: true);

    [HttpGet("list")]
    public async Task<IActionResult> List(CancellationToken ct)
        => this.ToActionResult(await _service.ListAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ToActionResult(await _service.GetByIdAsync(id, ct));

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProfessorDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.UpdateAsync(id, dto, ct));

    [HttpDelete("delete/{id:int}")]
    [Authorize(Roles = Roles.SuperAdmin)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ToActionResult(await _service.DeleteAsync(id, ct));

    [HttpGet("getGoodProfessors")]
    public async Task<IActionResult> GetGoodProfessors(CancellationToken ct)
        => this.ToActionResult(await _service.GetGoodProfessorsAsync(ct));

    [HttpGet("getBadProfessors")]
    public async Task<IActionResult> GetBadProfessors(CancellationToken ct)
        => this.ToActionResult(await _service.GetBadProfessorsAsync(ct));
}