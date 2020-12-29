using QuickUnity.Extensions.Security;
using UnityEngine;

namespace QuickUnity.Database
{
    /// <summary>
    /// Object for persistent single entity.
    /// DataMapper is Data access object for single entity.
    /// </summary>
    public abstract class DataMapperBase<TEntity, KDataMapper> : IDataMapper<TEntity> where TEntity : class, new()
        where KDataMapper : class, IDataMapper<TEntity>, new()
    {
        /// <summary>
        /// A single Entity object.
        /// </summary>
        protected abstract TEntity Entity { get; set; }

        /// <summary>
        /// DataMapper instance cache.
        /// </summary>
        private static KDataMapper instance;

        /// <summary>
        /// KDataMapper Me.
        /// </summary>
        public static KDataMapper Me => instance ??= new KDataMapper();

        /// <summary>
        /// Gets DataMapper name.
        /// </summary>
        public string TableName => typeof(KDataMapper).Name;

        /// <summary>
        /// Gets Entity name.
        /// </summary>
        public string EntityName => typeof(TEntity).Name;

        public void FromJson(string json) => instance = JsonUtility.FromJson<KDataMapper>(json);

        public void Update(TEntity entity) => Entity = entity;

        public virtual string ToJson(bool prettyPrint = false) => JsonUtility.ToJson(Me, prettyPrint);

        public string GetContentsHash() => PBKDF25.Encrypt(ToJson());

        public bool TryGet(out TEntity entity)
        {
            entity = Entity;
            return entity != null;
        }

        public TEntity GetOrDefault(TEntity defaultEntity)
        {
            TEntity result = defaultEntity;

            if (Entity != null)
            {
                result = Entity;
            }

            return result;
        }

        public void Delete() => Entity = null;
    }
}
