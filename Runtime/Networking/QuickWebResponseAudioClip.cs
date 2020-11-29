using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public class QuickWebResponseAudioClip : QuickWebResponseBase
    {
        public AudioClip Result { get; }

        public QuickWebResponseAudioClip(UnityWebRequest request) : base(request)
        {
            Result = DownloadHandlerAudioClip.GetContent(request);
            request.Dispose();
        }
    }
}
