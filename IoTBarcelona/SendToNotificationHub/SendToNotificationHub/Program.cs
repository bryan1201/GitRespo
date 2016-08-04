using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// **********************************************
using System.Configuration;
using Microsoft.ServiceBus.Notifications;
using Newtonsoft.Json;
// **********************************************

namespace SendToNotificationHub
{
    class Program
    {
        private static async void SendNotificationAsync()
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            string notificationHubPath = ConfigurationManager.AppSettings["Microsoft.ServiceBus.NotificationHubPath"];

            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, notificationHubPath);
            //var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">Hello from a .NET App!</text></binding></visual></toast>";
            //await hub.SendWindowsNativeNotificationAsync(toast);
            await hub.SendGcmNativeNotificationAsync("{ \"data\" : { \"message\" : \"Hello from Windows Azure!\" } }");
        }

        static void Main(string[] args)
        {
            SendNotificationAsync();
            Console.ReadLine();
        }
    }
}
