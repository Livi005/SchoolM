using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Extensions;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.AuxiliaryMeans;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("auxiliarymeans")]
[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
public class AuxiliaryMeansController : ControllerBase
{
    private readonly IAuxiliaryMeansService _service;
    public AuxiliaryMeansController(IAuxiliaryMeansService service) => _service = service;

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] AuxiliaryMeansDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.CreateAsync(dto, ct), created: true);

    [HttpGet("list")]
    public async Task<IActionResult> List(CancellationToken ct)
        => this.ToActionResult(await _service.ListAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => this.ToActionResult(await _service.GetByIdAsync(id, ct));

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AuxiliaryMeansDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.UpdateAsync(id, dto, ct));

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => this.ToActionResult(await _service.DeleteAsync(id, ct));
}