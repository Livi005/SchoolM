using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto;
using SchoolManagement.Application.Authentication;
using SchoolManagement.Domain.Common;
using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class IdentityService : IIdentityService
{
    private readonly IIdentityManager _identityManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        IIdentityManager identityManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper,
        ILogger<IdentityService> logger)
    {
        _identityManager = identityManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(string Token, string UserId)> CreateUserAsync(
        RegisterDto dto, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(dto);
        var role = string.IsNullOrWhiteSpace(dto.Role) ? Roles.Student : dto.Role;

        var (succeeded, created, errors) = await _identityManager.CreateUserAsync(user, dto.Password ?? string.Empty);
        if (!succeeded || created is null)
            throw new InvalidOperationException(
                $"No se pudo crear el usuario: {string.Join(", ", errors)}");

        var (roleOk, roleErrors) = await _identityManager.AddRoleAsync(created.Id, role);
        if (!roleOk)
            throw new InvalidOperationException(
                $"No se pudo asignar el rol: {string.Join(", ", roleErrors)}");

        var token = _jwtTokenGenerator.GenerateToken(created, role);
        return (token, created.Id);
    }

    public async Task<(bool Success, string Token, string Role)> CheckCredentialsAsync(
        LoginDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
            return (false, string.Empty, string.Empty);

        var valid = await _identityManager.CheckCredentialsAsync(dto.UserName, dto.Password);
        if (!valid)
            return (false, string.Empty, string.Empty);

        var user = await _identityManager.FindByUserNameAsync(dto.UserName);
        if (user is null)
            return (false, string.Empty, string.Empty);

        foreach (var role in new[] { Roles.SuperAdmin, Roles.Admin, Roles.Secretary, Roles.Professor, Roles.Student })
        {
            if (await _identityManager.IsInRoleAsync(user.Id, role))
            {
                var token = _jwtTokenGenerator.GenerateToken(user, role);
                return (true, token, role);
            }
        }

        _logger.LogWarning("Usuario {UserName} sin rol asignado", dto.UserName);
        return (false, string.Empty, string.Empty);
    }

    public Task<IReadOnlyList<User>> ListUsersAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(_identityManager.ListUsers());
}