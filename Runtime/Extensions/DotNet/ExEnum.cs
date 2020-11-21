using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickUnity.Extensions.DotNet
{
    public static class ExEnum
    {
        /// <summary>
        /// Cast the enumerated type to the IEnumerable type and return it.
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<T> ToIEnumerable<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Cast the enumerated type to the IEnumerable type and return it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ToList<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static string GetName<T>(T value) where T : struct, IComparable, IFormattable, IConvertible
        {
            return Enum.GetName(typeof(T), value);
        }

        public static string Format<T>(T value, string format) where T : struct, IComparable, IFormattable, IConvertible
        {
            return Enum.Format(typeof(T), value, format);
        }

        /// <summary>
        /// Converts the specified integer value to an enumerated type and return it.
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromInt<T>(int value) where T : struct, IComparable, IFormattable, IConvertible
        {
            return (T) Enum.ToObject(typeof(T), value);
        }

        /// <summary>
        /// Returns the name of the enumerated type as a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> ToNameList<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            List<T> list = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            List<string> result = new List<string>();
            foreach (T item in list)
            {
                result.Add(item.ToString());
            }

            return result;
        }

        /// <summary>
        /// Gets the number of items defined by the enumeration type.
        /// </summary>
        public static int Count<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            return Enum.GetNames(typeof(T)).CountIfNullZero();
        }

        /// <summary>
        /// Converts string type to enumerated type.
        /// </summary>
        /// <returns>The enum.</returns>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T FromString<T>(string value) where T : struct, IComparable, IFormattable, IConvertible
        {
            return (T) Enum.Parse(typeof(T), value, false);
        }

        /// <summary>
        /// Returns whether the specified string can be converted to an enumerated type or not.
        /// </summary>
        public static bool IsEnum<T>(string value) where T : struct, IComparable, IFormattable, IConvertible
        {
            return TryParse(value, out T _);
        }

        /// <summary>
        /// Returns whether the conversion of the specified string to an enumerated type was successful or not.
        /// </summary>
        public static bool TryParse<T>(string value, out T result)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            return TryParse(value, true, out result);
        }

        /// <summary>
        /// Returns whether the conversion of the specified string to an enumerated type was successful or not.
        /// </summary>
        public static bool TryParse<T>(string value, bool ignoreCase, out T result)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            try
            {
                result = (T) Enum.Parse(typeof(T), value, ignoreCase);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }
    }
}