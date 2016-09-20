using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.Client;
using System.Threading;
using Newtonsoft.Json;
using System.Configuration;

namespace simulator
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "";
        static string iotHubUri = "";
        static string deviceId = null;
        static string deviceKey = null;
        static Random random = new Random();
        static int max = 100;
        static int min = 0;
        static int duration = 60;
        static bool run = true;
        static private DeviceClient CreateDeviceClient(string deviceId, string deviceKey)
        {
#if true
            //AMQP (default)
            return DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey),
                Microsoft.Azure.Devices.Client.TransportType.Amqp_WebSocket_Only);
#else
            //HTTPS
            return DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey),
                                                Microsoft.Azure.Devices.Client.TransportType.Http1);
#endif
        }
        /// <summary>
        /// Simulator a device
        /// </summary>
        /// <param name="args">Usage: sumulator {deviceid} {min} {max}</param>
        static void Main(string[] args)
        {
            #region added
            if (args.Length < 3)
            {
                Error("Usage: sumulator {deviceid} {max} {min}");
                Wait();
                return;
            }
            deviceId = args[0];
            max = int.Parse(args[2]);
            min = int.Parse(args[1]);
            run = true;
            connectionString = ConfigurationManager.AppSettings["iotHubConnectionString"];
            iotHubUri = ConfigurationManager.AppSettings["iotHubUri"];

            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            #endregion
            AddDeviceAsync().Wait();


            SendDeviceToCloudMessagesAsync();
            ReceiveCommandAsync();
            Wait("Press [ENTER] to exit...");
            run = false;
            RemoveDeviceAsync().Wait();
        }
        static string GenerateMessage(int seq, string message)
        {
            var msg = TelemetryData.Random(deviceId, string.Format("{0}{1}", DateTime.UtcNow.ToString("yyyymmdd"), seq.ToString("0000000")), message, min, max);
            return JsonConvert.SerializeObject(msg);
        }
        static void Wait(string msg = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                Log("Press [ENTER] to continue...");
            }
            else
            {
                Log(msg);
            }
            Console.ReadLine();
        }
        
        static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        static void Log(string msg)
        {
            Console.ResetColor();
            Console.WriteLine(msg);
        }
        private async static Task RemoveDeviceAsync()
        {
            var device = await registryManager.GetDeviceAsync(deviceId);
            await registryManager.RemoveDeviceAsync(device);
        }
        private async static Task AddDeviceAsync()
        {
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            deviceKey = device.Authentication.SymmetricKey.SecondaryKey;
            Log($"device id {deviceId} : {deviceKey}");
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            int i = 0;
            DateTime start = DateTime.Now;
            while (run)
            {
                i++;
                string telemetry = GenerateMessage(i, $"message:{i}");
                DeviceClient deviceClient = CreateDeviceClient(deviceId, deviceKey);
                await deviceClient.SendEventAsync(new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(telemetry)));
                await deviceClient.CloseAsync();
                Console.WriteLine("{0} ==> Sending message: {1}", DateTime.Now, telemetry);

                Thread.Sleep(100);

                var time = DateTime.Now - start;
                if(time.Seconds >= duration)
                {
                    Success($"Sent {i} messages, druation {time.Seconds} seconds");
                    run = false;
                }
            }
        }

        private async static void ReceiveCommandAsync()
        {
            while (run)
            {
                DeviceClient deviceClient = CreateDeviceClient(deviceId, deviceKey);
                var cmd = await deviceClient.ReceiveAsync();
                if (cmd != null)
                {
                    Success(Encoding.UTF8.GetString(cmd.GetBytes()));
                    await deviceClient.CompleteAsync(cmd);
                }

                Thread.Sleep(100);
            }
        }
    }
}
