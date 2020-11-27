using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using QuickUnity.Extensions.Unity;
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
        /// Async get Request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        /// <returns></returns>
        public static async UniTask<QuickResponse> GetAsyncAsQuickResponse(string uri,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);
            return await FetchAsQuickResponse(request, headers, timeoutSec, uploadHandler, downloadHandler);
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
        /// <param name="postData"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickResponse> PostAsyncAsQuickResponse(string uri, string postData,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, postData);
            return await FetchAsQuickResponse(request, headers, timeoutSec, uploadHandler, downloadHandler);
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
        /// Async post request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="form"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickResponse> PostAsyncAsQuickResponse(string uri, WWWForm form,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Post(uri, form);
            return await FetchAsQuickResponse(request, headers, timeoutSec, uploadHandler, downloadHandler);
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
        /// Async put string request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="bodyData"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickResponse> PutAsyncAsQuickResponse(string uri, string bodyData,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Put(uri, bodyData);
            return await FetchAsQuickResponse(request, headers, timeoutSec, uploadHandler, downloadHandler);
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
        /// Async put byte[] request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="bodyData"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickResponse> PutAsyncAsQuickResponse(string uri, byte[] bodyData,
            Dictionary<string, string> headers = null, int timeoutSec = 60, UploadHandler uploadHandler = null,
            DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Put(uri, bodyData);
            return await FetchAsQuickResponse(request, headers, timeoutSec, uploadHandler, downloadHandler);
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

        /// <summary>
        /// Async delete request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="headers"></param>
        /// <param name="timeoutSec"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<QuickResponse> DeleteAsyncAsQuickResponse(string uri,
            Dictionary<string, string> headers = null,
            int timeoutSec = 60, UploadHandler uploadHandler = null, DownloadHandler downloadHandler = null)
        {
            UnityWebRequest request = UnityWebRequest.Delete(uri);
            return await FetchAsQuickResponse(request, headers, timeoutSec, uploadHandler, downloadHandler);
        }

        private static async UniTask<UnityWebRequest> Fetch(UnityWebRequest request, Dictionary<string, string> headers,
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

        private static async UniTask<QuickResponse> FetchAsQuickResponse(
            UnityWebRequest request,
            Dictionary<string, string> headers,
            int timeoutSec, UploadHandler uploadHandler,
            DownloadHandler downloadHandler)
        {
            UnityWebRequest result = await Fetch(request, headers, timeoutSec, uploadHandler, downloadHandler);
            return new QuickResponse(result);
        }

        private static IEnumerator FetchAsCoroutine(UnityWebRequest request, Action<UnityWebRequest> completed,
            Dictionary<string, string> headers, int timeoutSec, bool chunkedTransfer, UploadHandler uploadHandler,
            DownloadHandler downloadHandler)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            // request.chunkedTransfer = chunkedTransfer;
            request.timeout = timeoutSec;

            if (uploadHandler != null)
            {
                request.uploadHandler = uploadHandler;
            }

            if (downloadHandler != null)
            {
                request.downloadHandler = downloadHandler;
            }

            UnityWebRequestAsyncOperation operation = request.SendWebRequest();

            while (true)
            {
                if (request.result == UnityWebRequest.Result.ProtocolError ||
                    request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError(request.error);
                    yield break;
                }

                if (operation.isDone)
                {
                    yield break;
                }

                yield return null;

                ulong size = 0;
                var responseHeader = request.GetResponseHeader("Content-Length");
                if (responseHeader != null)
                {
                    ulong.TryParse(responseHeader, out size);
                }

                ExDebug.Log($"progress : {operation.progress}");
                ExDebug.Log($"download size : {request.downloadedBytes}, size : {size}");
            }
        }
    }
}
