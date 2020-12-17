using System;
using QuickUnity.Database;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickUnity.SceneManagement
{
    [Serializable]
    public class SceneEntity : EntityBase
    {
        [SerializeField] private Scene scene;
        [SerializeField] private bool isRootScene;

        public SceneEntity()
        {
        }

        public SceneEntity(Scene scene, bool isRootScene)
        {
            this.scene = scene;
            this.isRootScene = isRootScene;
        }
    }
}
