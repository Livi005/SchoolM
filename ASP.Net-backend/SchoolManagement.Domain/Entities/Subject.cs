using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Asignatura del plan de estudios.
/// </summary>
public class Subject : ISoftDeletable
{
    public int IdSub { get; set; }
    public string NameSub { get; set; } = string.Empty;
    public string StudyProgram { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public int CourseLoad { get; set; }

    public int IdClassRoom { get; set; }
    public ClassRoom ClassRoom { get; set; } = null!;

    public List<Professor> Professors { get; set; } = new();
    public List<Student> Students { get; set; } = new();
    public List<AuxiliaryMeans> AuxiliaryMeans { get; set; } = new();
}