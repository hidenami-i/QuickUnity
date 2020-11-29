using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;

namespace QuickUnity.Networking
{
    /// <summary>
    /// Simple UnityWebRequest
    /// </summary>
    public static class QuickWebRequest
    {
        /// <summary>
        /// HTTP GET requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        /// <returns></returns>
        public static async UniTask<QuickWebResponse> GetAsync(string uri,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        ///  <summary>
        /// 　HTTP GET Texture requests asynchronously.
        ///  </summary>
        ///  <param name="uri">The URI of the image to download.</param>
        ///  <param name="nonReadable">If true, the texture's raw data will not be accessible to script. This can conserve memory. Default: false.</param>
        ///  <param name="headers"></param>
        ///  <param name="timeoutSec"></param>
        ///  <param name="uploadHandler"></param>
        ///  <param name="downloadHandler"></param>
        ///  <returns></returns>
        public static async UniTask<QuickWebResponseTexture> GetTextureAsync(string uri, bool nonReadable = false,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri, nonReadable);
            UnityWebRequest result =
                await UnityWebRequestHelper.SendWebRequest(request, headers, timeoutSec, uploadHandler,
                    downloadHandler);
            return new QuickWebResponseTexture(result);
        }


        /// <summary>
        /// HTTP GET AudioClip requests asynchronously.
        /// </summary>
        /// <param name="uri">The URI of the audio clip to download.</param>
        /// <param name="audioType">The type of audio encoding for the downloaded audio clip. See AudioType.</param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        /// <returns></returns>
        public static async UniTask<QuickWebResponseTexture> GetAudioClipAsync(string uri, AudioType audioType,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(uri, audioType);
            UnityWebRequest result =
                await UnityWebRequestHelper.SendWebRequest(request, headers, timeoutSec, uploadHandler,
                    downloadHandler);
            return new QuickWebResponseTexture(result);
        }

        ///  <summary>
        /// 　HTTP GET AssetBundle requests asynchronously.
        ///  </summary>
        ///  <param name="uri">The URI of the asset bundle to download.</param>
        ///  <param name="crc">If nonzero, this number will be compared to the checksum of the downloaded asset bundle data. If the CRCs do not match, an error will be logged and the asset bundle will not be loaded. If set to zero, CRC checking will be skipped.</param>
        ///  <param name="headers"></param>
        ///  <param name="timeoutSec"></param>
        ///  <param name="uploadHandler"></param>
        ///  <param name="downloadHandler"></param>
        ///  <returns></returns>
        public static async UniTask<QuickWebResponseTexture> GetAssetBundleAsync(string uri, uint crc = 0,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri, crc);
            UnityWebRequest result =
                await UnityWebRequestHelper.SendWebRequest(request, headers, timeoutSec, uploadHandler,
                    downloadHandler);
            return new QuickWebResponseTexture(result);
        }

        /// <summary>
        /// HTTP POST requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickWebResponse> PostAsync(string uri, string postData,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, postData);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        /// <summary>
        /// HTTP POST requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="form"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickWebResponse> PostAsync(string uri, WWWForm form,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, form);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        /// <summary>
        /// HTTP PUT requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="bodyData"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickWebResponse> PutAsync(string uri, string bodyData,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Put(uri, bodyData);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        /// <summary>
        /// HTTP PUT requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="bodyData"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickWebResponse> PutAsync(string uri, byte[] bodyData,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Put(uri, bodyData);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        /// <summary>
        /// HTTP DELETE requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickWebResponse> DeleteAsync(string uri,
            Dictionary<string, string> headers = null,
            int timeoutSec = 60, UploadHandler uploadHandler = null, DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Delete(uri);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        private static async UniTask<QuickWebResponse> Fetch(
            UnityWebRequest request,
            Dictionary<string, string> headers,
            int timeoutSec, UploadHandler uploadHandler,
            DownloadHandler downloadHandler)
        {
            UnityWebRequest result =
                await UnityWebRequestHelper.SendWebRequest(request, headers, timeoutSec, uploadHandler,
                    downloadHandler);
            return new QuickWebResponse(result);
        }
    }
}
