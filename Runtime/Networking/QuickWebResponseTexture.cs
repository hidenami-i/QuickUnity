using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public class QuickWebResponseTexture
    {
        public QuickWebResponseTexture(UnityWebRequest request)
        {
            using (request)
            {
                Texture2D texture2D = DownloadHandlerTexture.GetContent(request);
            }
        }
    }
}
