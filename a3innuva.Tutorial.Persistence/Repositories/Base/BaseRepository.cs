namespace a3innuva.Tutorial.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using a3innuva.Tutorial.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public abstract class BaseRepository<TInterface, TEntity> : IRepository<TInterface, TEntity>
        where TInterface : class, IEntity
        where TEntity : class, TInterface
    {
        protected readonly AppDbContext Context;

        protected BaseRepository(AppDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task AddAsync(TInterface entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await this.Context.Set<TEntity>().AddAsync(entity as TEntity).ConfigureAwait(false);
            await this.Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TInterface entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            this.Context.Set<TEntity>().Update(entity as TEntity);
            await this.Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(TInterface entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            this.Context.Set<TEntity>().Remove(entity as TEntity);
            await this.Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            TInterface entity = await this.Context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
            this.Context.Set<TEntity>().Remove(entity as TEntity);

            await this.Context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task<TInterface> GetByIdAsync(Guid id)
        {
            return await this.Context.Set<TEntity>().FindAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<IList<TInterface>> GetAllAsync()
        {
            var entity = await this.Context.Set<TEntity>().ToListAsync().ConfigureAwait(false);

            return entity.OfType<TInterface>().ToList();
        }

        public virtual async Task<IList<TInterface>> Filter(Expression<Func<TInterface, bool>> predicate)
        {
            var query = this.Context.Set<TEntity>().Where(predicate);

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<TInterface> FindOne(Expression<Func<TInterface, bool>> predicate)
        {
            var query = this.Context.Set<TEntity>().Where(predicate);

            if (!query.Any())
                return null;

            return await query.FirstAsync().ConfigureAwait(false);
        }
    }
}
