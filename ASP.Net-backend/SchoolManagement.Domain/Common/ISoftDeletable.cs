namespace SchoolManagement.Domain.Common;

/// <summary>
/// Entidad que soporta borrado lógico (soft delete).
/// </summary>
public interface ISoftDeletable : IEntity
{
    bool IsDeleted { get; set; }
}