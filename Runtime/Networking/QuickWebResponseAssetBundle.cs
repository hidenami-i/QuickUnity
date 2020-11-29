using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public sealed class QuickWebResponseAssetBundle : QuickWebResponseBase, IWebResponse<AssetBundle>
    {
        private readonly AssetBundle result;

        public QuickWebResponseAssetBundle(UnityWebRequest request) : base(request)
        {
            result = DownloadHandlerAssetBundle.GetContent(request);
            request.Dispose();
        }

        public AssetBundle Result() => result;
    }
}
