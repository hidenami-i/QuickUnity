using Cysharp.Threading.Tasks;
using UnityEngine;

namespace QuickUnity.Networking
{
    public static class NTP
    {
        private const string Uri = "https://ntp-a1.nict.go.jp/cgi-bin/json";

        /// <summary>
        /// Gets the server time as async.
        /// </summary>
        public static async UniTask<NTPResponse> GetAsync()
        {
            var realtimeSinceStartup = Time.realtimeSinceStartup;
            QuickResponse response = await QuickRequest.GetAsyncAsQuickResponse(Uri);

            if (response.HasError(out QuickRequestException exception))
            {
                return new NTPResponse(exception);
            }

            NTPResponse ntpResponse = JsonUtility.FromJson<NTPResponse>(response.GetText());

            //　Adjusts the latency of the communication from the server to the client.
            var latency = Time.realtimeSinceStartup - realtimeSinceStartup;
            ntpResponse.AdjustTime(latency);

            return ntpResponse;
        }
    }
}
