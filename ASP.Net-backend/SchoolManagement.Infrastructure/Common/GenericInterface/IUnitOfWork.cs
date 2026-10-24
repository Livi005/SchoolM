namespace SchoolManagement.Infrastructure.Common.GenericInterface;

/// <summary>
/// Coordina los repositorios y confirma los cambios contra la base de datos.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>Persiste todos los cambios pendientes en una sola transacción.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>Acceso al repositorio genérico de una entidad concreta.</summary>
    IRepository<TEntity, int> Repository<TEntity>() where TEntity : class, Domain.Common.IEntity;
}