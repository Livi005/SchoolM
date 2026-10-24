using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Sanción aplicada a un profesor.
/// </summary>
public class ProfessorPunishment : IEntity
{
    public int IdPun { get; set; }
    public int IdProf { get; set; }
    public Professor Professor { get; set; } = null!;
    public DateTime PunishmentDate { get; set; }
}