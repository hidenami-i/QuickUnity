using System.Text;

namespace QuickUnity.Extensions.DotNet
{
    public static class ExStringBuilder
    {
        private static readonly StringBuilder builder = new StringBuilder(1024);

        public static string GCSafeString(params string[] appends)
        {
            builder.Length = 0;
            var length = appends.Length;
            for (var i = 0; i < length; i++)
            {
                builder.Append(appends[i]);
            }

            return builder.ToString();
        }
    }
}
