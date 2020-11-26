namespace QuickUnity.Database
{
    public interface ISavable
    {
        /// <summary>
        /// Initialize Database Me from json.
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
        /// Gets the hashCode of the Databse object.
        /// </summary>
        /// <returns></returns>
        string GetContentsHash();

        /// <summary>
        /// Me type Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Entity type name
        /// </summary>
        string EntityName { get; }
    }
}
