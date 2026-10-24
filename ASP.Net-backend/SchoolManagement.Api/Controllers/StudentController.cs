using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Extensions;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Student;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("student")]
[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Secretary},{Roles.Professor}")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _service;
    public StudentController(IStudentService service) => _service = service;

    [HttpPost("create")]
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Secretary}")]
    public async Task<IActionResult> Create([FromBody] StudentDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.CreateWithUserAsync(dto, ct), created: true);

    [HttpGet("list")]
    public async Task<IActionResult> List(CancellationToken ct)
        => this.ToActionResult(await _service.ListAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ToActionResult(await _service.GetByIdAsync(id, ct));

    [HttpPut("update/{id:int}")]
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Secretary}")]
    public async Task<IActionResult> Update(int id, [FromBody] StudentDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.UpdateAsync(id, dto, ct));

    [HttpDelete("delete/{id:int}")]
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Secretary}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ToActionResult(await _service.DeleteAsync(id, ct));

    [HttpGet("getBadStudents")]
    public async Task<IActionResult> GetBadStudents(CancellationToken ct)
        => this.ToActionResult(await _service.GetBadStudentsAsync(ct));
}