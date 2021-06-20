using System;

namespace QuickUnity.Extensions.DotNet
{
    public static class ExNumber
    {
        /// <summary>
        /// <para>Converts a decimal number to a hexadecimal string.</para>
        /// <para>1234.ConvertsDecimalToHex() // 0004D2</para>
        /// </summary>
        public static string ToHex(this int self)
        {
            self &= 0xFFFFFF;
            return self.ToString("X6");
        }

        /// <summary>
        /// Indicates whether the number is even or not.
        /// </summary>
        public static bool IsEven(this int self) => self % 2 == 0;

        /// <summary>
        /// Indicates whether it is an odd number.
        /// </summary>
        public static bool IsOdd(this int self) => self % 2 == 1;

        /// <summary>
        /// Indicates whether it is an natural number.
        /// </summary>
        public static bool IsPositiveNumber(this int self) => self > 0;

        /// <summary>
        /// Indicates whether it is an negative number.
        /// </summary>
        public static bool IsNegative(this int self) => self < 0;

        /// <summary>
        /// Indicates whether it is an natural number.
        /// </summary>
        public static bool IsZero(this int self) => self == 0;

        /// <summary>
        /// Convert the numbers to alphabet.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToAlphabet(this int self)
        {
            if (self < 1 || self > 26)
            {
                throw new ArgumentException("Must put a number between 1 and 26.");
            }

            var n = self % 26;
            n = n == 0 ? 26 : n;
            var s = ((char) (n + 64)).ToString();
            if (self == n)
                return s;
            return ((self - n) / 26).ToAlphabet() + s;
        }

        /// <summary>
        /// Get the number of digits.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int DigitsSize(this int self) => self == 0 ? 1 : (int) UnityEngine.Mathf.Log10(self) + 1;

        /// <summary>
        /// Converts the passed numbers into a three-digit comma-separated string and returns
        /// </summary>
        public static string WithComma(this int self) => self.ToString("N0");

        /// <summary>
        /// Converts the passed numbers into a three-digit comma-separated string and returns
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string WithComma(this float self) => self.ToString("N0");

        /// <summary>
        /// Converts the passed numbers into a three-digit comma-separated string and returns
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string WithComma(this long self) => self.ToString("N0");

        /// <summary>
        /// Convert bytes of type long to the appropriate units.
        /// </summary>
        /// <returns>The byte to string.</returns>
        /// <param name="size">bytes</param>
        public static string ToHumanReadableSize(long size)
        {
            double bytes = size;

            if (bytes <= 1024) return bytes.ToString("f2") + " B";

            bytes /= 1024;
            if (bytes <= 1024) return bytes.ToString("f2") + " KB";

            bytes /= 1024;
            if (bytes <= 1024) return bytes.ToString("f2") + " MB";

            bytes /= 1024;
            if (bytes <= 1024) return bytes.ToString("f2") + " GB";

            bytes /= 1024;
            if (bytes <= 1024) return bytes.ToString("f2") + " TB";

            bytes /= 1024;
            if (bytes <= 1024) return bytes.ToString("f2") + " PB";

            bytes /= 1024;
            if (bytes <= 1024) return bytes.ToString("f2") + " EB";

            bytes /= 1024;
            return bytes + " ZB";
        }
    }
}