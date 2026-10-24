using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Common;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Infrastructure.Common.Implementation;

/// <summary>
/// Implementación genérica de <see cref="IRepository{TEntity,TId}"/> sobre EF Core.
/// No guarda cambios: usa <see cref="IUnitOfWork.SaveChangesAsync"/>.
/// </summary>
public class GenericRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(Context context)
    {
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync(new object?[] { id }, cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => _dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
        => _dbSet.AsNoTracking().CountAsync(cancellationToken);

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => await _dbSet.AddRangeAsync(entities, cancellationToken);

    public void Update(TEntity entity) => _dbSet.Update(entity);

    /// <summary>
    /// Si la entidad soporta borrado lógico, lo marca. Si no, la elimina físicamente.
    /// </summary>
    public void Remove(TEntity entity)
    {
        if (entity is ISoftDeletable softDeletable)
        {
            softDeletable.IsDeleted = true;
            _dbSet.Update(entity);
        }
        else
        {
            _dbSet.Remove(entity);
        }
    }

    public IQueryable<TEntity> Query() => _dbSet.AsQueryable();
}