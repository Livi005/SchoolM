using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Extensions;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorStudentSubject;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("professorStudentSubject")]
[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Professor},{Roles.Secretary}")]
public class ProfessorStudentSubjectController : ControllerBase
{
    private readonly IProfessorStudentSubjectService _service;
    public ProfessorStudentSubjectController(IProfessorStudentSubjectService service) => _service = service;

    [HttpPost("givenote")]
    public async Task<IActionResult> GiveNote([FromBody] ProfessorStudentSubjectDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.CreateAsync(dto, ct), created: true);

    [HttpGet("listnotes")]
    public async Task<IActionResult> ListNotes(CancellationToken ct)
        => this.ToActionResult(await _service.ListAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ToActionResult(await _service.GetByIdAsync(id, ct));

    [HttpGet("getsubjects")]
    public async Task<IActionResult> GetSubjects([FromQuery] string userName, CancellationToken ct)
        => this.ToActionResult(await _service.GetSubjectsOfProfessorAsync(userName, ct));

    [HttpGet("getstudents")]
    public async Task<IActionResult> GetStudents([FromQuery] int idSub, CancellationToken ct)
        => this.ToActionResult(await _service.GetStudentsForSubjectAsync(idSub, ct));

    [HttpPut("updateStudentNote")]
    public async Task<IActionResult> UpdateStudentNote(
        [FromQuery] int profStudSubId, [FromQuery] float studentGrade, CancellationToken ct)
        => this.ToActionResult(await _service.UpdateGradeAsync(profStudSubId, studentGrade, ct));

    [HttpGet("getnotesByProfessor")]
    public async Task<IActionResult> GetNotesByProfessor([FromQuery] string userName, CancellationToken ct)
        => this.ToActionResult(await _service.ListByProfessorUsernameAsync(userName, ct));

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ToActionResult(await _service.DeleteAsync(id, ct));
}