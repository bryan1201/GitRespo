//
// References:
//
//  Tutorial: Get started with IoT Hub:
//    https://github.com/Azure/azure-content/blob/master/articles/iot-hub/iot-hub-csharp-csharp-getstarted.md
//    https://github.com/Azure/azure-content/blob/master/includes/iot-hub-get-started-cloud-csharp.md
//    https://azure.microsoft.com/en-in/documentation/articles/iot-hub-csharp-csharp-getstarted/
//
//  Tutorial: How to send cloud-to-device messages with IoT Hub:
//     https://github.com/Azure/azure-content/blob/master/articles/iot-hub/iot-hub-csharp-csharp-c2d.md
//     https://github.com/Azure/azure-content/blob/master/includes/iot-hub-c2d-cloud-csharp.md
//     https://azure.microsoft.com/en-in/documentation/articles/iot-hub-csharp-csharp-c2d/
//
//  Windows IoT Core and Azure IoT Hub – Putting the ‘I’ in IoT:
//    https://blogs.windows.com/buildingapps/2015/12/09/windows-iot-core-and-azure-iot-hub-putting-the-i-in-iot/
//
//
// Install NuGetPackage:
//  1. Microsoft.Azure.Devices v1.0.0-preview-??? (required to select "Include Prerelease" option)
//  2. WindowsAzure.ServiceBus
//  3. Newtonsoft.Json
//

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using SmartShoppingDemoWeb.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

public class ProcessDevice
{
    private static string deviceId1 = "Avatar01";
    private static string deviceId2 = "Avatar02";
    private static string deviceKey1 = String.Empty;
    private static string deviceKey2 = String.Empty;
    private static string connectionString = String.Empty;
    private static RegistryManager registryManager;
    private static SmartShoppingDemoWebContext db = new SmartShoppingDemoWebContext();

    public static void CreateDevices()
    {
        var appSettings = ConfigurationManager.AppSettings;

        connectionString = appSettings["IothubConnectionString"];
        //connectionString = SmartShoppingDemoWeb.Properties.Settings.Default.IothubConnectionString;

        var ignored = Task.Run(async () =>
        {
            await AddDeviceAsync(deviceId1);
            await AddDeviceAsync(deviceId2);
        });
        // Create a new device "Device01" and get its Primiary Key
    }

    private async static Task AddDeviceAsync(string deviceId)
    {
        Device device;
        string deviceKey;

        registryManager = RegistryManager.CreateFromConnectionString(connectionString);

        try
        {
            device = await registryManager.AddDeviceAsync(new Device(deviceId));
        }
        catch (DeviceAlreadyExistsException)
        {
            device = await registryManager.GetDeviceAsync(deviceId);
        }
        deviceKey = device.Authentication.SymmetricKey.PrimaryKey;

        if (deviceId == deviceId1)
            deviceKey1 = deviceKey;
        else
            deviceKey2 = deviceKey;
    }

    public static string GetDeviceKey(string deviceId)
    {
        if (deviceId == deviceId1)
            return deviceKey1;
        else
            return deviceKey2;
    }

    public static string GetIothubConnectionString()
    {
        return connectionString;
    }
}
