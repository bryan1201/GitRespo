using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using System.Configuration;

namespace nhsender
{
    class Program
    {

        static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        static void Log(string msg)
        {
            Console.ResetColor();
            Console.WriteLine(msg);
        }
        static void Wait(string msg)
        {
            Console.WriteLine(msg);
            Console.ReadLine();
        }
        static void Main(string[] args)
        {
            string msg = null;
            string[] tags = null;
            switch (args.Length)
            {
                case 2:
                    tags = args[1].Split(new char[] { '|' });
                    goto case 1;
                case 1:
                    msg = args[0];
                    break;
                default:
                    Error("Usage nhsender.exe <message> <tag1|tag2|tag3...>");
                    Wait("Press [ENTER] to exit");
                    break;
            }
            SendNotificationAsync(msg, tags).Wait();

            Wait("Press [ENTER] to exit...");
        }
        private static async Task SendNotificationAsync(string msg, string [] tags)
        {
            if (tags != null)
            {
                Log($"Sending notification [{msg}] to tags {string.Join("|", tags)}");
            }
            else
            {
                Log($"Sending notification [{msg}]");
            }
            //Need connection string of Send permission
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString(
                        ConfigurationManager.AppSettings["noficationHubConnectionString"],
                        ConfigurationManager.AppSettings["notificationHubName"]);
            var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + msg + "</text></binding></visual></toast>";
            NotificationOutcome result = null;

            if (tags != null)
            {
                result = await hub.SendWindowsNativeNotificationAsync(toast, tags);
            }
            else
            {
                result = await hub.SendWindowsNativeNotificationAsync(toast);
            }
            if( result.State    == NotificationOutcomeState.Abandoned ||
                result.State == NotificationOutcomeState.Unknown)
            {
                Error("...Unsuccessful");
            }
            else
            {
                Log("Sent...");
            }
        }
    }
}
