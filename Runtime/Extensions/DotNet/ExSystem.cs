using UnityEngine;

namespace QuickUnity.Extensions.DotNet
{
    public static class ExSystem
    {
        /// <summary>
        /// Get the name of the terminal. If you can't get the device name, get the model of the device.
        /// </summary>
        public static string DeviceNameIfNullOrDeviceModel()
        {
            string deviceName = SystemInfo.deviceName;
            if (!string.IsNullOrEmpty(deviceName))
            {
                deviceName = SystemInfo.deviceModel;
            }

            return deviceName;
        }
    }
}