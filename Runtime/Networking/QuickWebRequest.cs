using System;
using System.Collections.Generic;
using System.Threading;
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
        /// <param name="uri">The URI of the resource to retrieve via HTTP GET.</param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async UniTask<QuickWebResponse> GetAsync(
            string uri,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null,
            Dictionary<string, string> headers = null,
            TimeSpan timeout = default,
            UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);
            return await SendWebRequest(
                request,
                cancellationToken,
                progress,
                timeout,
                headers,
                uploadHandler,
                downloadHandler);
        }

        /// <summary>
        /// HTTP GET Texture requests asynchronously.
        /// </summary>
        /// <param name="uri">The URI of the image to download.</param>
        /// <param name="nonReadable">If true, the texture's raw data will not be accessible to script. This can conserve memory. Default: false.</param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        /// <returns></returns>
        public static async UniTask<QuickWebResponseTexture> GetTextureAsync(
            string uri,
            bool nonReadable = false,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null,
            Dictionary<string, string> headers = null,
            TimeSpan timeout = default,
            UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri, nonReadable);
            UnityWebRequest result = await UnityWebRequestHelper.SendWebRequest(
                request,
                cancellationToken,
                progress,
                timeout,
                headers,
                uploadHandler,
                downloadHandler);
            return new QuickWebResponseTexture(result);
        }


        /// <summary>
        /// HTTP GET AudioClip requests asynchronously.
        /// </summary>
        /// <param name="uri">The URI of the audio clip to download.</param>
        /// <param name="audioType">The type of audio encoding for the downloaded audio clip. See AudioType.</param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async UniTask<QuickWebResponseTexture> GetAudioClipAsync(
            string uri,
            AudioType audioType,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null,
            Dictionary<string, string> headers = null,
            TimeSpan timeout = default,
            UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(uri, audioType);
            UnityWebRequest result = await UnityWebRequestHelper.SendWebRequest(
                request,
                cancellationToken,
                progress,
                timeout,
                headers,
                uploadHandler,
                downloadHandler);
            return new QuickWebResponseTexture(result);
        }

        /// <summary>
        /// HTTP GET AssetBundle requests asynchronously.
        /// </summary>
        /// <param name="uri">The URI of the asset bundle to download.</param>
        /// <param name="crc">If nonzero, this number will be compared to the checksum of the downloaded asset bundle data. If the CRCs do not match, an error will be logged and the asset bundle will not be loaded. If set to zero, CRC checking will be skipped.</param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        /// <returns></returns>
        public static async UniTask<QuickWebResponseTexture> GetAssetBundleAsync(
            string uri,
            uint crc = 0,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null,
            Dictionary<string, string> headers = null,
            TimeSpan timeout = default,
            UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri, crc);
            UnityWebRequest result = await UnityWebRequestHelper.SendWebRequest(
                request,
                cancellationToken,
                progress,
                timeout,
                headers,
                uploadHandler,
                downloadHandler);
            return new QuickWebResponseTexture(result);
        }

        /// <summary>
        /// HTTP POST requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickWebResponse> PostAsync(
            string uri,
            string postData,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null,
            Dictionary<string, string> headers = null,
            TimeSpan timeout = default,
            UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, postData);
            return await SendWebRequest(
                request,
                cancellationToken,
                progress,
                timeout,
                headers,
                uploadHandler,
                downloadHandler);
        }

        /// <summary>
        /// HTTP POST requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="form"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickWebResponse> PostAsync(
            string uri,
            WWWForm form,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null,
            Dictionary<string, string> headers = null,
            TimeSpan timeout = default,
            UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, form);
            return await SendWebRequest(
                request,
                cancellationToken,
                progress,
                timeout,
                headers,
                uploadHandler,
                downloadHandler);
        }

        /// <summary>
        /// HTTP PUT requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="bodyData"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickWebResponse> PutAsync(
            string uri,
            string bodyData,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null,
            Dictionary<string, string> headers = null,
            TimeSpan timeout = default,
            UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Put(uri, bodyData);
            return await SendWebRequest(
                request,
                cancellationToken,
                progress,
                timeout,
                headers,
                uploadHandler,
                downloadHandler);
        }

        /// <summary>
        /// HTTP PUT requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="bodyData"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickWebResponse> PutAsync(
            string uri,
            byte[] bodyData,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null,
            Dictionary<string, string> headers = null,
            TimeSpan timeout = default,
            UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Put(uri, bodyData);
            return await SendWebRequest(
                request,
                cancellationToken,
                progress,
                timeout,
                headers,
                uploadHandler,
                downloadHandler);
        }

        /// <summary>
        /// HTTP DELETE requests asynchronously.
        /// </summary>
        /// <param name="uri">The URI to which a DELETE request should be sent.</param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        /// <param name="cancellationToken"></param>
        public static async UniTask<QuickWebResponse> DeleteAsync(
            string uri,
            CancellationToken cancellationToken = default,
            IProgress<float> progress = null,
            Dictionary<string, string> headers = null,
            TimeSpan timeout = default,
            UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Delete(uri);
            return await SendWebRequest(
                request,
                cancellationToken,
                progress,
                timeout,
                headers,
                uploadHandler,
                downloadHandler);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress"></param>
        /// <param name="timeout"></param>
        /// <param name="headers"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        /// <returns></returns>
        private static async UniTask<QuickWebResponse> SendWebRequest(
            UnityWebRequest request,
            CancellationToken cancellationToken,
            IProgress<float> progress,
            TimeSpan timeout,
            Dictionary<string, string> headers,
            UploadHandler uploadHandler,
            DownloadHandler downloadHandler)
        {
            UnityWebRequest result = await UnityWebRequestHelper.SendWebRequest(
                request,
                cancellationToken,
                progress,
                timeout,
                headers,
                uploadHandler,
                downloadHandler);
            return new QuickWebResponse(result);
        }
    }
}
