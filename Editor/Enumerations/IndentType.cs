namespace QuickUnity.Editor.Enumerations
{
    public enum IndentType
    {
        Whitespace2,
        Whitespace4,
        Tab
    }

    public static class IndentTypeExtensions
    {
        public static string ToString(this IndentType indentType)
        {
            switch (indentType)
            {
                case IndentType.Whitespace2:
                    return "  ";
                case IndentType.Whitespace4:
                    return "    ";
                case IndentType.Tab:
                    return "\t";
                default:
                    return "";
            }
        }
    }
}