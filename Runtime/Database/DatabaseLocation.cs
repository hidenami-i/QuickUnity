namespace QuickUnity.Core
{
    public static class DatabaseLocation
    {
        public static string RootFolderPath()
        {
#if UNITY_EDITOR
            return PathCache.DataPath;
#else
			return PathCache.PersistentDataPath;
#endif
        }
    }
}
