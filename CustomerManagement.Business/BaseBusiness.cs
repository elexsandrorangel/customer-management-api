using AutoMapper;
using CustomerManagement.Infra.Core.Exceptions;
using CustomerManagement.Models;
using CustomerManagement.Repository;
using CustomerManagement.ViewModels;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Business
{
    public abstract class BaseBusiness<TEntity, TModel, TRepo> : IBaseBusiness<TEntity, TModel>
        where TEntity : BaseEntity
        where TModel : BaseViewModel
        where TRepo : IBaseRepository<TEntity>
    {
        #region Fields

        protected readonly TRepo Repository;

        protected readonly IMapper _mapper;

        protected readonly ILogger _logger;

        #endregion Fields

        #region Ctor

        protected BaseBusiness(TRepo repository, IMapper mapper, ILogger logger)
        {
            Repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion Ctor

        #region Mapper

        /// <summary>
        /// Converts a entity object to value object
        /// </summary>
        /// <param name="t"><paramref name="t"/> Entity class</param>
        /// <returns>Converted model</returns>
        internal TModel? ModelFromEntity(TEntity? t)
        {
            return t == null ? null : _mapper.Map<TModel>(t);
        }

        /// <summary>
        /// Converts a list of entities object to value object enumerable
        /// </summary>
        /// <param name="entities"><paramref name="entities"/> Entity class</param>
        /// <returns></returns>
        internal IEnumerable<TModel>? ModelFromEntity(IEnumerable<TEntity>? entities)
        {
            return entities == null ? null : _mapper.Map<IEnumerable<TModel>>(entities);
        }

        internal TEntity EntityFromModel(TModel model)
        {
            return _mapper.Map<TEntity>(model);
        }

        internal IEnumerable<TEntity> EntityFromModel(IEnumerable<TModel> models)
        {
            return _mapper.Map<IEnumerable<TEntity>>(models);
        }

        #endregion Mapper

        #region Add

        public virtual async Task<TModel> AddAsync(TModel t)
        {
            ValidateInsert(t);

            t.Active = true;
            return ModelFromEntity(await Repository.AddAsync(EntityFromModel(t)))!;
        }

        public virtual async Task<IEnumerable<TModel>> AddAsync(IEnumerable<TModel> tList)
        {
            var list = tList;

            ValidateInsert(tList);
            return ModelFromEntity(await Repository.AddAsync(EntityFromModel(list)))!;
        }

        #endregion Add

        #region Count

        public virtual async Task<long> CountAsync()
        {
            return await Repository.CountAsync();
        }

        #endregion Count

        #region Delete

        public virtual async Task DeleteAsync(Guid id)
        {
            await InactivateAsync(id, null);
        }

        public virtual async Task DeleteAsync(TModel t)
        {
            await ValidateDeleteAsync(t);

            await InactivateAsync(t.Id, null);
        }

        #endregion Delete

        #region Get

        public virtual async Task<IEnumerable<TModel>> GetAsync(int page = 1, int qty = int.MaxValue)
        {
            if (page > 0)
            {
                page -= 1;
            }
            else
            {
                page = 0;
            }
            return ModelFromEntity(await Repository.GetNotDeletedAsync(page, qty, false))!;
        }

        public virtual async Task<TModel?> GetAsync(Guid id, bool track = false)
        {
            return ModelFromEntity(await Repository.GetNotDeletedAsync(id, track));
        }

        #endregion Get

        #region Save

        public virtual async Task<TModel> SaveOrUpdateAsync(TModel t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }
            return await SaveEntityAsync(t);
        }

        private async Task<TModel> SaveEntityAsync(TModel t)
        {
            return t!.Id == Guid.Empty ? await AddAsync(t) : await UpdateAsync(t, t.Id);
        }

        #endregion Save

        #region Update

        public virtual async Task<bool> InactivateAsync(Guid id, int? inactivatedBy)
        {
            TModel? obj = await GetAsync(id, false);
            if (obj == null)
            {
                throw new AppNotFoundException();
            }

            await ValidateDeleteAsync(obj);

            obj.IsDeleted = true;
            await Repository.UpdateAsync(EntityFromModel(obj));
            return true;
        }

        public virtual async Task<TModel> UpdateAsync(TModel updated)
        {
            await ValidateUpdateAsync(updated);

            TModel? record = await GetAsync(updated.Id, false);

            if (record == null)
            {
                throw new AppNotFoundException();
            }

            updated.TenantId = record.TenantId;
            updated.CreatedAt = record.CreatedAt;

            var data = await Repository.UpdateAsync(EntityFromModel(updated));
            return ModelFromEntity(data)!;
        }

        public virtual async Task<TModel> UpdateAsync(TModel updated, Guid key)
        {
            await ValidateUpdateAsync(updated);

            TModel? record = await GetAsync(key, true);

            if (record == null)
            {
                throw new AppNotFoundException();
            }

            updated.Id = key;
            updated.TenantId = record.TenantId;
            updated.CreatedAt = record.CreatedAt;

            var data = await Repository.UpdateAsync(EntityFromModel(updated), key);
            return ModelFromEntity(data)!;
        }

        #endregion Update

        #region Validations

        /// <summary>
        /// Data validation before insert model into database
        /// </summary>
        /// <param name="model">Model to insert</param>
        /// <exception cref="AppException">Business exception</exception>
        protected abstract void ValidateInsert(TModel model);

        /// <summary>
        /// Data validation before insert model into database
        /// </summary>
        /// <param name="models">Models to insert</param>
        /// <exception cref="AppException">Business exception</exception>
        protected virtual void ValidateInsert(IEnumerable<TModel> models)
        {
            foreach (var item in models)
            {
                ValidateInsert(item);
            }
        }

        /// <summary>
        /// Data validation before update model into database
        /// </summary>
        /// <param name="model">Model to update</param>
        /// <remarks>Asynchronous</remarks>
        /// <exception cref="AppException">Business exception</exception>
        protected virtual async Task ValidateUpdateAsync(TModel model)
        {
            if (model == null || model!.Id == Guid.Empty)
            {
                throw new AppException();
            }
            var record = await GetAsync(model.Id, false);
            if (record == null)
            {
                throw new AppNotFoundException();
            }

            ValidateInsert(model);
        }

        /// <summary>
        /// Data validation before remove model from database
        /// </summary>
        /// <param name="model">Model to remove</param>
        /// <remarks>Asynchronous</remarks>
        /// <exception cref="AppException">Business exception</exception>
        protected virtual async Task ValidateDeleteAsync(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Id == Guid.Empty)
            {
                throw new AppException();
            }
            var record = await GetAsync(model.Id, false);
            if (record == null)
            {
                throw new AppNotFoundException();
            }
        }

        protected async Task<TEntity> GetEntityOrThrowAsync(TModel model)
        {
            var data = await Repository.GetAsync(model.Id, false);

            if (data == null)
            {
                throw new AppNotFoundException();
            }

            return data;
        }

        #endregion Validations
    }
}
