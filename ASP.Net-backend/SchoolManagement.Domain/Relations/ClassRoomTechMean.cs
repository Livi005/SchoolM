using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Relations;

/// <summary>
/// Relación N:N entre aula y medio tecnológico.
/// </summary>
public class ClassRoomTechMean : IEntity
{
    public int IdClassRoomTech { get; set; }
    public int IdClassRoom { get; set; }
    public ClassRoom ClassRoom { get; set; } = null!;
    public int IdTechMean { get; set; }
    public TechnologicalMeans TechnologicalMeans { get; set; } = null!;
}
