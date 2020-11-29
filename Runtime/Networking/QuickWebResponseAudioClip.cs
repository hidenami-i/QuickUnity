using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public class QuickWebResponseAudioClip
    {
        public QuickWebResponseAudioClip(UnityWebRequest request)
        {
            using (request)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
            }
        }
    }
}
