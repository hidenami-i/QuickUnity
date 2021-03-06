﻿using System.Text;
using UnityEngine.Networking;

namespace QuickUnity.Networking
{
    /// <summary>
    /// network response for UnityWebRequest api class.
    /// </summary>
    public sealed class QuickWebResponse : QuickWebResponseBase, IWebResponse<string>
    {
        public byte[] Data { get; }

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