using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Relations;

/// <summary>
/// Relación N:N entre secretario y evaluación (profesor–estudiante–asignatura).
/// </summary>
public class SecretaryProfessorStudentSubject : IEntity
{
    public int IdSecProfStudSub { get; set; }
    public int IdSec { get; set; }
    public Secretary Secretary { get; set; } = null!;
    public int IdProfStudSub { get; set; }
    public ProfessorStudentSubject Evaluation { get; set; } = null!;
}