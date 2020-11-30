using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace QuickUnity.Extensions.DotNet
{
    public static class ExString
    {
        private const int IndexNotFound = -1;
        private const string Empty = "";
        private const string NumericChars = "0123456789";
        private const string PasswordChars = "0123456789abcdefghijklmnopqrstuvwxyz";
        private const string UniqueChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Cut out the characters before the specified character was last found.
        /// </summary>
        /// <returns>The before last.</returns>
        /// <param name="str">String.</param>
        /// <param name="separator">Separator.</param>
        public static string SubstringBeforeLast(this string str, string separator)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(separator))
            {
                return str;
            }

            var position = str.LastIndexOf(separator);
            if (position == IndexNotFound)
            {
                return str;
            }

            return str.Substring(0, position);
        }

        /// <summary>
        /// Cut out the specified number of characters from the right
        /// </summary>
        /// <param name="str">String.</param>
        /// <param name="length">Length.</param>
        public static string Right(this string str, int length)
        {
            if (str == null)
            {
                return null;
            }

            if (length < 0)
            {
                return Empty;
            }

            if (str.Length <= length)
            {
                return str;
            }

            var result = str.Substring(str.Length - length);
            if (result.StartsWith("0"))
            {
                return RemoveStart(result, "0");
            }

            return result;
        }

        /// <summary>
        /// Removing the beginning of a string by a specified character
        /// </summary>
        /// <returns>The start.</returns>
        /// <param name="str">String.</param>
        /// <param name="remove">Remove.</param>
        public static string RemoveStart(this string str, string remove)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(remove))
            {
                return str;
            }

            if (str.StartsWith(remove))
            {
                return str.Substring(remove.Length);
            }

            return str;
        }

        /// <summary>
        /// Removing line breaks
        /// </summary>
        /// <returns>The new line.</returns>
        /// <param name="str">Self.</param>
        public static string RemoveNewLine(this string str)
        {
            return str.Replace("\r", "").Replace("\n", "");
        }

        /// <summary>
        /// Reverse the target string.
        /// <para>Hello → olleH</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Reverse(this string str)
        {
            return string.Join("", str.Reverse());
        }

        /// <summary>
        /// Cut out the characters after the specified characters are first found
        /// </summary>
        /// <returns>The string after.</returns>
        /// <param name="str">String.</param>
        /// <param name="separator">Separator.</param>
        public static string SubStringAfter(this string str, string separator)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            if (separator == null)
            {
                return Empty;
            }

            var pos = str.IndexOf(separator);
            if (pos == IndexNotFound)
            {
                return Empty;
            }

            return str.Substring(pos + separator.Length);
        }

        /// <summary>
        /// Cut out the characters after the specified character was last found
        /// </summary>
        /// <returns>The string after last.</returns>
        /// <param name="str">String.</param>
        /// <param name="separator">Separator.</param>
        public static string SubStringAfterLast(this string str, string separator)
        {
            if (str.IsNullOrEmpty())
            {
                return str;
            }

            if (separator.IsNullOrEmpty())
            {
                return Empty;
            }

            var pos = str.LastIndexOf(separator);
            if (pos == IndexNotFound || pos == str.Length - separator.Length)
            {
                return Empty;
            }

            return str.Substring(pos + separator.Length);
        }

        /// <summary>
        /// Convert to D2 ToString
        /// </summary>
        /// <returns>The to string.</returns>
        /// <param name="str">String.</param>
        public static string D2ToString(this string str)
        {
            return str.IsNullOrEmpty() ? str : str.ToIntIfNullOrDefault().D2ToString();
        }

        /// <summary>
        /// D2s to string.
        /// </summary>
        /// <returns>The to string.</returns>
        /// <param name="num">Number.</param>
        public static string D2ToString(this int num)
        {
            return num.ToString("D2");
        }

        /// <summary>
        /// Generates and returns a string consisting of only random numbers.
        /// </summary>
        /// <returns>The numeric chars.</returns>
        /// <param name="length">Length.</param>
        public static string GenerateNumericChars(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            Random random = new Random();

            for (var i = 0; i < length; i++)
            {
                var pos = random.Next(NumericChars.Length);
                var c = NumericChars[pos];
                sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates and returns a string consisting only of random lowercase alphanumeric characters.
        /// </summary>
        public static string GenerateLowercaseAlphanumeric(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            Random random = new Random();

            for (var i = 0; i < length; i++)
            {
                var pos = random.Next(PasswordChars.Length);
                var c = PasswordChars[pos];
                sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates and returns a string composed of random alphanumeric characters.
        /// </summary>
        /// <returns>The unique I.</returns>
        /// <param name="length">Length.</param>
        public static string GenerateRandomAlphanumeric(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            Random random = new Random();

            for (var i = 0; i < length; i++)
            {
                var pos = random.Next(UniqueChars.Length);
                var c = UniqueChars[pos];
                sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the Guid generated by the system.
        /// </summary>
        /// <returns>Generates the unique ID</returns>
        public static string GenerateUUID()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Splits the camel case.
        /// </summary>
        /// <returns>The camel case.</returns>
        /// <param name="str">String.</param>
        public static string SplitCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var camelCase =
                Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
            var firstLetter = camelCase.Substring(0, 1).ToUpper();

            if (str.Length > 1)
            {
                var rest = camelCase.Substring(1);
                return firstLetter + rest;
            }

            return firstLetter;
        }

        /// <summary>
        /// Converts the snake case to an upper camel (Pascal) cases.
        /// </summary>
        /// <example> quoted_printable_encode → QuotedPrintableEncode</example>
        public static string ConvertsSnakeToUpperCamel(this string str)
        {
            return string.IsNullOrEmpty(str)
                ? str
                : str.Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                    .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        /// <summary>
        /// Converting snake cases to Lower Camel (Camel) cases.
        /// </summary>
        /// <example>quoted_printable_encode → quotedPrintableEncode</example>
        public static string ConvertsSnakeToLowerCamel(this string str)
        {
            return string.IsNullOrEmpty(str)
                ? str
                : str.ConvertsSnakeToUpperCamel().Insert(0, char.ToLowerInvariant(str[0]).ToString()).Remove(1, 1);
        }

        /// <summary>
        /// Returns a conversion from the specified string to type.
        /// </summary>
        public static Type ToType(this string str)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == str)
                    {
                        return type;
                    }
                }
            }

            throw new ArgumentException($"The string {str} could not be converted to type.");
        }

        /// <summary>
        /// Indicates whether the specified string is an email address.
        /// </summary>
        /// <returns><c>true</c> if is mail address the specified str; otherwise, <c>false</c>.</returns>
        /// <param name="str">String.</param>
        public static bool IsMailAddress(string str)
        {
            return !string.IsNullOrEmpty(str) &&
                   Regex.IsMatch(str, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Indicates whether all the characters in the specified string are hiragana or not.
        /// </summary>
        public static bool IsHiragana(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            foreach (var c in str)
            {
                if (!IsHiragana(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Indicates whether all the characters in the specified string are katakana or not.
        /// </summary>
        public static bool IsKatakana(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            foreach (var c in str)
            {
                if (!IsKatakana(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Indicates whether all the characters in the specified string are full-width katakana or not.
        /// </summary>
        public static bool IsFullWidthKatakana(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            foreach (var c in str)
            {
                if (!IsFullWidthKatakana(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Indicates whether all the characters in the specified string are hiragana and katakana.
        /// </summary>
        /// <returns><c>true</c> if is hiragana and katakana the specified str; otherwise, <c>false</c>.</returns>
        /// <param name="str">String.</param>
        public static bool IsHiraganaAndKatakana(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            foreach (var c in str)
            {
                if (!IsKatakana(c))
                {
                    if (!IsHiragana(c))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Indicates whether the string contains half-width characters.
        /// </summary>
        /// <returns><c>true</c> if is not contain one byte char the specified str; otherwise, <c>false</c>.</returns>
        /// <param name="str">String.</param>
        public static bool ContainsHalfWidth(this string str)
        {
            foreach (var c in str)
            {
                if (!c.IsFullWidth())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Remove all whitespace contained in the string.
        /// </summary>
        /// <returns>The whitespace.</returns>
        /// <param name="str">String.</param>
        public static string DeleteWhitespace(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            var sz = str.Length;
            var chs = new char[sz];
            var count = 0;
            for (var i = 0; i < sz; i++)
            {
                if (!char.IsWhiteSpace(str.ElementAt(i)))
                {
                    chs[count++] = str.ElementAt(i);
                }
            }

            return count == sz ? str : new string(chs, 0, count);
        }

        /// <summary>
        /// Indicates whether the characters are full-width characters or not.
        /// </summary>
        /// <returns><c>true</c> if is char2 byte the specified c; otherwise, <c>false</c>.</returns>
        /// <param name="c">C.</param>
        private static bool IsFullWidth(this char c)
        {
            return !(c >= 0x0 && c < 0x81 || c == 0xf8f0 || c >= 0xff61 && c < 0xffa0 || c >= 0xf8f1 && c < 0xf8f4);
        }

        /// <summary>
        /// Indicates whether the length of the specified string is within the specified number of characters.
        /// </summary>
        /// <returns><c>true</c> if is range by length the specified str min max; otherwise, <c>false</c>.</returns>
        /// <param name="str">String.</param>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public static bool IsWithinByStringLength(this string str, int min, int max)
        {
            if (str.IsNullOrEmpty())
            {
                return false;
            }

            return str.Length >= min && str.Length <= max;
        }

        /// <summary>
        /// Indicates whether the length of the specified string is within the specified number of characters.
        /// </summary>
        /// <returns><c>true</c> if is range by length the specified str min max; otherwise, <c>false</c>.</returns>
        /// <param name="str">String.</param>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public static bool IsWithinByStringLengthWithoutEmpty(this string str, int min, int max)
        {
            return str.Trim().IsWithinByStringLength(min, max);
        }

        /// <summary>
        /// Counting certain characters in a string.
        /// </summary>
        /// <returns>string counts</returns>
        /// <param name="s">str</param>
        /// <param name="c">char</param>
        public static int CountCharacters(this string str, char c)
        {
            return str.Length - str.Replace(c.ToString(), "").Length;
        }

        /// <summary>
        /// Determines if the specified Unicode character is hiragana or not.
        /// </summary>
        /// <param name="c">Unicode characters to evaluate.</param>
        /// <returns>True if char is hiragana, false otherwise.</returns>
        public static bool IsHiragana(this char c)
        {
            return ('\u3041' <= c && c <= '\u309F') || c == '\u30FC' || c == '\u30A0';
        }

        /// <summary>
        /// Determines if the specified Unicode character is katakana or not.
        /// </summary>
        /// <param name="c">Unicode characters to evaluate.</param>
        /// <returns>True if char is in katakana, false otherwise.</returns>
        public static bool IsKatakana(this char c)
        {
            return '\u30A0' <= c && c <= '\u30FF' || '\u31F0' <= c && c <= '\u31FF' || '\u3099' <= c && c <= '\u309C' ||
                   '\uFF65' <= c && c <= '\uFF9F';
        }

        /// <summary>
        /// Determines if the specified Unicode character is a full-width katakana or not.
        /// </summary>
        /// <param name="c">Unicode characters to evaluate.</param>
        /// <returns>True if char is in full-width katakana; false otherwise.</returns>
        public static bool IsFullWidthKatakana(this char c)
        {
            return '\u30A0' <= c && c <= '\u30FF' || '\u31F0' <= c && c <= '\u31FF' || '\u3099' <= c && c <= '\u309C';
        }

        /// <summary>
        /// Convert from string to bool. Returns the default value if it is empty or null.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string str)
        {
            try
            {
                return Convert.ToBoolean(str);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Convert from string to byte. Returns the default value if it is empty or null.
        /// </summary>
        public static byte ToByteIfNullOrDefault(this string str, byte defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            try
            {
                return byte.Parse(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Convert from string to int. Returns the default value if it is empty or null.
        /// </summary>
        public static int ToIntIfNullOrDefault(this string str, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            try
            {
                return int.Parse(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Convert from string to float. Returns the default value if it is empty or null.
        /// </summary>
        public static float ToFloatIfNullOrDefault(this string str, float defaultValue = 0.0f)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            try
            {
                return float.Parse(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Convert from string to Double. Returns the default value if it is empty or null.
        /// </summary>
        public static double ToDoubleIfNullOrDefault(this string str, double defaultValue = 0.0d)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            try
            {
                return double.Parse(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Convert from string to long. Returns the default value if it is empty or null.
        /// </summary>
        public static long ToLongIfNullOrDefault(this string str, long defaultValue = 0L)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            try
            {
                return long.Parse(str);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Concatenate all elements of an array of strings. The specified delimiter will be inserted between each element.
        /// </summary>
        public static string Join(string separator, IEnumerable<string> value)
        {
            return string.Join(separator, value.ToArray());
        }

        /// <summary>
        /// Concatenate the specified elements of an array of strings. The specified delimiter will be inserted between each element.
        /// </summary>
        public static string Join(string separator, IEnumerable<string> value, int startIndex, int count)
        {
            return string.Join(separator, value.ToArray(), startIndex, count);
        }
    }
}