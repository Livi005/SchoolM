using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Notifications;

/// <summary>
/// Notificación generada cuando un profesor acumula evaluaciones bajas.
/// </summary>
public class ProfessorNotification : IEntity
{
    public int ProfNotId { get; set; }
    public int IdProf { get; set; }
    public string ProfName { get; set; } = string.Empty;
    public bool BeenSended { get; set; }
    public Professor Professor { get; set; } = null!;
}