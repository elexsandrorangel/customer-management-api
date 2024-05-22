using CustomerManagement.Models;
using CustomerManagement.Repository.Helper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CustomerManagement.Repository
{
    public abstract class BaseRepository<T, C> : IBaseRepository<T>
        where T : BaseEntity
        where C : DbContext
    {
        protected readonly C Context;

        #region Constructor

        protected BaseRepository(C context)
        {
            Context = context;
        }

        #endregion Constructor

        #region Add

        public virtual async Task<T> AddAsync(T entity)
        {
            var data = Context.Set<T>().Add(entity);
            await SaveAsync();
            return data.Entity;
        }

        public virtual async Task<IEnumerable<T>> AddAsync(IEnumerable<T> entities)
        {
            Context.Set<T>().AddRange(entities);
            await SaveAsync();
            return entities;
        }

        #endregion Add

        #region Count

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().AsNoTracking().LongCountAsync(predicate);
        }

        public virtual async Task<long> CountAsync()
        {
            return await Context.Set<T>().AsNoTracking().LongCountAsync();
        }

        #endregion Count

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id, true) ?? throw new InvalidOperationException("Entity does not exists");
            await DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Detached;

            Context.Set<T>().Remove(entity);
            await SaveAsync();
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().AsNoTracking().Any(predicate);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().AsNoTracking().AnyAsync(predicate);
        }

        #region Get

        public virtual async Task<IEnumerable<T>> GetAsync(int page = 0, int qty = int.MaxValue, bool track = false)
        {
            if (track)
            {
                return await Context.Set<T>()
                    .OrderBy(a => a.CreatedAt).Skip(page * qty)
                    .Take(qty).ToListAsync();
            }
            return await Context.Set<T>()
                .AsNoTracking()
                .OrderBy(a => a.CreatedAt).Skip(page * qty)
                .Take(qty).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> match, int page = 0, int qty = int.MaxValue, bool track = false)
        {
            if (track)
            {
                return await Context.Set<T>().Where(match)
                    .OrderBy(a => a.CreatedAt)
                    .Skip(page * qty).Take(qty).ToListAsync();
            }
            return await Context.Set<T>().Where(match)
                .AsNoTracking()
                .OrderBy(a => a.CreatedAt)
                .Skip(page * qty).Take(qty).ToListAsync();
        }

        public virtual async Task<T?> GetAsync(Guid id, bool track = false)
        {
            return await GetSingleOrDefaultAsync(x => x.Id == id, track);
        }

        public virtual async Task<T?> GetSingleOrDefaultAsync(Expression<Func<T, bool>> match, bool track = false)
        {
            if (track)
            {
                return await Context.Set<T>().FirstOrDefaultAsync(match);
            }
            return await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(match);
        }

        #endregion Get

        public virtual async Task<int> SaveAsync()
        {

            return await Context.SaveChangesAsync();
        }

        public virtual async Task<T> SaveOrUpdateAsync(T entity)
        {
            Context.Set<T>().AddOrUpdate(entity);
            await SaveAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T updated)
        {
            return await SaveOrUpdateAsync(updated);
        }

        public virtual async Task<T> UpdateAsync(T updated, Guid key)
        {
            return await SaveOrUpdateAsync(updated);
        }
    }

}
