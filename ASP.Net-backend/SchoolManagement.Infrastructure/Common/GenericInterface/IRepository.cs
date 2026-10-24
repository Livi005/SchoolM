using System.Linq.Expressions;
using SchoolManagement.Domain.Common;

namespace SchoolManagement.Infrastructure.Common.GenericInterface;

/// <summary>
/// Repositorio genérico para cualquier entidad del dominio.
/// No guarda cambios: eso lo hace <see cref="IUnitOfWork"/>.
/// </summary>
public interface IRepository<TEntity, in TId> where TEntity : class, IEntity
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);

    /// <summary>Query libre para composición con Include/Where/Select.</summary>
    IQueryable<TEntity> Query();
}