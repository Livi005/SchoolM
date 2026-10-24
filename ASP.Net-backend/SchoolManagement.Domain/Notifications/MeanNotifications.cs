using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Notifications;

/// <summary>
/// Notificación generada cuando un medio supera el umbral de mantenimientos en un periodo.
/// </summary>
public class MeanNotification : IEntity
{
    public int MeanNotId { get; set; }
    public int IdMean { get; set; }
    public string MeanName { get; set; } = string.Empty;
    public string MeanType { get; set; } = string.Empty;
    public bool BeenSended { get; set; }
}