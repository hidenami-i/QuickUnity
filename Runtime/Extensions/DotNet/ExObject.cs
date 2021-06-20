using System.Collections.Generic;

namespace QuickUnity.Extensions.DotNet
{
    public static class ExObject
    {
        /// <summary>
        /// Convert the object to an int type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this object obj, int defaultValue = 0) =>
            obj.ToString().ToIntIfNullOrDefault(defaultValue);

        /// <summary>
        /// Convert the object to a float type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(this object obj, float defaultValue = 0.0f) =>
            obj.ToString().ToFloatIfNullOrDefault(defaultValue);

        /// <summary>
        /// Convert the object to a double type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this object obj, double defaultValue = 0.0d) =>
            obj.ToString().ToDoubleIfNullOrDefault(defaultValue);

        /// <summary>
        /// Convert the object to a long type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(this object obj, long defaultValue = 0L) =>
            obj.ToString().ToLongIfNullOrDefault(defaultValue);

        /// <summary>
        /// Convert the object to a Dictionary type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this object obj) => obj as Dictionary<string, object>;

        /// <summary>
        /// Convert the object to a Dictionary type.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionaryByKey(this object obj, string key)
        {
            var data = ToDictionary(obj);
            if (!data.TryGetValue(key, out object _))
            {
                return new Dictionary<string, object>();
            }

            var value = data[key] as Dictionary<string, object>;
            return value;
        }

        /// <summary>
        /// Gets the value of the dictionary from the object and converts it to list.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<object> ToListFromDictionaryByKey(this object obj, string key)
        {
            var data = ToDictionary(obj);
            if (!data.TryGetValue(key, out object _))
            {
                return new List<object>();
            }

            var list = data[key] as List<object>;
            return list;
        }

        /// <summary>
        /// Gets the value of the dictionary from the object and converts it to list.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ToListFromDictionaryByKey<T>(this object obj, string key)
        {
            var data = ToDictionary(obj);
            if (!data.TryGetValue(key, out object _))
            {
                return new List<T>();
            }

            var list = data[key] as List<T>;
            return list;
        }
    }
}