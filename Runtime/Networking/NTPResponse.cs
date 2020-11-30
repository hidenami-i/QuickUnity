using System;
using QuickUnity.Extensions.DotNet;
using UnityEngine;

namespace QuickUnity.Networking
{
    [Serializable]
    public class NTPResponse
    {
        [SerializeField] private double st = 0;

        // unused field
        // [SerializeField] private string id;
        // [SerializeField] private double it;
        // [SerializeField] private int leap;
        // [SerializeField] private long next;
        // [SerializeField] private int step;

        private readonly QuickRequestException requestException;

        private DateTime utcNow;

        internal void AdjustTime(float latency)
        {
            utcNow = ExDateTime.FromEpochSeconds(st).AddMilliseconds(latency);
        }

        internal NTPResponse(QuickRequestException requestException)
        {
            this.requestException = requestException;
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

        /// <summary>
        /// Gets the UTC Time.
        /// </summary>
        public DateTime UtcNow => utcNow;

        /// <summary>
        /// Gets the Local Time.
        /// </summary>
        public DateTime Now => utcNow.ToLocalTime();
    }
}