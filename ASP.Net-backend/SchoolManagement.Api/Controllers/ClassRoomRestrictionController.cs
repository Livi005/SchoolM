using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Extensions;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomRestriction;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("classRoomRestriction")]
[Authorize(Roles = Roles.SuperAdmin)]
public class ClassRoomRestrictionController : ControllerBase
{
    private readonly IClassRoomRestrictionService _service;
    public ClassRoomRestrictionController(IClassRoomRestrictionService service) => _service = service;

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ClassRoomRestrictionDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.CreateAsync(dto, ct), created: true);

    [HttpGet("list")]
    public async Task<IActionResult> List(CancellationToken ct)
        => this.ToActionResult(await _service.ListAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ToActionResult(await _service.GetByIdAsync(id, ct));

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClassRoomRestrictionDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.UpdateAsync(id, dto, ct));

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ToActionResult(await _service.DeleteAsync(id, ct));
}