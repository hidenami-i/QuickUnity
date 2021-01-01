namespace QuickUnity.Database
{
    public interface IDatabase
    {
        /// <summary>
        /// Database schema
        /// </summary>
        string Schema { get; }

        /// <summary>
        /// Me type TableName
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Setup Database Me from json.
        /// </summary>
        /// <param name="json"></param>
        void FromJson(string json);

        /// <summary>
        /// Database instance to json data.
        /// </summary>
        /// <param name="prettyPrint"></param>
        /// <returns></returns>
        string ToJson(bool prettyPrint = true);

        /// <summary>
        /// Gets the hashCode of the Database object.
        /// </summary>
        /// <returns></returns>
        string GetContentsHash();
    }
}
