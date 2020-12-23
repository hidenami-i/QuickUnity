namespace QuickUnity.Core
{
    /// <summary>
    /// Interface for persisting a single Entity object.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDataMapper<TEntity> where TEntity : class
    {
        /// <summary>
        /// Setup DataMapper Me from TEntity.
        /// </summary>
        void Update(TEntity entity);

        /// <summary>
        /// Try get TEntity instance.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TryGet(out TEntity entity);

        /// <summary>
        /// Get TEntity instance.
        /// If the entity is null, it returns the default value. 
        /// </summary>
        /// <param name="defaultEntity"></param>
        /// <returns></returns>
        TEntity GetOrDefault(TEntity defaultEntity);

        /// <summary>
        /// Delete entity.
        /// </summary>
        void Delete();
    }
}
