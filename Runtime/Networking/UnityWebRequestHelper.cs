using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public static class UnityWebRequestHelper
    {
        /// <summary>
        /// Async get Request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<UnityWebRequest> GetAsync(string uri, Dictionary<string, string> headers = null,
            int timeoutSec = 60, UploadHandler uploadHandler = null, DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        /// <summary>
        /// Async post request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<UnityWebRequest> PostAsync(string uri, string postData,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, postData);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        /// <summary>
        /// Async post request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="form"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<UnityWebRequest> PostAsync(string uri, WWWForm form,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, form);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        /// <summary>
        /// Async put string request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="bodyData"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<UnityWebRequest> PutAsync(string uri, string bodyData,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Put(uri, bodyData);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        /// <summary>
        /// Async put byte[] request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="bodyData"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<UnityWebRequest> PutAsync(string uri, byte[] bodyData,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Put(uri, bodyData);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        /// <summary>
        /// Async delete request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<UnityWebRequest> DeleteAsync(string uri, Dictionary<string, string> headers = null,
            int timeoutSec = 60, UploadHandler uploadHandler = null, DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Delete(uri);
            return await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        internal static async UniTask<UnityWebRequest> Fetch(UnityWebRequest request,
            Dictionary<string, string> headers,
            int timeoutSec, UploadHandler uploadHandler,
            DownloadHandler downloadHandler)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            request.timeout = timeoutSec;

            if (uploadHandler != null)
            {
                request.uploadHandler = uploadHandler;
            }

            if (downloadHandler != null)
            {
                request.downloadHandler = downloadHandler;
            }

            return await request.SendWebRequest();
        }
    }
}
