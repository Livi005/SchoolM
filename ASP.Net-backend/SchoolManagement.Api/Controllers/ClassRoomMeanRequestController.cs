using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Extensions;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomMeanRequest;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("classroommeans")]
[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Professor}")]
public class ClassRoomMeanRequestController : ControllerBase
{
    private readonly IClassRoomMeanRequestService _service;
    public ClassRoomMeanRequestController(IClassRoomMeanRequestService service) => _service = service;

    [HttpPost("checkAviableClassRoomsAndMeans")]
    public async Task<IActionResult> CheckAviable([FromBody] ClassRoomMeanRequestGetAviableDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.GetAviableClassRoomMeanAsync(dto, ct));

    [HttpPost("checkNotAviableClassRoomsAndMeans")]
    public async Task<IActionResult> CheckNotAviable([FromBody] ClassRoomMeanRequestGetAviableDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.GetNotAviableClassRoomMeanAsync(dto, ct));

    [HttpPost("reserveClassRoomAndMeans")]
    public async Task<IActionResult> Reserve([FromBody] ClassRoomMeanRequestReserveDto dto, CancellationToken ct)
        => this.ToActionResult(await _service.ReserveClassRoomAndMeanAsync(dto, ct));
}