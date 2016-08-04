using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;

// Install NuGet packages for being as IotHub device: 
//  1. Microsoft.Azure.Devices.Client  (required to select "Include prerelease" option)
//  2. Newtonsoft.Json

// promotion content is hard coded in this class

namespace SmartShopping.PhoneApp.BGTask
{
    // A background task to listen to cloud to device message, a
    // and then prompt a toast with promotion message (a notification in phone action center)
    public sealed class MessageListenerTask : IBackgroundTask
    {
        private const string CURRENTSCENARIO_EMUAZURE_TAG = "EmuAzure";
        private const string CONTENTID_TAG = "ContentId";
        private const string BEACONPREFIX_TAG = "BeaconPrefix";


        private IBackgroundTaskInstance _taskInstance = null;
        private BackgroundTaskDeferral _deferral = null;
        RemoteMessageReceiver _receiver = null;

        /// <summary>
        /// The entry point of a background task.
        /// </summary>
        /// <param name="taskInstance">The current background task instance.</param>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // In this example, the background task simply constructs a message communicated
            // to the App. For more interesting applications, a notification can be sent from here instead.
            _taskInstance = taskInstance;
            _taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            _deferral = taskInstance.GetDeferral();

            // Store the message in a local settings indexed by this task's name so that the foreground App
            // can display this message.
            ApplicationData.Current.LocalSettings.Values[_taskInstance.Task.Name] = "started";

            string contentId = ApplicationData.Current.LocalSettings.Values[CONTENTID_TAG] as string;

            _receiver = new RemoteMessageReceiver(contentId);
            _receiver._taskInstance = _taskInstance;

            Debug.WriteLine("MessageListenerTask: Run()");

            bool emuAzure = false;
            byte emuAzureFlag = 0;
            var settings = ApplicationData.Current.LocalSettings;
            byte.TryParse(settings.Values[CURRENTSCENARIO_EMUAZURE_TAG] as string, out emuAzureFlag);

            if (emuAzureFlag != 0) emuAzure = true;
            if (!emuAzure)
            {
                // create a task to listen for messages
                var ignored = Task.Run(async () =>
                {
                    Debug.WriteLine("MessageListenerTask: Start...");
                    await _receiver.Listen();
                    ApplicationData.Current.LocalSettings.Values[_taskInstance.Task.Name] = "stopped";
                    Debug.WriteLine("MessageListenerTask: Stopped!");
                    _deferral.Complete();
                });
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[_taskInstance.Task.Name] = "stopped";
                Debug.WriteLine("MessageListenerTask: Stopped!");
                _deferral.Complete();
            }
        }   

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _receiver.Stop();
        }

    }

    class RemoteMessageReceiver
    {
        private const string DEVICEID_TAG = "DeviceId";
        private const string IOTHUBRECVCONN_TAG = "IotHubConn";
        private const string NOTIFICATION_TAG = ".Message";

        private const string CURRENTINRANGE_TAG = "BTCurrentInRange";
        private const string BTWATCHERTASK_TAG = "BTWatcherTask";

        private const string CURRENTSCENARIO_EMUNOTIFICATIONINTERVALS_TAG = "EmuAzureNotifIntervals";

        private const string BEACONPREFIX_TAG = "BeaconPrefix";

        private volatile bool isCanceled = false;
        private volatile bool isPaused = false;

        public IBackgroundTaskInstance _taskInstance = null;

        public RemoteMessageReceiver(string contentId)
        {
            if (contentId == null)
                promotionList = promotionList_Default;
            //else if (contentId == "Demo2")
            //    promotionList = promotionList_Demo2;
            else
                promotionList = promotionList_Default;
        }

        public bool OneshotProcess(short? filterInRange, short? filterOutRange)
        {
            Debug.WriteLine("MessageListenerTask: OneshotProcess()");

            bool isSuccess = false;

            var localSettings = ApplicationData.Current.LocalSettings;
            string eventMessages = localSettings.Values[BTWATCHERTASK_TAG] as string;
            string inrangeList = localSettings.Values[CURRENTINRANGE_TAG] as string;
            string beaconPrefix = localSettings.Values[BEACONPREFIX_TAG] as string;
            List<string> inrangeDevices = null;
            string productId = null;
            string dbgmsg = null;
            try
            {
                short inrangeValue = ((filterInRange != null)? filterInRange.Value : (short)-127);
                short outrangeValue = ((filterOutRange != null)? filterOutRange.Value : (short)-127);

                if (inrangeList == null)
                    inrangeDevices = new List<string>();
                else
                    inrangeDevices = new List<string>(inrangeList.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

                string[] events = eventMessages.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var singleevent in events)
                {
                    if (singleevent.Length == 0) continue;
                    try
                    {
                        string[] fields = singleevent.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (fields == null || fields.Length < 3) continue;

                        string beaconId = fields[0];
                        short signalStrength = 0;
                        short.TryParse(fields[1], out signalStrength);

                        //if (beaconId.Substring(0, beaconPrefix.Length) != beaconPrefix)
                        //    continue;

                        if (inrangeDevices.Contains(beaconId))
                        {
                            if (signalStrength <= outrangeValue)
                            {
                                inrangeDevices.Remove(beaconId);
                                //dbgmsg += ";-" + beaconId + "("+ signalStrength + ")";
                            }
                        }
                        else
                        {
                            if (signalStrength > inrangeValue)
                            {
                                if (productId == null)
                                {
                                    productId = "Product" + beaconId.Substring(beaconPrefix.Length, 2);
                                    inrangeDevices.Add(beaconId);
                                    //dbgmsg += ";+" + beaconId;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

                inrangeList = "";
                foreach (var device in inrangeDevices)
                {
                    inrangeList += device + " ";
                }
                localSettings.Values[CURRENTINRANGE_TAG] = inrangeList;
                //dbgmsg += ";" + inrangeDevices.Count + ")" + inrangeList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            if (productId == null) return true;

            // To prevent exception happened when network is disconnected abnormally
            try
            {
                HotZoneInfo zoneInfo = null;
                try
                {
                    zoneInfo = promotionList[productId];
                }
                catch
                { }

                if (zoneInfo == null)
                {
                    zoneInfo = promotionList["default"];
                }

                if (_taskInstance != null)
                {
                    // string msg = "[" + _taskInstance.Progress + "] " + ((messageData == null) ? "" : messageData);
                    string msg = zoneInfo.ZoneDesc
                    + "\n" + _taskInstance.Progress + " " + productId + " " + DateTime.Now.ToLocalTime();
                    ApplicationData.Current.LocalSettings.Values[_taskInstance.Task.Name + NOTIFICATION_TAG] = msg;

                    Debug.WriteLine("MessageListenerTask Got Message: " + msg);

                    _taskInstance.Progress = 100;
                }

                ToastPromotionInfo(productId, dbgmsg);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MessageListenerTask: " + ex.Message);
            }
            return isSuccess;
        }

        public class ReceivedData
        {
            public string ProductId { get; set; }
            public DateTime SendTimestamp { get; set; }
        }

        public async Task<bool> Listen()
        {
            Debug.WriteLine("MessageListenerTask: Listen()");

            bool isSuccess = false;

            isCanceled = false;

            var localSettings = ApplicationData.Current.LocalSettings;
            string deviceId = localSettings.Values[DEVICEID_TAG] as string;
            string receiverConnectionString = localSettings.Values[IOTHUBRECVCONN_TAG] as string;

            DeviceClient deviceClientReceiver = null;

            try
            {
                // https://github.com/Azure/azure-iot-sdks/blob/master/csharp/device/Microsoft.Azure.Devices.Client/DeviceClient.cs
                deviceClientReceiver = DeviceClient.CreateFromConnectionString(
                    receiverConnectionString,
                    deviceId,
                    TransportType.Amqp_WebSocket_Only);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MessageListenerTask.Listen Exception: " + ex.Message);
                isCanceled = true;
            }

            _taskInstance.Progress = 0;

            while (true)
            {
                if (isCanceled)
                    break;

                CheckStopCondition();
                CheckPauseCondition();

                if (isPaused)
                {
                    await Task.Delay(500);
                    continue;
                }
                // To prevent exception happened when network is disconnected abnormally
                try
                {
                    Message receivedMessage = await deviceClientReceiver.ReceiveAsync(); // ReceiveAsync(new TimeSpan(0, 0, 30)); // 1 second

                    if (receivedMessage == null)
                    {
                        await Task.Delay(1000);
                        continue;
                    }

                    string messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    
                    var resultData = JsonConvert.DeserializeObject<ReceivedData>(messageData);

                    await deviceClientReceiver.CompleteAsync(receivedMessage);

                    HotZoneInfo zoneInfo = null;
                    try
                    {
                        zoneInfo = promotionList[resultData.ProductId];
                    }
                    catch
                    { }

                    if (zoneInfo == null)
                    {
                        zoneInfo = promotionList["default"];
                    }

                    string msg = zoneInfo.ZoneDesc
                        + "\n" + _taskInstance.Progress + " " + resultData.ProductId + " " + resultData.SendTimestamp.ToLocalTime();
                    ApplicationData.Current.LocalSettings.Values[_taskInstance.Task.Name + NOTIFICATION_TAG] = msg;

                    Debug.WriteLine("MessageListenerTask Got Message: " + msg);

                    uint progressVal = (_taskInstance.Progress + 1) % 100;
                    if (progressVal == 0) progressVal = 1;
                    _taskInstance.Progress = progressVal;

                    ToastPromotionInfo(resultData.ProductId, null);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("MessageListenerTask: " + ex.Message);
                    await Task.Delay(1000);
                }
            }
            isCanceled = false;
            return isSuccess;
        }

        public void Stop()
        {
            isCanceled = true;
        }

        public void CheckStopCondition()
        {
            string shouldCancel = ApplicationData.Current.LocalSettings.Values[_taskInstance.Task.Name + ".Cancel"] as string;
            if (shouldCancel != null && shouldCancel == "1")
            {
                Stop();
                ApplicationData.Current.LocalSettings.Values[_taskInstance.Task.Name + ".Cancel"] = null;
            }
        }
        public void CheckPauseCondition()
        {
            string state = ApplicationData.Current.LocalSettings.Values["BeaconTask.State"] as string;
            if (state != null)
            {
                bool shouldPause = false;
                if (state == "started")
                    shouldPause = false;
                else
                    shouldPause = true;
                if (shouldPause != isPaused)
                {
                    Debug.WriteLine("MessageListenerTask: " + (shouldPause? "Pause" : "Resume"));
                }
                isPaused = shouldPause;
            }
        }

        private class HotZoneInfo
        {
            public string ZoneDesc;
            public List<string[]> PromotionUrls;
        }

        private Dictionary<string, HotZoneInfo> promotionList_Default = new Dictionary<string, HotZoneInfo>
        {
            {
                "default",
                new HotZoneInfo
                {
                    ZoneDesc = "You are here at Zone - Microsoft",
                    PromotionUrls = new List<string[]>
                    {
                        new string[] { "Today's promotion : Windows Phone", "ms-appx-web:///Content/promote.html?pcode=01-000&amp;txt=Microsoft" },
                    }
                }
            },
            {
                "Product01",
                new HotZoneInfo
                {
                    ZoneDesc = "Approaching Zone 1 - HoloLens",
                    PromotionUrls = new List<string[]>
                    {
                        new string[] { "Promotion : HoloLens", "ms-appx-web:///Content/promote.html?pcode=06-000&amp;txt=HoloLens" },
                    }
                }
            },
            {
                "Product02",
                new HotZoneInfo
                {
                    ZoneDesc = "Approaching Zone 2 - SurfaceHub",
                    PromotionUrls = new List<string[]>
                    {
                        new string[] { "Promotion : Surface Hub", "ms-appx-web:///Content/promote.html?pcode=04-000&amp;txt=Surface Hub" },
                    }
                }
            },
            {
                "Product03",
                new HotZoneInfo
                {
                    ZoneDesc = "Approaching Zone 3 - SurfacePro4",
                    PromotionUrls = new List<string[]>
                    {
                        new string[] { "Promotion : SurfacePro4", "ms-appx-web:///Content/promote.html?pcode=02-000&amp;txt=Surface Pro4" },
                    }
                }
            },
            {
                "Product04",
                new HotZoneInfo
                {
                    ZoneDesc = "Approaching Zone 4 - WindowsPhone",
                    PromotionUrls = new List<string[]>
                    {
                        new string[] { "Promotion : Windows Phone", "ms-appx-web:///Content/promote.html?pcode=01-000&amp;txt=Windows Phone" },
                    }
                }
            },
            {
                "Product05",
                new HotZoneInfo
                {
                    ZoneDesc = "Approaching Zone 5 - SurfaceBook",
                    PromotionUrls = new List<string[]>
                    {
                        new string[] { "Promotion : Surface Book", "ms-appx-web:///Content/promote.html?pcode=05-000&amp;txt=Surface Book" },
                    }
                }
            },
            {
                "Product06",
                new HotZoneInfo
                {
                    ZoneDesc = "Approaching Zone 6 - Xbox",
                    PromotionUrls = new List<string[]>
                    {
                        new string[] { "Promotion : Xbox", "ms-appx-web:///Content/promote.html?pcode=03-000&amp;txt=Xbox" },
                    }
                }
            },
        };

        private Dictionary<string, HotZoneInfo> promotionList = null;
        private string lastPromotionUrl = null;

        private void ToastPromotionInfo(string promotionKey, string dbgmsg)
        {
            string appTitle = Windows.ApplicationModel.Package.Current.DisplayName;
            HotZoneInfo zoneInfo = null;

            try
            {
                zoneInfo = promotionList[promotionKey];
            }
            catch (Exception)
            { }

            if (zoneInfo == null)
            {
                zoneInfo = promotionList["default"];
            }

            var promotionUrls = zoneInfo.PromotionUrls;
            var promotionUrl = promotionUrls[0];

            if (lastPromotionUrl != null)
            {
                foreach (var purl in promotionUrls)
                {
                    if (purl[1].CompareTo(lastPromotionUrl) != 0)
                    {
                        promotionUrl = purl;
                        break;
                    }
                }
            }

            string promotionMsg = promotionUrl[0];
            string url = promotionUrl[1];

            lastPromotionUrl = url;

            try
            {
                string xmlPayload =
                "<toast duration=\"long\" launch=\"" + url + "\">" + "\n"
                + "<visual>" + "\n"
                + "<binding template=\"ToastGeneric\">" + "\n"
                + "<text>" + appTitle + "</text>" + "\n"
                + "<text>" + zoneInfo.ZoneDesc + "</text>" + "\n"
                + ((dbgmsg == null) ?
                ("<text>" + promotionMsg + "</text>" + "\n")
                : ("<text>" + dbgmsg + ";" + promotionMsg + "</text>" + "\n"))
                + "<image placement=\"appLogoOverride\" src=\"Assets/product-logo.png\" />" + "\n"
                + "</binding>" + "\n"
                + "</visual>" + "\n"
                //+ "<actions>" + "\n"
                //+ "<action activationType=\"foreground\" content=\"View Promotion\" arguments=\"" + url + "\" />" + "\n"
                //+ "</actions>" + "\n"
                + "</toast>" + "\n";

                // Create an XML document from the XML.
                var toastDOM = new Windows.Data.Xml.Dom.XmlDocument();
                toastDOM.LoadXml(xmlPayload);

                // Prepare to raise the toast.
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();

                // Raise the toast immediately.
                var toast = new ToastNotification(toastDOM);
                toastNotifier.Show(toast);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Toast: " + ex.Message);
            }
        }
    }
}
