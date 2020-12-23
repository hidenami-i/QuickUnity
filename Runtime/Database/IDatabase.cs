namespace QuickUnity.Core
{
    public interface IDatabase : ISavable
    {
        /// <summary>
        /// Database schema
        /// </summary>
        string Schema { get; }
    }
}