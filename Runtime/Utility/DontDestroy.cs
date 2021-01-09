using UnityEngine;

namespace App.Utility
{
    public class DontDestroy : MonoBehaviour
    {
        private void Start() => DontDestroyOnLoad(gameObject);
    }
}
