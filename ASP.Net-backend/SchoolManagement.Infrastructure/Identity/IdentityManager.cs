using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagement.Infrastructure.Identity;

public class IdentityManager : IIdentityManager
{
    private readonly UserManager<User> _userManager;

    public IdentityManager(UserManager<User> userManager) => _userManager = userManager;

    public async Task<(bool Succeeded, User? User, IEnumerable<string> Errors)> CreateUserAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return (result.Succeeded, result.Succeeded ? user : null, result.Errors.Select(e => e.Description));
    }

    public async Task<(bool Succeeded, IEnumerable<string> Errors)> AddRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return (false, new[] { $"User '{userId}' not found." });

        if (!await _userManager.IsInRoleAsync(user, role))
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return (result.Succeeded, result.Errors.Select(e => e.Description));
        }

        return (true, Array.Empty<string>());
    }

    public async Task<bool> CheckCredentialsAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user is not null && await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user is not null && await _userManager.IsInRoleAsync(user, role);
    }

    public Task<User?> FindByUserNameAsync(string username)
        => _userManager.FindByNameAsync(username);

    public IReadOnlyList<User> ListUsers() => _userManager.Users.AsNoTracking().ToList();
}