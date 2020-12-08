using System;

namespace QuickUnity.Editor.Database.Enumerations
{
    public enum CSharpDataType
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,

        /// <summary>
        /// -2147483648 to 2147483647
        /// </summary>
        Int,

        /// <summary>
        /// -9223372036854775808 to 9223372036854775807
        /// </summary>
        Long,

        /// <summary>
        /// -9,223,372,036,854,775,808～9,223,372,036,854,775,807
        /// </summary>
        Float,

        /// <summary>
        /// -3.402823466E+38～-1.175494351E-38
        /// </summary>
        Double,

        /// <summary>
        /// true of false. 1 or 0.
        /// </summary>
        Bool,

        /// <summary>
        /// string
        /// </summary>
        String,

        /// <summary>
        /// 1970-01-01 00:00:01' UTC ～ '2038-01-19 03:14:07' UTC
        /// </summary>
        DateTime
    }

    public static class DataTypeExtensions
    {
        public static bool IsInt(this CSharpDataType cSharpDataType) => cSharpDataType == CSharpDataType.Int;
        public static bool IsLong(this CSharpDataType cSharpDataType) => cSharpDataType == CSharpDataType.Long;
        public static bool IsFloat(this CSharpDataType cSharpDataType) => cSharpDataType == CSharpDataType.Float;
        public static bool IsDouble(this CSharpDataType cSharpDataType) => cSharpDataType == CSharpDataType.Double;
        public static bool IsBool(this CSharpDataType cSharpDataType) => cSharpDataType == CSharpDataType.Bool;
        public static bool IsString(this CSharpDataType cSharpDataType) => cSharpDataType == CSharpDataType.String;
        public static bool IsDateTime(this CSharpDataType cSharpDataType) => cSharpDataType == CSharpDataType.DateTime;

        public static string ToCsharpName(this CSharpDataType cSharpDataType)
        {
            return cSharpDataType.IsDateTime() ? nameof(DateTime) : cSharpDataType.ToString().ToLower();
        }
    }
}
