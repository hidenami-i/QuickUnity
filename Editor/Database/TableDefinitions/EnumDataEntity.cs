using System.Text;
using QuickUnity.Database;
using QuickUnity.Editor.Utility;
using QuickUnity.Extensions.DotNet;

namespace QuickUnity.Editor.Database.TableDefinition
{
    public class EnumDataEntity : EntityBase
    {
        private readonly int index;
        private readonly string fieldLogicalName;
        private readonly string fieldPhysicalName;
        private readonly int value;
        private readonly string description;

        public EnumDataEntity(int index, string fieldLogicalName, string fieldPhysicalName, int value,
            string description)
        {
            this.index = index;
            this.fieldLogicalName = fieldLogicalName;
            this.fieldPhysicalName = fieldPhysicalName;
            this.value = value;
            this.description = description;
        }

        public string GenerateEnumValueScript()
        {
            StringBuilder builder = new StringBuilder();
            builder.SetSummaryComment($"{fieldLogicalName}", 2);
            builder.AppendLine();
            builder.Indent2().AppendLine($"{fieldPhysicalName.ConvertsSnakeToUpperCamel()} = {value},");
            return builder.ToString();
        }

        public string GenerateLogicalNameScript(string enumPhysicalName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Indent4()
                .Append(
                    $"{enumPhysicalName.ConvertsSnakeToUpperCamel()}.{fieldPhysicalName.ConvertsSnakeToUpperCamel()} => \"{fieldLogicalName}\",");
            return builder.ToString();
        }

        public string GenerateBooleanScript(string enumPhysicalName)
        {
            StringBuilder builder = new StringBuilder();
            builder.SetSummaryComment(
                $"Returns true if the {enumPhysicalName} is {fieldPhysicalName.ConvertsSnakeToUpperCamel()}.",
                2);
            builder.AppendLine();
            builder.Indent2()
                .Append(
                    $"public static bool Is{fieldPhysicalName.ConvertsSnakeToUpperCamel()}(this {enumPhysicalName.ConvertsSnakeToUpperCamel()} {enumPhysicalName.ConvertsSnakeToLowerCamel()})")
                .AppendLine(" {");
            builder.Indent3()
                .AppendLine(
                    $"return {enumPhysicalName.ConvertsSnakeToLowerCamel()} == {enumPhysicalName.ConvertsSnakeToUpperCamel()}.{fieldPhysicalName.ConvertsSnakeToUpperCamel()};");
            builder.Indent2().AppendLine("}");
            return builder.ToString();
        }
    }
}
