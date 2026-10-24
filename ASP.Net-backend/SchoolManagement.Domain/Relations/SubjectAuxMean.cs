using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Domain.Relations;

/// <summary>
/// Relación N:N entre asignatura y medio auxiliar.
/// </summary>
public class SubjectAuxMean : IEntity
{
    public int IdSubAuxMean { get; set; }
    public int IdSub { get; set; }
    public Subject Subject { get; set; } = null!;

    // Antes era un campo público: `public int IdAuxMean = 0;`
    public int IdAuxMean { get; set; }
    public AuxiliaryMeans AuxMean { get; set; } = null!;
}
