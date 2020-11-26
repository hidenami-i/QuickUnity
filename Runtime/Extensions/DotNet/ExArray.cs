namespace QuickUnity.Extensions.DotNet
{
    public static class ExArray
    {
        /// <summary>
        /// Returns the array of the length.
        /// If the array is null it returns 0;
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        public static int LengthIfNullZero<T>(this T[] self) => self.IsNullOrEmpty() ? 0 : self.Length;

        /// <summary>
        /// Returns the value randomly from the specified array.
        /// </summary>
        public static T Random<T>(params T[] values) => values[UnityEngine.Random.Range(0, values.Length)];
    }
}
