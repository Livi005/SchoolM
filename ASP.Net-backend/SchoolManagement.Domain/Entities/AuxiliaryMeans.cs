namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Medio auxiliar (pizarra, proyector, etc.).
/// </summary>
public class AuxiliaryMeans : Mean
{
    public string Type { get; set; } = string.Empty;
    public List<Subject> Subjects { get; set; } = new();
    public List<Professor> Professors { get; set; } = new();
}