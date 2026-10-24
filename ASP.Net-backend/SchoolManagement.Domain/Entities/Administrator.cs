using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Administrador/director del centro.
/// </summary>
public class Administrator : ISoftDeletable
{
    public int AdminId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string AdminName { get; set; } = string.Empty;
    public int AdminSalary { get; set; }
    public bool IsDeleted { get; set; }
}