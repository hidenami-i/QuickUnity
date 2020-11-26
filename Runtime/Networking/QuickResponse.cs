using System.Net;
using System.Text;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    /// <summary>
    /// network response for UnityWebRequest api class.
    /// </summary>
    public class QuickResponse
    {
        /// <summary>
        /// response data
        /// </summary>
        private readonly byte[] data;

        private readonly QuickRequestException requestException;

        public QuickResponse(UnityWebRequest request)
        {
            using (request)
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

                // 200 <= code <= 299
                if (HttpStatusCode.OK <= responseCode && responseCode < HttpStatusCode.Ambiguous)
                {
                    data = request.downloadHandler.data;
                }
                else
                {
                    requestException = new QuickRequestException(request);
                }
            }
        }

        /// <summary>
        /// Gets request url.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Gets response byte data.
        /// </summary>
        /// <returns></returns>
        public byte[] GetData()
        {
            return data;
        }

        /// <summary>
        /// Gets response text data.
        /// </summary>
        /// <returns></returns>
        public string GetText()
        {
            if (data != null && data.Length > 0)
            {
                return Encoding.UTF8.GetString(data);
            }

            return "";
        }

        public bool HasError()
        {
            return requestException != null;
        }

        public bool HasError(out QuickRequestException exception)
        {
            exception = requestException;
            return requestException != null;
        }
    }
}
