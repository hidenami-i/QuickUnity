using UnityEngine;

namespace QuickUnity.Extensions.Unity
{
    public static class ExMathf
    {
        /// <summary>
        /// Radian to Degree.
        /// </summary>
        /// <returns>The angle.</returns>
        /// <param name="radian">Radian.</param>
        public static float ToAngle(this float radian)
        {
            return radian * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Degree to Radian.
        /// </summary>
        /// <returns>The radian.</returns>
        /// <param name="angle">Angle.</param>
        public static float ToRadian(this float angle)
        {
            return angle * Mathf.Deg2Rad;
        }

        /// <summary>
        /// The length of the circumference
        /// 2 * 3.14 * r * degree / 360f
        /// </summary>
        /// <returns>The of circumference.</returns>
        /// <param name="halfLength"></param>
        /// <param name="degree"></param>
        public static float CircumferenceLength(float halfLength, float degree)
        {
            return Mathf.PI * halfLength * degree / 180.0f;
        }

        /// <summary>
        /// Find the decibel from the amplitude.
        /// Max is 20 dB, but it rounds at 0 dB so that it does not exceed the original volume of the audio file.
        /// </summary>
        /// <returns>The decibel.</returns>
        /// <param name="amplitude">amplitude</param>
        public static float ToDecibel(this float amplitude)
        {
            float decibel = 20f * Mathf.Log10(amplitude);
            const float minDecibel = -80;
            const float maxDecibel = 0;
            return Mathf.Clamp(decibel, minDecibel, maxDecibel);
        }

        /// <summary>
        /// Return an angle for every n degrees smaller (such an angle to be truncated in multiples)
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        /// <example>
        /// AngleFloor(185, 90) → 180
        /// AngleFloor(angle, 45) ： 54, 192, 325 → 45, 180, 315
        /// AngleFloor(angle, 90) ： 54, 192, 325 → 0, 180, 270
        /// </example>
        public static float AngleFloor(float angle, float width)
        {
            return Mathf.Repeat(Mathf.Floor(angle / width) * width, 360f);
        }

        /// <summary>
        /// Return an angle for every n degrees greater (such as angles rolled up in multiples)
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        /// <example>
        /// AngleCeil(185, 90) → 270
        /// AngleCeil(angle, 45) ： 54, 192, 325 → 90, 225, 0
        /// AngleCeil(angle, 90) ： 54, 192, 325 → 90, 270, 0
        /// </example>
        public static float AngleCeil(float angle, float width)
        {
            return Mathf.Repeat(Mathf.Ceil(angle / width) * width, 360f);
        }

        /// <summary>
        /// Return angles like rounding by n degrees (angles like rounding by n/2 degrees)
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        /// <example>
        /// AngleRound(134, 90) → 90, AngleRound(135, 90) → 180
        /// AngleRound(angle, 45) ： 54, 192, 325 → 45, 180, 315
        /// AngleRound(angle, 90) ： 54, 192, 325 → 90, 180, 0
        /// </example>
        public static float AngleRound(float angle, float width)
        {
            return Mathf.Repeat(AngleFloor(angle + width * 0.5f, width), 360f);
        }
    }
}
