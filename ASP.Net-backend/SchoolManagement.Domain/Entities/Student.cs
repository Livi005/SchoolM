using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Estudiante del centro.
/// </summary>
public class Student : ISoftDeletable
{
    public int IdStud { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string NameStud { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool EActivity { get; set; }

    public int IdC { get; set; }
    public Course Course { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public List<Subject> Subjects { get; set; } = new();
    public List<Professor> Professors { get; set; } = new();
}
