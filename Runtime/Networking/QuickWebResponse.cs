using System.Text;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    /// <summary>
    /// network response for UnityWebRequest api class.
    /// </summary>
    public class QuickWebResponse : QuickWebResponseBase
    {
        private byte[] Data { get; } = new byte[0];

        public QuickWebResponse(UnityWebRequest request) : base(request)
        {
            Data = request.downloadHandler.data;
            request.Dispose();
        }

        /// <summary>
        /// Gets response text data.
        /// </summary>
        /// <returns></returns>
        public string Result()
        {
            if (Data != null && Data.Length > 0)
            {
                return Encoding.UTF8.GetString(Data);
            }

            return "";
        }
    }
}
