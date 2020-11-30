using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public class QuickRequestException : Exception
    {
        /// <summary>
        /// returns UnityWebRequest error property.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// become [true] when ResponseCode of UnityWebRequest is more than 0.
        /// </summary>
        public bool HasResponse { get; }

        /// <summary>
        /// returns UnityWebRequest DownloadHandler text
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// returns UnityWebRequest responseCode as System.Net.HttpStatusCode
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// returns UnityWebRequest responseHeaders.
        /// </summary>
        public Dictionary<string, string> ResponseHeaders { get; }

        /// <summary>
        /// cache the text because if www was disposed, can't access it.
        /// </summary>
        /// <param name="request"></param>
        public QuickRequestException(UnityWebRequest request)
        {
            ErrorMessage = request.error;
            ResponseHeaders = request.GetResponseHeaders() == null
                ? new Dictionary<string, string>()
                : request.GetResponseHeaders();
            StatusCode = (HttpStatusCode) request.responseCode;

            if (request.downloadHandler != null)
            {
                Text = request.downloadHandler.text;
            }

            // 0 is not reachable error
            HasResponse = request.responseCode != 0;
        }

        public override string ToString()
        {
            return
                $"{base.ToString()}, {nameof(ErrorMessage)}: {ErrorMessage}, {nameof(HasResponse)}: {HasResponse}, {nameof(Text)}: {Text}, {nameof(StatusCode)}: {StatusCode}, {nameof(ResponseHeaders)}: {ResponseHeaders}";
        }
    }
}