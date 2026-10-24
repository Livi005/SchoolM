using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Relations;

/// <summary>
/// Relación profesor–estudiante–asignatura–curso con la evaluación asociada (0–10).
/// </summary>
public class ProfStudSubCourse : IEntity
{
    public int IdProfStudSubCourse { get; set; }

    public int IdProf { get; set; }
    public Professor Professor { get; set; } = null!;

    public int IdStud { get; set; }
    public Student Student { get; set; } = null!;

    public int IdSub { get; set; }
    public Subject Subject { get; set; } = null!;

    public int IdCourse { get; set; }
    public Course Course { get; set; } = null!;

    /// <summary>Evaluación del profesor (0–10).</summary>
    public int Evaluation { get; set; }
}
