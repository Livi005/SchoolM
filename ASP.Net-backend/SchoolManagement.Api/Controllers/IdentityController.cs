using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Api.Controllers;

[ApiController]
[Route("identity")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _service;
    public IdentityController(IIdentityService service) => _service = service;

    [HttpPost("register")]
    [Authorize(Roles = Roles.SuperAdmin)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken ct)
    {
        var (token, userId) = await _service.CreateUserAsync(dto, ct);
        return StatusCode(StatusCodes.Status201Created, new { userId, token });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var (success, token, role) = await _service.CheckCredentialsAsync(dto, ct);
        if (!success)
            return Unauthorized(new { message = "Credenciales incorrectas." });

        return Ok(new { token, role });
    }

    [HttpGet("list")]
    [Authorize(Roles = Roles.SuperAdmin)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var users = await _service.ListUsersAsync(ct);
        return Ok(users.Select(u => new { u.Id, u.UserName, u.Email }));
    }
}