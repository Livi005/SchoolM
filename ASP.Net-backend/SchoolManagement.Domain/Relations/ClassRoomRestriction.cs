using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Relations;

/// <summary>
/// Relación N:N entre aula y restricción.
/// </summary>
public class ClassRoomRestriction : IEntity
{
    public int IdClassRoomRest { get; set; }
    public int IdClassRoom { get; set; }
    public ClassRoom ClassRoom { get; set; } = null!;
    public int IdRest { get; set; }
    public Restriction Restriction { get; set; } = null!;
}