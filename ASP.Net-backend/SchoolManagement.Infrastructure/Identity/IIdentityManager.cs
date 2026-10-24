namespace SchoolManagement.Infrastructure.Identity;

public interface IIdentityManager
{
    Task<(bool Succeeded, User? User, IEnumerable<string> Errors)> CreateUserAsync(User user, string password);
    Task<(bool Succeeded, IEnumerable<string> Errors)> AddRoleAsync(string userId, string role);
    Task<bool> CheckCredentialsAsync(string username, string password);
    Task<bool> IsInRoleAsync(string userId, string role);
    Task<User?> FindByUserNameAsync(string username);
    IReadOnlyList<User> ListUsers();
}