using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public static class UnityWebRequestHelper
    {
        /// <summary>
        /// HTTP GET requests asynchronously.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<UnityWebRequest> GetAsync(
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
        public static async UniTask<UnityWebRequest> PostAsync(
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
        public static async UniTask<UnityWebRequest> PostAsync(
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
        public static async UniTask<UnityWebRequest> PutAsync(
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
        public static async UniTask<UnityWebRequest> PutAsync(
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
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress"></param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="uploadHandler"></param>
        /// <param name="downloadHandler"></param>
        public static async UniTask<UnityWebRequest> DeleteAsync(
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

        internal static async UniTask<UnityWebRequest> SendWebRequest(
            UnityWebRequest request,
            CancellationToken cancellationToken,
            IProgress<float> progress,
            TimeSpan timeout,
            Dictionary<string, string> headers,
            UploadHandler uploadHandler,
            DownloadHandler downloadHandler)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            CancellationTokenSource linkToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            if (timeout == default)
            {
                timeout = TimeSpan.FromSeconds(120);
            }

            linkToken.CancelAfterSlim(timeout);

            if (uploadHandler != null)
            {
                request.uploadHandler = uploadHandler;
            }

            if (downloadHandler != null)
            {
                request.downloadHandler = downloadHandler;
            }

            try
            {
                await request.SendWebRequest()
                             .ToUniTask(progress: progress, cancellationToken: linkToken.Token);
            }
            catch (OperationCanceledException)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    throw new TimeoutException();
                }
            }
            finally
            {
                // If it is not a timeout exception, cancel the process.
                if (!linkToken.IsCancellationRequested)
                {
                    linkToken.Cancel();
                }
            }

            return request;
        }
    }
}
