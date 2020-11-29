using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public sealed class QuickWebResponseTexture : QuickWebResponseBase, IWebResponse<Texture2D>
    {
        private readonly Texture2D result;

        public QuickWebResponseTexture(UnityWebRequest request) : base(request)
        {
            result = DownloadHandlerTexture.GetContent(request);
            request.Dispose();
        }

        public Texture2D Result() => result;
    }
}
