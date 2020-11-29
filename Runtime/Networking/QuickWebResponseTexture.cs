using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public class QuickWebResponseTexture : QuickWebResponseBase
    {
        public Texture2D Result { get; } = Texture2D.whiteTexture;

        public QuickWebResponseTexture(UnityWebRequest request) : base(request)
        {
            Result = DownloadHandlerTexture.GetContent(request);
            request.Dispose();
        }
    }
}
