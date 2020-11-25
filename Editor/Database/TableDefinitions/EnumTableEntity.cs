using System;
using System.Collections.Generic;
using System.Text;
using QuickUnity.Editor.Utility;
using QuickUnity.Extensions.DotNet;
using QuickUnity.Runtime.Database;
using UnityEngine;

namespace QuickUnity.Editor.Database.TableDefinition
{
    [Serializable]
    public class EnumTableEntity : EntityBase
    {
        [SerializeField] private string schema;
        [SerializeField] private string enumLogicalName;
        [SerializeField] private string enumPhysicalName;
        [SerializeField] private List<EnumDataEntity> enumDataEntityList;

        public string EnumPhysicalName => enumPhysicalName;

        public string ScriptFileName => enumPhysicalName.ConvertsSnakeToUpperCamel() + ".cs";

        public EnumTableEntity()
        {
        }

        public EnumTableEntity(string schema, string enumLogicalName, string enumPhysicalName,
            List<EnumDataEntity> enumDataEntityList)
        {
            this.schema = schema;
            this.enumLogicalName = enumLogicalName;
            this.enumPhysicalName = enumPhysicalName;
            this.enumDataEntityList = enumDataEntityList;
        }

        public string GenerateEnumScript()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.SetUsing("System.Collections.Generic", "System");
            stringBuilder.AppendLine();
            stringBuilder.SetNameSpace("Database.Enumerations");
            stringBuilder.Indent0().AppendLine("{");
            stringBuilder.SetSummaryComment(
                $"This enum is generated automatically, so it can not be edited.\nTable logical name is {enumLogicalName}",
                1);

            stringBuilder.AppendLine();
            stringBuilder.Indent1().AppendLine($"public enum {enumPhysicalName.ConvertsSnakeToUpperCamel()}");
            stringBuilder.Indent1().AppendLine("{");

            foreach (EnumDataEntity entity in enumDataEntityList)
            {
                stringBuilder.AppendLine(entity.GenerateEnumValueScript());
            }

            stringBuilder.Indent1().AppendLine("}");
            stringBuilder.AppendLine();

            stringBuilder.Indent1()
                .AppendLine(
                    $"public class {enumPhysicalName.ConvertsSnakeToUpperCamel()}Compare : IEqualityComparer<{enumPhysicalName.ConvertsSnakeToUpperCamel()}>");
            stringBuilder.Indent1().AppendLine("{");
            stringBuilder.Indent2()
                .AppendLine(
                    $"public bool Equals({enumPhysicalName.ConvertsSnakeToUpperCamel()} x, {enumPhysicalName.ConvertsSnakeToUpperCamel()} y) " +
                    "{");
            stringBuilder.Indent3().AppendLine("return x == y;");
            stringBuilder.Indent2().AppendLine("}");
            stringBuilder.AppendLine();
            stringBuilder.Indent2()
                .AppendLine($"public int GetHashCode({enumPhysicalName.ConvertsSnakeToUpperCamel()} obj) " + "{");
            stringBuilder.Indent3().AppendLine("return (int)obj;");
            stringBuilder.Indent2().AppendLine("}");
            stringBuilder.Indent1().AppendLine("}");
            stringBuilder.AppendLine();

            stringBuilder.Indent1()
                .AppendLine($"public static partial class {enumPhysicalName.ConvertsSnakeToUpperCamel()}Extensions");
            stringBuilder.Indent1().AppendLine("{");

            stringBuilder.Indent2()
                .AppendLine(
                    $"public static string ToLogicalName(this {enumPhysicalName.ConvertsSnakeToUpperCamel()} {enumPhysicalName.ConvertsSnakeToLowerCamel()})");

            stringBuilder.Indent2().AppendLine("{");
            stringBuilder.Indent3().AppendLine($"return {enumPhysicalName.ConvertsSnakeToLowerCamel()} switch");
            stringBuilder.Indent3().AppendLine("{");

            foreach (EnumDataEntity entity in enumDataEntityList)
            {
                stringBuilder.AppendLine(entity.GenerateLogicalNameScript(enumPhysicalName));
            }

            stringBuilder.Indent4()
                .AppendLine(
                    $"_ => throw new ArgumentNullException($\"It is an undefined {enumPhysicalName.ConvertsSnakeToUpperCamel()}.\")");
            stringBuilder.Indent3().AppendLine("};");
            stringBuilder.Indent2().AppendLine("}");
            stringBuilder.AppendLine();

            foreach (EnumDataEntity entity in enumDataEntityList)
            {
                stringBuilder.AppendLine(entity.GenerateBooleanScript(enumPhysicalName));
            }

            stringBuilder.Indent1().AppendLine("}");
            stringBuilder.Indent0().AppendLine("}");

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return
                $"{nameof(schema)}: {schema}, {nameof(enumLogicalName)}: {enumLogicalName}, {nameof(enumPhysicalName)}: {enumPhysicalName}, {nameof(enumDataEntityList)}: {enumDataEntityList}";
        }
    }
}
