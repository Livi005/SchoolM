using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Extensions;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfStudSubCourse;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("profstudsubcourse")]
[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Professor},{Roles.Secretary}")]
public class ProfStudSubCourseController : ControllerBase
{
    private readonly IProfStudSubCourseService _service;
    public ProfStudSubCourseController(IProfStudSubCourseService service) => _service = service;

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ProfStudSubCourseDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.CreateAsync(dto, ct), created: true);

    [HttpGet("list")]
    public async Task<IActionResult> List(CancellationToken ct)
        => this.ToActionResult(await _service.ListAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ToActionResult(await _service.GetByIdAsync(id, ct));

    [HttpGet("listProfessorsByStudent")]
    public async Task<IActionResult> ListPendingByStudent([FromQuery] string userName, CancellationToken ct)
        => this.ToActionResult(await _service.GetPendingEvaluationsAsync(userName, ct));

    [HttpPost("listByProfessor")]
    public async Task<IActionResult> ListByProfessor([FromBody] ProfStudSubCourseConsultDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.ListByProfessorAsync(dto, ct));

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProfStudSubCourseDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.UpdateAsync(id, dto, ct));

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ToActionResult(await _service.DeleteAsync(id, ct));
}