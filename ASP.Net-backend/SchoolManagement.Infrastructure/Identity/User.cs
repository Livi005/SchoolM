using Microsoft.AspNetCore.Identity;

namespace SchoolManagement.Infrastructure.Identity;

/// <summary>
/// Usuario del sistema. Extiende IdentityUser sin añadir campos propios.
/// </summary>
public class User : IdentityUser
{
}