using SchoolManagement.Domain.Common;

namespace SchoolManagement.Domain.Entities;

/// <summary>
/// Clase base abstracta para los medios (recursos) del centro.
/// No se instancia directamente: se usan <see cref="AuxiliaryMeans"/> o <see cref="TechnologicalMeans"/>.
/// </summary>
public abstract class Mean : ISoftDeletable
{
    public int IdMean { get; set; }
    public bool IsAviable { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
    public string NameMean { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public List<Maintenance> Maintenances { get; set; } = new();
}