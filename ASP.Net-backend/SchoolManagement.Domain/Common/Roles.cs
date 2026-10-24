namespace SchoolManagement.Domain.Common;

/// <summary>
/// Nombres de los roles del sistema.
/// Se usan como constantes para evitar strings mágicos en [Authorize(Roles = ...)] y en Identity.
/// </summary>
public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string Secretary = "Secretary";
    public const string Professor = "Professor";
    public const string Student = "Student";
}