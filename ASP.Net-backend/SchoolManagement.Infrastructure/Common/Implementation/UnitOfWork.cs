using System.Collections.Concurrent;
using SchoolManagement.Domain.Common;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Infrastructure.Common.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly Context _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public UnitOfWork(Context context) => _context = context;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public IRepository<TEntity, int> Repository<TEntity>() where TEntity : class, IEntity
    {
        return (IRepository<TEntity, int>)_repositories.GetOrAdd(
            typeof(TEntity),
            _ => new GenericRepository<TEntity, int>(_context));
    }
}