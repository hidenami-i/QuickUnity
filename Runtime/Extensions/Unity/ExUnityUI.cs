using UnityEngine.Events;
using UnityEngine.UI;

namespace QuickUnity.Extensions.Unity
{
    public static class ExUnityUI
    {
        public static void OnClick(this Button button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }
    }
}
