using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Relations;

namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Profesor del centro.
/// </summary>
public class Professor : ISoftDeletable
{
    public int IdProf { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string NameProf { get; set; } = string.Empty;
    public string Contract { get; set; } = string.Empty;
    public string Spec { get; set; } = string.Empty;
    public int Salary { get; set; }
    public bool IsDean { get; set; }
    public int LaboralExperience { get; set; }
    public bool IsDeleted { get; set; }
    public bool UseAuxMean { get; set; }

    public List<Subject> Subjects { get; set; } = new();
    public List<AuxiliaryMeans> AuxiliaryMeans { get; set; } = new();
    public List<ClassRoom> ClassRooms { get; set; } = new();
    public List<StudentSubject> StudentSubjects { get; set; } = new();
}