using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Api.Extensions;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("notification")]
[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _service;
    public NotificationController(INotificationService service) => _service = service;

    [HttpGet("get")]
    public async Task<IActionResult> Get([FromQuery] string userId, CancellationToken ct)
        => this.ToActionResult(await _service.GetNotificationsAsync(userId, ct));
}