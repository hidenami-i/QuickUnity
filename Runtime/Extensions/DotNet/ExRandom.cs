namespace QuickUnity.Extensions.DotNet
{
    public static class ExRandom
    {
        /// <summary>
        /// Randomly returns a floating point number from 0.0 to 1.0.
        /// </summary>
        public static float Value => UnityEngine.Random.value;

        /// <summary>
        /// Randomly return true or false boolean values.
        /// </summary>
        public static bool BoolValue => UnityEngine.Random.Range(0, 2) == 0;

        /// <summary>
        /// Randomly returns an integer between 1 and max - 1.
        /// </summary>
        public static int PositiveRange(int max) => UnityEngine.Random.Range(1, max);

        /// <summary>
        /// Randomly returns an integer between 1 and max.
        /// </summary>
        public static float PositiveRange(float max) => UnityEngine.Random.Range(1f, max);

        /// <summary>
        /// Randomly returns an integer between min and max - 1.
        /// Use UnityEngine's Random instead of the Random in the System.
        /// </summary>
        public static int Range(int min, int max) => UnityEngine.Random.Range(min, max);

        /// <summary>
        /// Randomly returns a floating point number between min and max.
        /// Use UnityEngine's Random instead of the Random in the System.
        /// </summary>
        public static float Range(float min, float max) => UnityEngine.Random.Range(min, max);
    }
}