using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Mantenimiento realizado a un medio (auxiliar o tecnológico).
/// </summary>
public class Maintenance : ISoftDeletable
{
    public int IdM { get; set; }
    public DateOnly MaintenanceDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public int Cost { get; set; }
    public bool IsDeleted { get; set; }

    public MeanType TypeOfMean { get; set; } = MeanType.Technological;

    public int? IdAuxMean { get; set; }
    public AuxiliaryMeans? AuxMean { get; set; }
    public int? IdTechMean { get; set; }
    public TechnologicalMeans? TechnologicalMean { get; set; }
}