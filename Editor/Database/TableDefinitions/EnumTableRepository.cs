using System;
using System.Collections.Generic;
using QuickUnity.Runtime.Database;
using UnityEngine;

namespace QuickUnity.Editor.Database.TableDefinition
{
    [Serializable]
    public class EnumTableRepository : RepositoryBase<EnumTableEntity, EnumTableRepository>
    {
        [SerializeField] private List<EnumTableEntity> enumTableEntityList = new List<EnumTableEntity>();
        protected override List<EnumTableEntity> EntityList => enumTableEntityList;
    }
}