using System.Collections.Generic;
using QuickUnity.Extensions.DotNet;

namespace QuickUnity.Database
{
    public static class Database
    {
        private static readonly Dictionary<string, IDatabase> databaseCache = new Dictionary<string, IDatabase>();

        /// <summary>
        /// Objects that inherit from the IDatabase interface will be added automatically.
        /// </summary>
        /// <param name="database"></param>
        internal static void Add(IDatabase database)
        {
            if (!databaseCache.ContainsKeyIfNullFalse(database.TableName))
            {
                databaseCache.Add(database.TableName, database);
            }
        }

        public static void Load()
        {
            if (databaseCache.IsNullOrEmpty())
            {
                return;
            }

            foreach (IDatabase database in databaseCache.Values)
            {
                database.Load();
            }
        }

        public static void Delete()
        {
            if (databaseCache.IsNullOrEmpty())
            {
                return;
            }

            foreach (IDatabase database in databaseCache.Values)
            {
                database.DeleteContents();
            }
        }
    }
}
