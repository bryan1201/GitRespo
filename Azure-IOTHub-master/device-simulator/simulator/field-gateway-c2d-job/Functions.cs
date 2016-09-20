using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices;
using System.Threading;

namespace field_gateway_c2d_job
{
    public class Functions
    {
        /// <summary>
        /// In this sample gateway, device information stored at \APP_DATA\{deviceId}; This function get all device information from the folder
        /// </summary>
        /// <returns></returns>
        public async static Task<string[]> GetAllDevicesAsync()
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["iotHubOwnerConnectionString"];
            RegistryManager mgr = RegistryManager.CreateFromConnectionString(connString.ConnectionString);
            var hostname = connString.ConnectionString.Split(';')[0].Split('=')[1];
            var devices = await mgr.GetDevicesAsync(1000);
            return devices.Select(d => $"HostName={hostname};DeviceId={d.Id};SharedAccessKey={d.Authentication.SymmetricKey.PrimaryKey}").ToArray();
        }
        private static void WriteFile(string deviceId,string log)
        {
            var root = Environment.GetEnvironmentVariable("WEBROOT_PATH");
            var fn = System.IO.Path.Combine(root, $"App_Data\\{deviceId}.txt");
            if (!System.IO.File.Exists(fn))
            {
                using (var sw = File.CreateText(fn))
                {
                    sw.Write(log);
                }
            }
            else
            {
                File.AppendAllText(fn, log + Environment.NewLine);
            }
        }
        public static async Task ReceiveC2DAsync()
        {
            while (true)
            {
                var conns = await GetAllDevicesAsync();

                if (conns != null && conns.Length > 0)
                {
                    foreach (var conn in conns)
                    {
                        Thread.Sleep(1000);
                        try
                        {
                            Console.WriteLine($"Checking [{conn}]...");
                            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(conn);
                            var cmd = await deviceClient.ReceiveAsync(TimeSpan.FromMilliseconds(3000));
                            if (cmd != null)
                            {
                                Console.WriteLine($"...Got message");
                                using (var sr = new StreamReader(cmd.GetBodyStream()))
                                {
                                    var msg = sr.ReadToEnd();
                                    var deviceId = conn.Split(';')[1].Split('=')[1];
                                    WriteFile(deviceId, msg);
                                    Console.WriteLine($"*** Received:{msg}");
                                }
                                await deviceClient.CompleteAsync(cmd);
                            }
                            else
                            {
                                Console.WriteLine("...no message");
                            }
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine($"Error handling {conn}");
                            Console.WriteLine($"Exception:{exp.Message}");
                        }
                    }
                }
                
            }
        }
    }
}
