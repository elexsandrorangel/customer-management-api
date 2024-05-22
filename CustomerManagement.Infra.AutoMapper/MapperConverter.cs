using AutoMapper;

namespace CustomerManagement.Infra.AutoMapper
{
    public class MapperConverter<TModel, TEntity>
        where TModel : class
        where TEntity : class
    {
        private readonly IMapper _mapper;

        public MapperConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        #region Mapper

        /// <summary>
        /// Converts a entity object to value object
        /// </summary>
        /// <param name="t"><paramref name="t"/> Entity class</param>
        /// <returns>Converted model</returns>
        public TModel? ModelFromEntity(TEntity? t)
        {
            return t == null ? null : _mapper.Map<TModel>(t);
        }

        /// <summary>
        /// Converts a list of entities object to value object enumerable
        /// </summary>
        /// <param name="entities"><paramref name="entities"/> Entity class</param>
        /// <returns></returns>
        public IEnumerable<TModel>? ModelFromEntity(IEnumerable<TEntity>? entities)
        {
            return entities == null ? null : _mapper.Map<IEnumerable<TModel>>(entities);
        }

        public TEntity EntityFromModel(TModel model)
        {
            return _mapper.Map<TEntity>(model);
        }

        public IEnumerable<TEntity> EntityFromModel(IEnumerable<TModel> models)
        {
            return _mapper.Map<IEnumerable<TEntity>>(models);
        }

        #endregion Mapper

    }
}
