using System.Net;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    public abstract class QuickWebResponseBase
    {
        private readonly QuickRequestException requestException;

        protected QuickWebResponseBase(UnityWebRequest request)
        {
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                requestException = new QuickRequestException(request);
                return;
            }

            Url = request.url;
            HttpStatusCode responseCode = (HttpStatusCode) request.responseCode;

            // 200 > code || code >= 300
            if (HttpStatusCode.OK > responseCode || responseCode >= HttpStatusCode.Ambiguous)
            {
                requestException = new QuickRequestException(request);
            }
        }

        /// <summary>
        /// Gets request url.
        /// </summary>
        public string Url { get; }

        public bool HasError() => requestException != null;

        public bool HasError(out QuickRequestException exception)
        {
            exception = requestException;
            return requestException != null;
        }
    }
}
