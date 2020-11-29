using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public class QuickWebResponseAssetBundle : QuickWebResponseBase
    {
        public AssetBundle Result { get; }

        public QuickWebResponseAssetBundle(UnityWebRequest request) : base(request)
        {
            Result = DownloadHandlerAssetBundle.GetContent(request);
            request.Dispose();
        }
    }
}
