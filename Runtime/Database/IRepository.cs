using System;
using System.Collections.Generic;

namespace QuickUnity.Database
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets total entity count.
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Gets total entity count by.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        int CountBy(Predicate<TEntity> match);

        /// <summary>
        /// Determine whether the number of data is null or empty.
        /// </summary>
        /// <returns></returns>
        bool IsNullOrEmpty();

        /// <summary>
        /// Determine whether the number of data is null or empty.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        bool IsNullOrEmptyBy(Predicate<TEntity> match);

        /// <summary>
        /// Determine whether the number of data is not empty.
        /// </summary>
        /// <returns></returns>
        bool IsNotEmpty();

        /// <summary>
        /// Determine whether the number of data is not empty.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        bool IsNotEmptyBy(Predicate<TEntity> match);

        /// <summary>
        /// Add entity.
        /// </summary>
        /// <param name="entity">TEntity</param>
        void Add(TEntity entity);

        /// <summary>
        /// Add entity base.
        /// </summary>
        /// <param name="entityBase"></param>
        void Add(EntityBase entityBase);

        /// <summary>
        /// Add all entity by list object.
        /// </summary>
        /// <param name="list"></param>
        void AddAll(IEnumerable<Dictionary<string, object>> list);

        /// <summary>
        /// Find entity by. But if entity is null it returns default.
        /// </summary>
        /// <param name="match"></param>
        /// <param name="defaultEntity"></param>
        /// <returns></returns>
        TEntity GetByOrDefault(Predicate<TEntity> match, TEntity defaultEntity);

        /// <summary>
        /// Find entity by index. But if entity is null it returns default.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="defaultEntity"></param>
        /// <returns></returns>
        TEntity GetByIndexOrDefault(int index, TEntity defaultEntity);

        /// <summary>
        /// Get fist entity. But if Entity is null it returns default.
        /// </summary>
        /// <param name="defaultEntity"></param>
        /// <returns></returns>
        TEntity GetFirstOrDefault(TEntity defaultEntity);

        /// <summary>
        /// Get last entity. But if Entity is null it returns default.
        /// </summary>
        /// <param name="defaultEntity"></param>
        /// <returns></returns>
        TEntity GetLastOrDefault(TEntity defaultEntity);

        /// <summary>
        /// Find entity by.
        /// </summary>
        /// <param name="match">TEntity</param>
        /// <typeparam name="T">T is class</typeparam>
        /// <returns>TEntity</returns>
        bool TryFindBy(Predicate<TEntity> match, out TEntity entity);

        /// <summary>
        /// Find entity by index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TryFindByIndex(int index, out TEntity entity);

        /// <summary>
        /// Gets fist entity. But if Entity is null it returns false.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TryGetFirst(out TEntity entity);

        /// <summary>
        /// Gets last entity. But if Entity is null it return false.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TryGetLast(out TEntity entity);

        /// <summary>
        /// Find all entity.
        /// </summary>
        /// <returns>IEnumerable<TEntity></returns>
        List<TEntity> FindAll();

        /// <summary>
        /// Find all entity by.
        /// </summary>
        /// <param name="match">TEntity</param>
        /// <returns>IEnumerable<TEntity></returns>
        List<TEntity> FindAllBy(Predicate<TEntity> match);

        /// <summary>
        /// Clear the retrieved data.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        void ClearBy(Predicate<TEntity> match);

        /// <summary>
        /// Delete the all data.
        /// </summary>
        void ClearAll();

        /// <summary>
        /// Delete all searched data.
        /// </summary>
        /// <param name="match"></param>
        void ClearAllBy(Predicate<TEntity> match);
    }
}
