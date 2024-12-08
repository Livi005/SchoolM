using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Common;
using SchoolManagement.Infrastructure.Common.Interfaces;


namespace SchoolManagement.Infrastructure.Common.Implementation
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbSet<TEntity> entity;
        public SchoolManagement_Context _context;
        public GenericRepository(SchoolManagement_Context context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
            // entity = context.Set<TEntity>();
        }

        public async Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default)
        {
            var element = entity.Find(elementId);
            entity.Remove(element);
            // await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync<TId>(TId elementId, CancellationToken cancellationToken = default)
        {
            return await entity.FindAsync(elementId, cancellationToken);
        }


        public async Task<TEntity> CreateAsync(TEntity element, CancellationToken cancellationToken = default)
        {
            entity.Add(element);
            //await _context.SaveChangesAsync(cancellationToken);
            return element;
        }

        public async Task UpdateAsync(TEntity element, CancellationToken cancellationToken = default)
        {
            entity.Update(element);
            // await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default)
        {
            var entities = await entity.ToListAsync(cancellationToken);
            return entities;

        }

        public TEntity GetById<TId>(TId elementId)
        {
            return entity.Find(elementId)!;
        }
    }
}
