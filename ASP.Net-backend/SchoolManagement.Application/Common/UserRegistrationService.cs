using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Application.Common;

public interface IUserRegistrationService
{
    Task<(User User, string Password)> RegisterUserAsync(
        string displayName,
        string role,
        CancellationToken cancellationToken = default);
}

public class UserRegistrationService : IUserRegistrationService
{
    private readonly IIdentityManager _identityManager;
    private static readonly Random _random = new();
    private const string PasswordChars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%";

    public UserRegistrationService(IIdentityManager identityManager)
        => _identityManager = identityManager;

    public async Task<(User User, string Password)> RegisterUserAsync(
        string displayName,
        string role,
        CancellationToken cancellationToken = default)
    {
        var userName = GenerateUsername(displayName);
        var password = GeneratePassword(12);

        var user = new User
        {
            UserName = userName,
            Email = $"{userName}@school.local"
        };

        var (succeeded, created, errors) = await _identityManager.CreateUserAsync(user, password);
        if (!succeeded || created is null)
            throw new InvalidOperationException(
                $"No se pudo crear el usuario: {string.Join(", ", errors)}");

        var (roleOk, roleErrors) = await _identityManager.AddRoleAsync(created.Id, role);
        if (!roleOk)
            throw new InvalidOperationException(
                $"No se pudo asignar el rol: {string.Join(", ", roleErrors)}");

        return (created, password);
    }

    private static string GenerateUsername(string displayName)
    {
        var baseName = new string(displayName
            .Where(char.IsLetterOrDigit)
            .ToArray())
            .ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(baseName))
            baseName = "user";

        return $"{baseName}_{Guid.NewGuid():N}"[..Math.Min(20, baseName.Length + 7)];
    }

    private static string GeneratePassword(int length)
    {
        var chars = new char[length];
        for (int i = 0; i < length; i++)
            chars[i] = PasswordChars[_random.Next(PasswordChars.Length)];

        // Asegurar complejidad para Identity
        chars[0] = 'A';
        chars[1] = 'a';
        chars[2] = '1';
        chars[3] = '!';

        return new string(chars);
    }
}