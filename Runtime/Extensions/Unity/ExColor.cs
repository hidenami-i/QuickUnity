using System;
using QuickUnity.Extensions.DotNet;
using UnityEngine;

namespace QuickUnity.Extensions.Unity
{
    public static class ExColor
    {
        #region Constants

        const float LightOffset = 0.0625f;
        const float DarkerFactor = 0.9f;

        #endregion

        /// <summary>
        /// <para>Convert Color to a string of hexadecimal digits.</para>
        /// <para>Color.red.EncodeColor() // FF0000</para>
        /// </summary>
        public static string ConvertsDecimalToHex(this Color self)
        {
            int i = 0xFFFFFF & (self.ToInt() >> 8);
            return i.ToHex();
        }

        /// <summary>
        /// Convert colors to hexadecimal numbers.
        /// </summary>
        public static int ToInt(this Color self)
        {
            int result = 0;
            result |= Mathf.RoundToInt(self.r * 255f) << 24;
            result |= Mathf.RoundToInt(self.g * 255f) << 16;
            result |= Mathf.RoundToInt(self.b * 255f) << 8;
            result |= Mathf.RoundToInt(self.a * 255f);
            return result;
        }

        /// <summary>
        /// Convert from RGB values to the Color class.
        /// </summary>
        /// <returns>The RG.</returns>
        /// <param name="red">Red.</param>
        /// <param name="green">Green.</param>
        /// <param name="blue">Blue.</param>
        public static Color RGBToColor(int red, int green, int blue)
        {
            return new Color(red / 255f, green / 255f, blue / 255f);
        }

        /// <summary>
        /// Converts hexadecimal values starting with "#" to the Color class.
        /// </summary>
        /// <returns>The hex to RG.</returns>
        /// <param name="hex">Hex.</param>
        public static Color ConvertsHexToColor(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                throw new ArgumentNullException($"The target character {hex} is empty or null.");
            }

            hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
            byte a = 255; //assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// Returns a color brighter than the target color.
        /// </summary>
        /// <param name="color">Color.</param>
        public static Color Lighter(this Color color)
        {
            return new Color(color.r + LightOffset, color.g + LightOffset, color.a + LightOffset, color.a);
        }

        /// <summary>
        /// Returns a color darker than the target color
        /// </summary>
        /// <param name="color">Color.</param>
        public static Color Darker(this Color color)
        {
            return new Color(color.r - DarkerFactor, color.g - DarkerFactor, color.a - DarkerFactor, color.a);
        }

        /// <summary>
        /// Returns the brightness of the target color.
        /// </summary>
        /// <param name="color">Color.</param>
        public static float Brightness(this Color color)
        {
            return (color.r + color.g + color.b) / 3;
        }

        /// <summary>
        /// Returns specifying the brightness of the target color.
        /// </summary>
        /// <returns>The brightness.</returns>
        /// <param name="color">Color.</param>
        /// <param name="brightness">Brightness.</param>
        public static Color WithBrightness(this Color color, float brightness)
        {
            float bright = Mathf.Clamp01(brightness);

            if (color.IsApproximatelyBlack())
            {
                return new Color(bright, bright, bright, color.a);
            }

            float factor = bright / color.Brightness();

            // Multiply the target color
            float r = color.r * factor;
            float g = color.g * factor;
            float b = color.b * factor;
            float a = color.a;

            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Determine what is not infinitely black.
        /// </summary>
        /// <returns><c>true</c> if is approximately black the specified color; otherwise, <c>false</c>.</returns>
        /// <param name="color">Color.</param>
        public static bool IsApproximatelyBlack(this Color color)
        {
            return color.r + color.g + color.b <= Mathf.Epsilon;
        }

        /// <summary>
        /// Determine if it is not infinitely white.
        /// </summary>
        /// <returns><c>true</c> if is approximately white the specified color; otherwise, <c>false</c>.</returns>
        /// <param name="color">Color.</param>
        public static bool IsApproximatelyWhite(this Color color)
        {
            return color.r + color.g + color.b >= 1 - Mathf.Epsilon;
        }

        /// <summary>
        /// Returns the target color opaque.
        /// </summary>
        /// <param name="color">Color.</param>
        public static Color Opaque(this Color color)
        {
            return new Color(color.r, color.g, color.b);
        }

        /// <summary>
        /// Returns a specified transparent value for the target color.
        /// </summary>
        /// <returns>The alpha.</returns>
        /// <param name="color">Color.</param>
        /// <param name="alpha">Alpha.</param>
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha));
        }
    }
}