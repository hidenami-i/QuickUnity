using System.Text;
using QuickUnity.Editor.Enumerations;

namespace QuickUnity.Editor.Utility
{
    public static class DocumentUtility
    {
        private static readonly IndentType IndentType = IndentType.Whitespace4;

        public static void setup(IndentType indentType)
        {
            indentType = IndentType;
        }

        public static void SetUsing(this StringBuilder builder, params string[] strs)
        {
            foreach (string str in strs)
            {
                builder.AppendLine($"using {str};");
            }
        }

        public static void SetNameSpace(this StringBuilder builder, string nameSpace)
        {
            builder.Indent0().AppendLine($"namespace {nameSpace}");
        }

        public static void SetSummaryComment(this StringBuilder builder, string comment, int indent)
        {
            builder.Indent(indent).AppendLine("/// <summary>");
            var lines = comment.Split(new[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                builder.Indent(indent).AppendLine($"/// {line}");
            }

            builder.Indent(indent).Append("/// </summary>");
        }

        private static string Indent(int indentLevel)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < indentLevel; ++i)
            {
                builder.Append(IndentTypeExtensions.ToString(IndentType));
            }

            return builder.ToString();
        }

        public static StringBuilder Indent(this StringBuilder builder, int indentLevel)
        {
            return builder.Append(Indent(indentLevel));
        }

        public static StringBuilder Indent0(this StringBuilder builder)
        {
            return builder.Append(Indent(0));
        }

        public static StringBuilder Indent1(this StringBuilder builder)
        {
            return builder.Append(Indent(1));
        }

        public static StringBuilder Indent2(this StringBuilder builder)
        {
            return builder.Append(Indent(2));
        }

        public static StringBuilder Indent3(this StringBuilder builder)
        {
            return builder.Append(Indent(3));
        }

        public static StringBuilder Indent4(this StringBuilder builder)
        {
            return builder.Append(Indent(4));
        }

        public static StringBuilder Indent5(this StringBuilder builder)
        {
            return builder.Append(Indent(5));
        }

        public static StringBuilder Indent6(this StringBuilder builder)
        {
            return builder.Append(Indent(6));
        }

        public static StringBuilder Indent7(this StringBuilder builder)
        {
            return builder.Append(Indent(7));
        }

        public static StringBuilder Indent8(this StringBuilder builder)
        {
            return builder.Append(Indent(8));
        }
    }
}