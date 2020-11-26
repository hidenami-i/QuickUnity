using System.Collections.Generic;

namespace QuickUnity.Extensions.DotNet
{
    public static class ExDictionary
    {
        /// <summary>
        /// Safely clear the dictionary.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        public static void ClearNotThrow<T, K>(this IDictionary<T, K> dictionary)
        {
            if (dictionary.IsNullOrEmpty())
            {
                return;
            }

            dictionary.Clear();
        }

        /// <summary>
        /// Search securely whether key is included.
        /// </summary>
        /// <returns><c>true</c>, if contains key was safe, <c>false</c> otherwise.</returns>
        /// <param name="dictionary"></param>
        /// <param name="key">Key.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="K">The 2nd type parameter.</typeparam>
        public static bool ContainsKeyIfNullFalse<T, K>(this IDictionary<T, K> dictionary, T key)
        {
            return !dictionary.IsNullOrEmpty() && dictionary.ContainsKey(key);
        }
    }
}
