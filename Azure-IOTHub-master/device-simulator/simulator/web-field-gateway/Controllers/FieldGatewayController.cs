
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client.Exceptions;
using System.Web;
using System.IO;
using web_field_gateway.Models;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using System.Configuration;

namespace web_field_gateway.Controllers
{
    public class FieldGatewayController : ApiController
    {
        static string connectionString = null;
        static string iotHubUri = null;

        static FieldGatewayController()
        {
            connectionString = ConfigurationManager.AppSettings["iotHubOwnerConnectionString"];
            iotHubUri = ConfigurationManager.AppSettings["iotHubUrl"];
        }
        private void SaveDeviceIdentity(string deviceId, string deviceKey)
        {
            var fn = Path.Combine(HttpContext.Current.Server.MapPath("~/APP_DATA"),
                                deviceId);
            if (File.Exists(fn))
            {
                File.Delete(fn);
            }
            File.WriteAllText(fn, deviceKey);
        }
        private async Task<string> GetDeviceKey(string deviceId)
        {
            var fn = Path.Combine(HttpContext.Current.Server.MapPath("~/APP_DATA"),
                                deviceId);
            if (File.Exists(fn))
            {
                return File.ReadAllText(fn);
            }
            else
            {
                return await Register(deviceId);
            }
        }
        private void DeleteDeviceKey(string deviceId)
        {
            var fn = Path.Combine(HttpContext.Current.Server.MapPath("~/APP_DATA"),
                                deviceId);
            if (File.Exists(fn))
            {
                File.Delete(fn);
            }
        }
        public async Task<string> Register(string deviceId)
        {
            Device device = null;
            RegistryManager registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            try
            {
                device = await registryManager.AddDeviceAsync(
                        new Device(deviceId)
                    );
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }catch(Microsoft.Azure.Devices.Common.Exceptions.DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            if(device != null)
            {
                var deviceKey  =device.Authentication.SymmetricKey.PrimaryKey;
                SaveDeviceIdentity(deviceId, deviceKey);
                return deviceKey;
            }
            return string.Empty;
        }
        public async Task Unregister(string deviceId)
        {
            RegistryManager registryManager = RegistryManager.CreateFromConnectionString(connectionString);

            var device = await registryManager.GetDeviceAsync(deviceId);
            await registryManager.RemoveDeviceAsync(device);
        }
        public async Task SendTelemetry(TelemetryData telemetry)
        {
            try {
#if false
            DeviceClient dc = DeviceClient.Create(iotHubUri,
                new DeviceAuthenticationWithRegistrySymmetricKey(
                        telemetry.DeviceId,
                        GetDeviceKey(telemetry.DeviceId)
                    ));
#else
                var key = await GetDeviceKey(telemetry.DeviceId);
                DeviceClient dc = DeviceClient.CreateFromConnectionString(
                    $"HostName={iotHubUri};DeviceId={telemetry.DeviceId};SharedAccessKey={key}",
                    Microsoft.Azure.Devices.Client.TransportType.Amqp);
#endif
                var text = JsonConvert.SerializeObject(telemetry);
                var buffer = Encoding.UTF8.GetBytes(text);
                await dc.SendEventAsync(new Microsoft.Azure.Devices.Client.Message(buffer));
                await dc.CloseAsync();
            }
            catch(Exception exp)
            {
                System.Diagnostics.Trace.WriteLine($"[SendTelemetry]{exp.Message}");
                System.Diagnostics.Trace.WriteLine($"Stack Trace: {exp.StackTrace}");
                if (exp.InnerException != null)
                {
                    System.Diagnostics.Trace.WriteLine($"      - {exp.InnerException.Message}");
                    System.Diagnostics.Trace.WriteLine($"      - {exp.InnerException.StackTrace}");
                }
                throw;
            }
        }
        [HttpGet]
        public HttpResponseMessage ReceiveCommand([FromUri]string deviceId)
        {
            try
            {
                var fn = HttpContext.Current.Server.MapPath($"~/App_Data/{deviceId}.txt");
                if (File.Exists(fn))
                {
                    var text = File.ReadAllText(fn);
                    Trace.WriteLine($"{fn}::{text}");
                    File.Delete(fn);
                    return Request.CreateResponse<string>(HttpStatusCode.OK, text);
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine($"Receive File {fn}...Not exists");
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch(Exception exp)
            {
                System.Diagnostics.Trace.WriteLine($"[ReceiveCommand]{exp.Message}");
                System.Diagnostics.Trace.WriteLine($"Stack Trace: {exp.StackTrace}");
                if (exp.InnerException != null)
                {
                    System.Diagnostics.Trace.WriteLine($"      - {exp.InnerException.Message}");
                    System.Diagnostics.Trace.WriteLine($"      - {exp.InnerException.StackTrace}");
                }
                throw;
            }
        }
    }
}
