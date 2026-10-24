using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Curso académico (año escolar).
/// </summary>
public class Course : ISoftDeletable
{
    public int IdC { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public List<Student> Students { get; set; } = new();
}