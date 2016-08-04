using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using SmartShoppingDemoProcess.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace SmartShoppingDemoProcess
{
    public class ProcessAdvertisement
    {
        private static string connectionString = String.Empty;
        private static ServiceClient serviceClient = null;
        private static SmartShoppingDemoProcessContext db = new SmartShoppingDemoProcessContext();

        // BeaconId to ProductId and Signal Strength In/Out Filter mapping list: Dictionary<BeaconId, BeaconInfo>
        private static Dictionary<string, BeaconInfo> BeaconInfoList = new Dictionary<string, BeaconInfo>();

        // Target Device send notification List: Dictionary<TargetDeviceId+BeaconId, HadSentNotification>
        private static Dictionary<string, bool> SendNotificationList = new Dictionary<string, bool>();

        private static DateTime LastProcessTime = DateTime.UtcNow;

        private static long LastProcessId = 0;


        public class ReceivedData
        {
            public string BeaconId { get; set; }
            public string TargetDeviceId { get; set; }
            public int SignalStrength { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public class SendData
        {
            public string ProductId { get; set; }
            public DateTime SendTimestamp { get; set; }
        }

        public class BeaconInfo
        {
            public string ProductId { get; set; }
            public int InFilter { get; set; }
            public int OutFilter { get; set; }
        }

        private static void InitializeBeaconInfoList()
        {
            foreach (var beacon in db.Beacons)
            {
                BeaconInfo info = new BeaconInfo();
                info.ProductId = beacon.ProductId;
                info.InFilter = beacon.InFilter.GetValueOrDefault();
                info.OutFilter = beacon.OutFilter.GetValueOrDefault();
                BeaconInfoList.Add(beacon.BeaconId, info);
            }
        }

        public static void StartProcessAdvertisement()
        {
            // Setup BeaconId to ProductId and signal strength in/out filter mapping list
            InitializeBeaconInfoList();

            var appSettings = ConfigurationManager.AppSettings;

            connectionString = appSettings["IothubConnectionString"];

            // Settings for send message
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            if (serviceClient == null) return;

            // Start to receive messages from Azure SQL DB
            Task.Run(async () =>
            {
                await ProcessAdvertisementAsync();
            });
        }

        private async static Task ProcessAdvertisementAsync()
        {
            // Initialize LastProcessId to be the Top 1 Id in [Advertisements] table
            List<long> idList = db.Database.SqlQuery<long>(
            "SELECT TOP 1 [Id] FROM [Advertisements] ORDER BY [Id] DESC;").ToList();

            if (idList.Count > 0)
                LastProcessId = idList[0];

            // Start to process Advertisements
            while (true)
            {
                try
                {
                    // List<Advertisement> advList = db.Database.SqlQuery<Advertisement>(
                    //    "Exec ProcGetAdvertisementListByTime N'" + LastProcessTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "';").ToList();
                    List<Advertisement> advList = db.Database.SqlQuery<Advertisement>(
                        "Exec ProcGetAdvertisementListById N'" + LastProcessId.ToString() + "';").ToList();

                    if (advList.Count <= 0)
                    {
                        Thread.Sleep(1000);  // sleep 1 sec
                        continue;
                    }

                    for (int i = 0; i < advList.Count; i++)
                    {
                        // Update LastProcessId
                        if (advList[i].Id > LastProcessId)
                            LastProcessId = advList[i].Id;

                        // Skip invalid BeaconId
                        if (!BeaconInfoList.ContainsKey(advList[i].BeaconId))
                            continue;

                        // Skip invalid TargetDeviceId
                        if (advList[i].TargetDeviceId.Length < 7 ||
                            advList[i].TargetDeviceId.Substring(0, 6).ToUpper() != "DEVICE")
                        {
                            continue;
                        }

                        // Send received data back to device
                        if (!CheckSignalStrength(advList[i]))
                            continue;

                        var task = Task.Run(async () =>
                        {
                            await SendCloudToDeviceMessageAsync(advList[i].BeaconId, advList[i].TargetDeviceId);
                        });

                        // Insert data into [Notifications] table
                        string queryCommand = "Exec ProcInsertNotification " +
                                              advList[i].Id.ToString() + ";";

                        await db.Database.ExecuteSqlCommandAsync(queryCommand);
                    }  // for

                    // Update LastProcessTime
                    LastProcessTime = advList[advList.Count - 1].Timestamp;

                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    continue;
                }
            }
        }

        private async static Task SendCloudToDeviceMessageAsync(string beacon, string targetDevice)
        {
            if (serviceClient == null || beacon == null)
                return;

            try
            {
                string productId = String.Empty;

                if (BeaconInfoList.ContainsKey(beacon))
                {
                    productId = BeaconInfoList[beacon].ProductId;
                }

                if (String.IsNullOrEmpty(productId))
                    return;

                // Send message to device
                SendData newData = new SendData
                {
                    ProductId = productId,
                    SendTimestamp = DateTime.UtcNow
                };

                var str = JsonConvert.SerializeObject(newData);
                var message = new Message(Encoding.ASCII.GetBytes(str));
                await serviceClient.SendAsync(targetDevice, message);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private static bool CheckSignalStrength(Advertisement adv)
        {
            if (adv.BeaconId == null || !BeaconInfoList.ContainsKey(adv.BeaconId))
                return false;

            int InSignalStrength = BeaconInfoList[adv.BeaconId].InFilter;
            int OutSignalStrength = BeaconInfoList[adv.BeaconId].OutFilter;

            // Add a new TargetDeviceId and BeaconId combination pair if it's a new combination
            if (!SendNotificationList.ContainsKey(adv.TargetDeviceId + adv.BeaconId))
            {
                SendNotificationList.Add(adv.TargetDeviceId + adv.BeaconId, false);
            }

            if (adv.SignalStrength >= InSignalStrength)
            {
                // Change send notification from false to true if its previous state is false
                if (!SendNotificationList[adv.TargetDeviceId + adv.BeaconId])
                {
                    SendNotificationList[adv.TargetDeviceId + adv.BeaconId] = true;
                    return true;
                }
            }

            if (adv.SignalStrength < OutSignalStrength)
            {
                if (SendNotificationList[adv.TargetDeviceId + adv.BeaconId])
                {
                    SendNotificationList[adv.TargetDeviceId + adv.BeaconId] = false;
                }
            }

            return false;
        }

        public static int GetFilter(string beaconId, bool isIn)
        {
            int signalStrength = (isIn) ? BeaconInfoList[beaconId].InFilter : BeaconInfoList[beaconId].OutFilter;

            return signalStrength;
        }

        public static void SetFilter(string beaconId, int filterIn, int filterOut)
        {
            BeaconInfoList[beaconId].InFilter = filterIn;
            BeaconInfoList[beaconId].OutFilter = filterOut;

            return;
        }
    }
}
