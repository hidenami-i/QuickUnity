using System;
using System.Collections.Generic;
using QuickUnity.Core;
using QuickUnity.Database;
using UnityEngine;

namespace QuickUnity.SceneManagement
{
    [Serializable]
    public class SceneRepository : RepositoryBase<SceneEntity, SceneRepository>
    {
        [SerializeField] private List<SceneEntity> sceneList = new List<SceneEntity>();

        protected override List<SceneEntity> EntityList => sceneList;
    }
}
