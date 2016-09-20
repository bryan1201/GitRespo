using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;

namespace sks_webjob_sdk_version
{
    public class Functions
    {
        private static string nhConnectionString = "Endpoint=sb://skshub.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=+toF6XlnEcBSPlDTN0WPW2XBNmpIEQnZbQUTfO2y/aY=";
        private static string hubName = "sks-notification";
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([ServiceBusTrigger("sksdemo")] string message,
                TextWriter logger)
        {
            logger.WriteLine(message);
            logger.WriteLine($"{message} received at {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")}");
            SendNotificationAsync(message);
        }

        private static async Task SendNotificationAsync(string msg)
        {
            await SendWindowsNotificationAsync(msg);
            await SendAndroidNotificationAsync(msg);
        }
        private static async Task SendWindowsNotificationAsync(string msg)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString(nhConnectionString, hubName);
            var toast = $"<toast launch=\"launch_arguments\"><visual><binding template=\"ToastText01\"><text id=\"1\">{msg}</text></binding></visual></toast>";
            var results = await hub.SendWindowsNativeNotificationAsync(toast);
        }
        private static async Task SendAndroidNotificationAsync(string msg)
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString(nhConnectionString, hubName);
            Newtonsoft.Json.Linq.JObject o = JsonConvert.DeserializeObject(msg) as Newtonsoft.Json.Linq.JObject;
            var toast = "{data:{message:'{device} alert at {time}'}}".Replace("{device}", (string)o["deviceid"]).Replace("{time}", (string)o["time"]);
            var results = await hub.SendGcmNativeNotificationAsync(toast);
            Console.WriteLine($"***{msg}");
        }
    }
}
