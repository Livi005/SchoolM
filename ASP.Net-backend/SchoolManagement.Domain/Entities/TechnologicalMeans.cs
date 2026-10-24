namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Medio tecnológico (PC, tablet, etc.).
/// </summary>
public class TechnologicalMeans : Mean
{
    public List<ClassRoom> ClassRooms { get; set; } = new();
}