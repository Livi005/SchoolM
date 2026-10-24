using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Relations;

/// <summary>
/// Relación N:N entre estudiante y asignatura. Incluye las ausencias acumuladas.
/// </summary>
public class StudentSubject : IEntity
{
    public int IdStudSub { get; set; }
    public int IdStud { get; set; }
    public Student Student { get; set; } = null!;
    public int IdSub { get; set; }
    public Subject Subject { get; set; } = null!;
    public int NJAbsents { get; set; }

    public List<Professor> Professors { get; set; } = new();
}
