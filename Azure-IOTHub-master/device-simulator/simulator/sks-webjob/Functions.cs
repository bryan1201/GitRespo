using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.NotificationHubs;
using Microsoft.ServiceBus.Messaging;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml;
using Newtonsoft.Json;

namespace sks_webjob
{
    public class Functions
    {
        private static string nhConnectionString = "Endpoint=sb://skshub.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=+toF6XlnEcBSPlDTN0WPW2XBNmpIEQnZbQUTfO2y/aY=";
        private static string hubName = "sks-notification";
        private static string sbQueueAdminConnectinString = "Endpoint=sb://michiazurecontw.servicebus.windows.net/;SharedAccessKeyName=admin;SharedAccessKey=nY6VrI4Qb8/dNCi6GNpVa1ncLa8ZjcqYakucyaqaMyk=";
        private static string sbQueueName = "sksdemo";
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("queue")] string message, TextWriter log)
        {
            log.WriteLine(message);
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
            
            
        }
        public static void LookupNotification()
        {
            QueueClient qc = QueueClient.CreateFromConnectionString(
                sbQueueAdminConnectinString,
                sbQueueName);
            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            // Callback to handle received messages.
            qc.OnMessage((message) =>
            {
                try
                {
                    // Process message from event processor host.
                    if (message.Properties.Keys.Contains("message-source") && (string)message.Properties["message-source"] == "evh")
                    {
                        var o = message.GetBody<System.IO.Stream>();
                        using (var r = new StreamReader(o))
                        {
                            var msg = r.ReadToEnd();
                            Console.WriteLine("Body: " + msg);
                            Console.WriteLine("MessageID: " + message.MessageId);
                            SendNotificationAsync(msg);
                            // Remove message from queue.
                            message.Complete();
                        }

                    }
                    else
                    {
                        // Process message from stream analytics.
                        var msg = message.GetBody<string>();
                        
                            Console.WriteLine("Body: " + msg);
                            Console.WriteLine("MessageID: " + message.MessageId);
                            SendNotificationAsync(msg);
                            // Remove message from queue.
                            message.Complete();
                    }
                }
                catch (Exception exp)
                {
                    // Indicates a problem, unlock message in queue.
                    Console.WriteLine("EXCEPTION:" + exp.Message);
                    if(exp.InnerException != null)
                    {
                        Console.WriteLine("INNER:" + exp.Message);
                    }
                    if(exp.StackTrace != null)
                    {
                        Console.WriteLine($"Stack:{exp.StackTrace}");
                    }
                    message.Abandon();
                    //message.Complete();
                }
            }, options);
        }
    }
}
