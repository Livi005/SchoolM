using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolManagement.Domain.Common;


namespace SchoolManagement.Infrastructure.Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity:BaseEntity
    {
        Task <TEntity>CreateAsync(TEntity element, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity element, CancellationToken cancellationToken = default);
        Task<TEntity> GetByIdAsync<TId>(TId elementId, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);
        
        Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default);
        TEntity GetById<TId>(TId elementId);
        
    }
}