using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Extensions;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("professorClassRoom")]
[Authorize(Roles = Roles.SuperAdmin)]
public class ProfessorClassRoomController : ControllerBase
{
    private readonly IProfessorClassRoomService _service;
    public ProfessorClassRoomController(IProfessorClassRoomService service) => _service = service;

    [HttpGet("getSpecs")]
    public async Task<IActionResult> GetSpecs(CancellationToken ct)
        => this.ToActionResult(await _service.GetSpecsAsync(ct));

    [HttpGet("getProfessorsBySpec")]
    public async Task<IActionResult> GetProfessorsBySpec([FromQuery] string spec, CancellationToken ct)
        => this.ToActionResult(await _service.GetProfessorsBySpecAsync(spec, ct));
}