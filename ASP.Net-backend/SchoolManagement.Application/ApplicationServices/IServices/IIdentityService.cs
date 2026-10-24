using SchoolManagement.Application.ApplicationServices.Maps_Dto;
using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface IIdentityService
{
    /// <summary>Registra un usuario con rol y devuelve (token, userId).</summary>
    Task<(string Token, string UserId)> CreateUserAsync(RegisterDto dto, CancellationToken cancellationToken = default);

    /// <summary>Valida credenciales y devuelve (success, token, role).</summary>
    Task<(bool Success, string Token, string Role)> CheckCredentialsAsync(LoginDto dto, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<User>> ListUsersAsync(CancellationToken cancellationToken = default);
}