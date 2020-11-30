using System;
using System.Collections.Generic;
using QuickUnity.Editor.Database.Enumerations;
using QuickUnity.Database;
using UnityEngine;

namespace QuickUnity.Editor.Database.TableDefinition
{
    [Serializable]
    public class TableDefinitionRepository : RepositoryBase<TableDefinitionEntity, TableDefinitionRepository>
    {
        [SerializeField] private List<TableDefinitionEntity> tableDefinition = new List<TableDefinitionEntity>();

        protected override List<TableDefinitionEntity> EntityList => tableDefinition;

        public List<TableDefinitionEntity> FindAllMaster()
        {
            return FindAllBy(x => x.SchemaType.IsMaster());
        }

        public List<TableDefinitionEntity> FindAllUser()
        {
            return FindAllBy(x => x.SchemaType.IsUser());
        }

        public List<TableDefinitionEntity> FindAllLocal()
        {
            return FindAllBy(x => x.SchemaType.IsLocal());
        }
    }
}