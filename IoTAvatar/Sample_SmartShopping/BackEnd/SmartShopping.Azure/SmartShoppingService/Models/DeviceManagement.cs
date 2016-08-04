using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace SmartShoppingDemoService.Models
{
    public class DeviceManagement
    {
        private static string deviceKey = String.Empty;

        private static string connectionString = ConfigurationManager.AppSettings["IothubConnectionString"];

        private static RegistryManager registryManager = RegistryManager.CreateFromConnectionString(connectionString);

        public async static Task AddDeviceAsync(string deviceId)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            Microsoft.Azure.Devices.Device device;

            try
            {
                device = await registryManager.AddDeviceAsync(new Microsoft.Azure.Devices.Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            deviceKey = device.Authentication.SymmetricKey.PrimaryKey;
        }

        public async static Task RemoveDeviceAsync(string deviceId)
        {
            Microsoft.Azure.Devices.Device device =
                await registryManager.GetDeviceAsync(deviceId);
            try
            {
                await registryManager.RemoveDeviceAsync(device);
            }
            catch
            { }
        }

        public static string GetDeviceKey()
        {
            return deviceKey;
        }

        public static string GetIothubConnectionString()
        {
            return connectionString;
        }
    }
}