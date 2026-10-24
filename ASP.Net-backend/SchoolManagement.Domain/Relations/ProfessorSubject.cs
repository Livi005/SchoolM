using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Relations;

/// <summary>
/// Relación N:N entre profesor y asignatura.
/// </summary>
public class ProfessorSubject : IEntity
{
    public int IdProfSub { get; set; }
    public int IdProf { get; set; }
    public Professor Professor { get; set; } = null!;
    public int IdSub { get; set; }
    public Subject Subject { get; set; } = null!;
}
