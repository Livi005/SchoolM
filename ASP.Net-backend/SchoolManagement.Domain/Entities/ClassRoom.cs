using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Aula física del centro.
/// </summary>
public class ClassRoom : ISoftDeletable
{
    public int IdClassR { get; set; }
    public bool IsAviable { get; set; } = true;
    public bool IsDeleted { get; set; }
    public string Location { get; set; } = string.Empty;

    public List<Subject> Subjects { get; set; } = new();
    public List<Professor> Professors { get; set; } = new();
    public List<Restriction> Restrictions { get; set; } = new();
    public List<TechnologicalMeans> TechnologicalMeans { get; set; } = new();
}