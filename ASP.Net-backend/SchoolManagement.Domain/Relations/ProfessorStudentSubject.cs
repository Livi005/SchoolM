using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Relations;

/// <summary>
/// Relación profesor–estudiante–asignatura con la nota asociada.
/// Es el núcleo de la evaluación.
/// </summary>
public class ProfessorStudentSubject : IEntity
{
    public int IdProfStudSub { get; set; }
    public int IdProf { get; set; }
    public Professor Professor { get; set; } = null!;
    public int IdStudSub { get; set; }
    public StudentSubject StudentSubject { get; set; } = null!;
    public float StudentGrades { get; set; }

    // Antes: { get; } sin setter → EF no podía hidratar la colección.
    public List<Secretary> Secretaries { get; set; } = new();
}