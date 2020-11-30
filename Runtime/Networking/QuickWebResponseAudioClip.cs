using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public sealed class QuickWebResponseAudioClip : QuickWebResponseBase, IWebResponse<AudioClip>
    {
        private readonly AudioClip result;

        public QuickWebResponseAudioClip(UnityWebRequest request) : base(request)
        {
            result = DownloadHandlerAudioClip.GetContent(request);
            request.Dispose();
        }

        public AudioClip Result() => result;
    }
}