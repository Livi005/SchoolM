using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Relations;

/// <summary>
/// Relación N:N entre profesor y aula (reserva/asignación de aula).
/// </summary>
public class ProfessorClassRoom : IEntity
{
    public int IdProfClass { get; set; }
    public int IdProf { get; set; }
    public Professor Professor { get; set; } = null!;
    public int IdClassR { get; set; }
    public ClassRoom ClassRoom { get; set; } = null!;
}