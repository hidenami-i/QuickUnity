namespace QuickUnity.Networking
{
    /// <summary>
    /// <see cref="https://stackoverrun.com/ja/q/2271458"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWebResponse<out T>
    {
        T Result();
    }
}
