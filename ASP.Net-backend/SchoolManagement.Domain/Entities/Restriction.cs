using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Restricción aplicable a un aula (ej. "no usar equipos").
/// </summary>
public class Restriction : ISoftDeletable
{
    public int IdRes { get; set; }
    public bool IsDeleted { get; set; }
    public string NameRes { get; set; } = string.Empty;

    public List<ClassRoom> ClassRooms { get; set; } = new();
}