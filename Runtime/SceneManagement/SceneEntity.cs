using System;
using QuickUnity.Database;
using UnityEngine.SceneManagement;

namespace QuickUnity.SceneManagement
{
    [Serializable]
    public class SceneEntity : EntityBase
    {
        public Scene Scene { get; set; }
    }
}
