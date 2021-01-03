using QuickUnity.Editor.Utility;
using QuickUnity.Extensions.DotNet;
using QuickUnity.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickUnity.Editor.Database.Enumerations;
using UnityEngine;

namespace QuickUnity.Editor.Database.TableDefinition
{
    [Serializable]
    public class TableDefinitionEntity : EntityBase
    {
        [SerializeField] private string tableName = "";
        [SerializeField] private string schema = "";
        [SerializeField] private string logicalName = "";
        [SerializeField] private string physicalName = "";
        [SerializeField] private string persistentObjectType = "";
        [SerializeField] private List<TableDefinitionDataEntity> data = null;

        public TableDefinitionEntity() { }

        public TableDefinitionEntity(string tableName, string schema, string logicalName, string physicalName,
            string persistentObjectType, List<TableDefinitionDataEntity> data)
        {
            this.tableName = tableName;
            this.schema = schema;
            this.logicalName = logicalName;
            this.physicalName = physicalName;
            this.persistentObjectType = persistentObjectType;
            this.data = data;
        }

        public string TableName => tableName;
        public SchemaType SchemaType => ExEnum.FromString<SchemaType>(schema);
        public string LogicalName => logicalName;
        public string PhysicalName => physicalName;

        public PersistentObjectType PersistentObjectType =>
            ExEnum.FromString<PersistentObjectType>(persistentObjectType);

        public List<TableDefinitionDataEntity> Data => data;

        string GetTableLogicalNameScript
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Indent1().AppendLine("/// <summary>");
                stringBuilder.Indent1().AppendLine("/// This class is auto-generated do not modify.");
                stringBuilder.Indent1().AppendLine($"/// Table logical name is [{logicalName}]");
                stringBuilder.Indent1().Append("/// </summary>");
                return stringBuilder.ToString();
            }
        }

        private string ClassName => PhysicalName.ConvertsSnakeToUpperCamel();
        public string EntityScriptFileName => ClassName + "Entity.cs";
        public string EntityServiceScriptFileName => ClassName + "EntityService.cs";
        public string RepositoryScriptFileName => ClassName + "Repository.cs";
        public string RepositoryServiceScriptFileName => ClassName + "RepositoryService.cs";
        public string DataMapperScriptFileName => ClassName + "DataMapper.cs";
        public string DataMapperServiceScriptFileName => ClassName + "DataMapperService.cs";

        /// <summary>
        /// Generate Entity Classes.
        /// </summary>
        /// <returns>Entity Script.</returns>
        public string GenerateEntityScript(string nameSpace)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Header
            stringBuilder.SetUsing("System", "System.Text", "QuickUnity.Database", "UnityEngine");
            stringBuilder.AppendLine();
            stringBuilder.SetNameSpace(nameSpace);

            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(GetTableLogicalNameScript);
            stringBuilder.Indent1().AppendLine("[Serializable]");
            stringBuilder.Indent1().AppendLine($"public sealed partial class {ClassName}Entity : EntityBase");
            stringBuilder.Indent1().AppendLine("{");

            // Field list
            foreach (TableDefinitionDataEntity dataStructureEntity in data)
            {
                stringBuilder.Indent2().AppendLine(dataStructureEntity.GenerateSerializeFieldScript);
            }

            stringBuilder.AppendLine();

            // Getter list
            foreach (TableDefinitionDataEntity dataStructureEntity in data)
            {
                stringBuilder.AppendLine(dataStructureEntity.GeneratePhysicalNameGetterScript);
            }

            // Setter list
            foreach (TableDefinitionDataEntity dataStructureEntity in data)
            {
                stringBuilder.AppendLine(dataStructureEntity.GenerateSetterScript);
            }

            // default constructor
            stringBuilder.Indent2().Append($"public {ClassName}Entity()").AppendLine(" { }");
            stringBuilder.AppendLine();
            stringBuilder.Indent2().AppendLine($"public {ClassName}Entity(");

            // Constructor argument list
            string argument = string.Join(",\n", data.Select(x => "\t\t\t" + x.GenerateConstructorArgumentScript));
            stringBuilder.AppendLine(argument);

            stringBuilder.Indent2().AppendLine(")");
            stringBuilder.Indent2().AppendLine("{");

            // Constructor initialize argument list
            foreach (TableDefinitionDataEntity dataStructureEntity in data)
            {
                stringBuilder.Indent3().AppendLine(dataStructureEntity.GenerateConstructorInitializeArgumentScript);
            }

            stringBuilder.Indent2().AppendLine("}");

            stringBuilder.AppendLine();

            stringBuilder.Indent2().AppendLine("public override string ToString()");
            stringBuilder.Indent2().AppendLine("{");
            stringBuilder.Indent3().AppendLine("StringBuilder builder = new StringBuilder();");
            stringBuilder.Indent3().Append("builder.AppendLine().AppendLine($\"<b>ClassName [{nameof(");
            stringBuilder.Append($"{ClassName}" + "Entity");
            stringBuilder.Append(")}]</b>\");");
            stringBuilder.AppendLine();

            // log field list
            foreach (TableDefinitionDataEntity dataStructureEntity in data)
            {
                stringBuilder.Indent3().AppendLine(dataStructureEntity.GenerateLogFieldScript);
            }

            stringBuilder.Indent3().AppendLine("return builder.ToString();");

            stringBuilder.Indent2().AppendLine("}");
            stringBuilder.Indent1().AppendLine("}");
            stringBuilder.Indent0().AppendLine("}");

            return stringBuilder.ToString();
        }

        public string GenerateEntityServiceScript(string nameSpace)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Header
            stringBuilder.SetNameSpace(nameSpace);

            stringBuilder.AppendLine("{");
            stringBuilder.Indent1().AppendLine($"public sealed partial class {ClassName}Entity");
            stringBuilder.Indent1().AppendLine("{");
            stringBuilder.Indent1().AppendLine("}");
            stringBuilder.Indent0().AppendLine("}");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Generate Repository Classes.
        /// </summary>
        /// <returns>Repository Script.</returns>
        public string GenerateRepositoryScript(string nameSpace)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Header
            stringBuilder.SetUsing("System", "System.Collections.Generic", "QuickUnity.Database", "UnityEngine");
            stringBuilder.AppendLine();
            stringBuilder.SetNameSpace(nameSpace);

            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(GetTableLogicalNameScript);
            stringBuilder.Indent1().AppendLine("[Serializable]");
            stringBuilder.Indent1()
                .AppendLine(
                    $"public sealed partial class {ClassName}Repository : RepositoryBase<{ClassName}Entity, {ClassName}Repository>, IDatabase");
            stringBuilder.Indent1().AppendLine("{");

            stringBuilder.SetSummaryComment("This field is generated automatically, so it can not be edited.", 2);
            stringBuilder.AppendLine();
            stringBuilder.Indent2()
                .AppendLine(
                    $"[SerializeField] private List<{ClassName}Entity> {ClassName.ConvertsSnakeToLowerCamel()} = new List<{ClassName}Entity>();");

            stringBuilder.AppendLine();

            stringBuilder.SetSummaryComment("This getter is generated automatically, so it can not be edited.", 2);
            stringBuilder.AppendLine();
            stringBuilder.Indent2()
                .AppendLine(
                    $"protected override List<{ClassName}Entity> EntityList => {ClassName.ConvertsSnakeToLowerCamel()};");

            stringBuilder.AppendLine();

            stringBuilder.SetSummaryComment("This getter is generated automatically, so it can not be edited.", 2);
            stringBuilder.AppendLine();
            stringBuilder.Indent2().AppendLine($"public string Schema => \"{schema}\";");

            stringBuilder.SetSummaryComment("This getter is generated automatically, so it can not be edited.", 2);
            stringBuilder.AppendLine();
            stringBuilder.Indent2().AppendLine($"public override string TableName => \"{tableName}\";");

            stringBuilder.SetSummaryComment("This getter is generated automatically, so it can not be edited.", 2);
            stringBuilder.AppendLine();
            stringBuilder.Indent2().AppendLine($"public override string PhysicalName => \"{physicalName}\";");

            stringBuilder.AppendLine();

            // find function
            foreach (TableDefinitionDataEntity tableDefinitionEntity in data)
            {
                stringBuilder.AppendLine(tableDefinitionEntity.GenerateFindFunctionScript(ClassName));
            }

            // find if null or default function
            foreach (TableDefinitionDataEntity tableDefinitionEntity in data)
            {
                stringBuilder.AppendLine(tableDefinitionEntity.GenerateGetOrDefaultFunctionScript(ClassName));
            }

            // find all function
            foreach (TableDefinitionDataEntity tableDefinitionEntity in data)
            {
                stringBuilder.AppendLine(tableDefinitionEntity.GenerateFindAllFunctionScript(ClassName));
            }

            stringBuilder.Indent1().AppendLine("}");
            stringBuilder.Indent0().AppendLine("}");

            return stringBuilder.ToString();
        }

        public string GenerateRepositoryServiceScript(string nameSpace)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Header
            stringBuilder.SetNameSpace(nameSpace);

            stringBuilder.AppendLine("{");
            stringBuilder.Indent1().AppendLine($"public sealed partial class {ClassName}Repository");
            stringBuilder.Indent1().AppendLine("{");
            stringBuilder.Indent1().AppendLine("}");
            stringBuilder.Indent0().AppendLine("}");

            return stringBuilder.ToString();
        }

        public string GenerateDataMapperScript(string nameSpace)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Header
            stringBuilder.SetUsing("System", "QuickUnity.Database", "UnityEngine");
            stringBuilder.AppendLine();
            stringBuilder.SetNameSpace(nameSpace);

            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(GetTableLogicalNameScript);
            stringBuilder.Indent1().AppendLine("[Serializable]");
            stringBuilder.Indent1()
                .AppendLine(
                    $"public sealed partial class {ClassName}DataMapper : DataMapperBase<{ClassName}Entity, {ClassName}DataMapper>, IDatabase");
            stringBuilder.Indent1().AppendLine("{");

            stringBuilder.SetSummaryComment("This field is generated automatically, so it can not be edited.", 2);
            stringBuilder.AppendLine();
            stringBuilder.Indent2()
                .AppendLine(
                    $"[SerializeField] private {ClassName}Entity {ClassName.ConvertsSnakeToLowerCamel()} = null;");

            stringBuilder.AppendLine();

            stringBuilder.SetSummaryComment("This getter is generated automatically, so it can not be edited.", 2);
            stringBuilder.AppendLine();
            stringBuilder.Indent2()
                .AppendLine(
                    $"protected override {ClassName}Entity Entity {{ get => {ClassName.ConvertsSnakeToLowerCamel()}; set => {ClassName.ConvertsSnakeToLowerCamel()} = value; }}");

            stringBuilder.Indent2().AppendLine($"public string Schema => \"{schema}\";");

            stringBuilder.Indent1().AppendLine("}");
            stringBuilder.Indent0().AppendLine("}");

            return stringBuilder.ToString();
        }

        public string GenerateDataMapperServiceScript(string nameSpace)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Header
            stringBuilder.SetNameSpace(nameSpace);

            stringBuilder.AppendLine("{");
            stringBuilder.Indent1().AppendLine($"public sealed partial class {ClassName}DataMapperService");
            stringBuilder.Indent1().AppendLine("{");
            stringBuilder.Indent1().AppendLine("}");
            stringBuilder.Indent0().AppendLine("}");

            return stringBuilder.ToString();
        }
    }
}
