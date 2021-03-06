using System;
using System.Collections.Generic;
using QuickUnity.Extensions.DotNet;
using QuickUnity.Extensions.Security;
using QuickUnity.Extensions.Unity;
using UnityEngine;

namespace QuickUnity.Database
{
    /// <summary>
    /// Objects for persisting multiple Entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="KRepository"></typeparam>
    public abstract class RepositoryBase<TEntity, KRepository> : IRepository<TEntity> where TEntity : EntityBase, new()
        where KRepository : IRepository<TEntity>, new()
    {
        private static readonly object LockObject = new();

        /// <summary> TEntity List. </summary>
        protected abstract List<TEntity> EntityList { get; }

        /// <summary> Repository instance cache. </summary>
        private static KRepository instance;

        /// <summary> Repository instance. </summary>
        public static KRepository Me => instance ??= new KRepository();

        /// <summary> Gets Table name. </summary>
        public virtual string TableName => typeof(KRepository).Name;

        /// <summary> Gets Physical name. </summary>
        public virtual string PhysicalName => typeof(KRepository).Name;

        public void FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            instance = JsonUtility.FromJson<KRepository>(json);
        }

        public void AddFromJson(string json)
        {
            if (EntityList == null)
            {
                FromJson(json);
                return;
            }

            var repository = JsonUtility.FromJson<KRepository>(json);
            lock (LockObject)
            {
                EntityList.AddRange(repository.FindAll());
            }
        }

        public string ToJson(bool prettyPrint = false) => JsonUtility.ToJson(Me, prettyPrint);
        public virtual string GetContentsHash() => PBKDF25.Encrypt(ToJson());
        public int Count() => EntityList.CountIfNullZero();
        public int CountBy(Predicate<TEntity> match) => FindAllBy(match).CountIfNullZero();
        public bool IsNullOrEmpty() => EntityList.IsNullOrEmpty();
        public bool IsNullOrEmptyBy(Predicate<TEntity> match) => FindAllBy(match).IsNullOrEmpty();
        public bool IsNotEmpty() => EntityList.IsNotEmpty();
        public bool IsNotEmptyBy(Predicate<TEntity> match) => FindAllBy(match).IsNotEmpty();

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                ExDebug.LogError($"{typeof(TEntity).Name} is null.");
                return;
            }

            EntityList.Add(entity);
        }

        public void AddThreadSafe(TEntity entity)
        {
            if (entity == null)
            {
                ExDebug.LogError($"{typeof(TEntity).Name} is null.");
                return;
            }

            lock (LockObject)
            {
                EntityList.Add(entity);
            }
        }

        public void Add(EntityBase entityBase)
        {
            if (entityBase == null)
            {
                ExDebug.LogError($"{typeof(TEntity).Name} is null.");
                return;
            }

            TEntity entity = entityBase as TEntity;
            EntityList.Add(entity);
        }

        public void AddThreadSafe(EntityBase entityBase)
        {
            if (entityBase == null)
            {
                ExDebug.LogError($"{typeof(TEntity).Name} is null.");
                return;
            }

            TEntity entity = entityBase as TEntity;
            lock (LockObject)
            {
                EntityList.Add(entity);
            }
        }

        public void AddAll(params EntityBase[] entityBases)
        {
            if (entityBases.IsNullOrEmpty())
            {
                return;
            }

            ClearAll();
            foreach (var entityBase in entityBases)
            {
                Add(entityBase);
            }
        }

        public void AddAll(IEnumerable<Dictionary<string, object>> list)
        {
            if (list.IsNullOrEmpty())
            {
                return;
            }

            ClearAll();
            foreach (var values in list)
            {
                TEntity entity = new TEntity();
                entity.SetField(values);
                Add(entity);
            }
        }

        /// <summary>
        /// Gets Entity Data.
        /// If the data does not exist, null is returned.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public TEntity GetBy(Predicate<TEntity> match)
        {
            return GetByOrDefault(match, null);
        }

        /// <summary>
        /// Gets First Entity Data.
        /// If the data does not exist, null is returned.
        /// </summary>
        /// <returns></returns>
        public TEntity GetFirst()
        {
            return GetFirstOrDefault(null);
        }

        /// <summary>
        /// Gets Last Entity Data.
        /// If the data does not exist, null is returned.
        /// </summary>
        /// <returns></returns>
        public TEntity GetLast()
        {
            return GetLastOrDefault(null);
        }

        public TEntity GetByOrDefault(Predicate<TEntity> match, TEntity defaultEntity)
        {
            TEntity result = defaultEntity;

            if (TryFindBy(match, out TEntity entity))
            {
                result = entity;
            }

            return result;
        }

        public TEntity GetByIndexOrDefault(int index, TEntity defaultEntity)
        {
            TEntity result = defaultEntity;

            if (TryFindByIndex(index, out TEntity entity))
            {
                result = entity;
            }

            return result;
        }

        public TEntity GetFirstOrDefault(TEntity defaultEntity)
        {
            TEntity result = defaultEntity;

            if (TryGetFirst(out TEntity entity))
            {
                result = entity;
            }

            return result;
        }

        public TEntity GetLastOrDefault(TEntity defaultEntity)
        {
            TEntity result = defaultEntity;

            if (TryGetLast(out TEntity entity))
            {
                result = entity;
            }

            return result;
        }

        public bool TryFindBy(Predicate<TEntity> match, out TEntity entity)
        {
            entity = EntityList.Find(match);
            return entity != null;
        }

        public bool TryFindByIndex(int index, out TEntity entity)
        {
            entity = null;

            if (index < 0 || index > Count())
            {
                return false;
            }

            entity = EntityList[index];
            return entity != null;
        }

        public bool TryGetFirst(out TEntity entity)
        {
            entity = null;
            return TryFindByIndex(0, out entity);
        }

        public bool TryGetLast(out TEntity entity)
        {
            entity = null;
            return TryFindByIndex(Count(), out entity);
        }

        public List<TEntity> FindAll() => EntityList;

        public List<TEntity> FindAllBy(Predicate<TEntity> match) => EntityList.FindAll(match);

        public void ClearBy(Predicate<TEntity> match)
        {
            if (TryFindBy(match, out TEntity e))
            {
                EntityList.Remove(e);
                return;
            }

            ExDebug.LogWarning("Not found Entity");
        }

        public void ClearAll() => EntityList.ClearNotThrow();

        public void ClearAllBy(Predicate<TEntity> match)
        {
            var list = FindAllBy(match);
            foreach (var entity in list)
            {
                EntityList.Remove(entity);
            }
        }

        public void LogAllEntity()
        {
            if (IsNullOrEmpty())
            {
                Debug.LogWarning($"{TableName} data is null or empty.");
                return;
            }

            foreach (var entity in FindAll())
            {
                Debug.Log(entity.ToString());
            }
        }
    }
}