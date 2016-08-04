using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.Background;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SmartShopping.PhoneApp.BGTask
{
    // A background task to watch BLE Advertisements / signals
    public sealed class BTWatcherTask : IBackgroundTask
    {
        private const string massageListenerTaskName = "BTWatcherTask";
        private const string CURRENTSCENARIO_EMUAZURE_TAG = "EmuAzure";
        private const string CONTENTID_TAG = "ContentId";
        private const string BEACONPREFIX_TAG = "BeaconPrefix";
        private IBackgroundTaskInstance backgroundTaskInstance = null;
        private BackgroundTaskDeferral _deferral = null;

        /// <summary>
        /// The entry point of a background task.
        /// </summary>
        /// <param name="taskInstance">The current background task instance.</param>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("[WatcherTask] " + DateTime.Now.ToString("hh\\:mm\\:ss\\.fff"));

            backgroundTaskInstance = taskInstance;
            _deferral = taskInstance.GetDeferral();

            var details = taskInstance.TriggerDetails as BluetoothLEAdvertisementWatcherTriggerDetails;

            if (details != null)
            {
                // If the background watcher stopped unexpectedly, an error will be available here.
                var error = details.Error;

                // The Advertisements property is a list of all advertisement events received
                // since the last task triggered. The list of advertisements here might be valid even if
                // the Error status is not Success since advertisements are stored until this task is triggered
                IReadOnlyList<BluetoothLEAdvertisementReceivedEventArgs> advertisements = details.Advertisements;

                // The signal strength filter configuration of the trigger is returned such that further 
                // processing can be performed here using these values if necessary. They are read-only here.
                var rssiFilter = details.SignalStrengthFilter;
                string statusMsg = DateTime.Now.ToString("hh\\:mm\\:ss\\.fff") + " " 
                                    + string.Format("ErrorStatus: {0}, EventCount: {1}, HighDBm: {2}, LowDBm: {3}, Timeout: {4}, Sampling: {5}",
                                    error.ToString(),
                                    advertisements.Count.ToString(),
                                    rssiFilter.InRangeThresholdInDBm.ToString(),
                                    rssiFilter.OutOfRangeThresholdInDBm.ToString(),
                                    rssiFilter.OutOfRangeTimeout.GetValueOrDefault().TotalMilliseconds.ToString(),
                                    rssiFilter.SamplingInterval.GetValueOrDefault().TotalMilliseconds.ToString());
                Debug.WriteLine("---- BTWatcherTask: " + statusMsg);

                // In this example, the background task simply constructs a message communicated
                // to the App. For more interesting applications, a notification can be sent from here instead.
                string eventMessage = "";
                Dictionary<string, BluetoothLEAdvertisementReceivedEventArgs> eventList = new Dictionary<string, BluetoothLEAdvertisementReceivedEventArgs>(advertisements.Count);

                string beaconPrefix = ApplicationData.Current.LocalSettings.Values[BEACONPREFIX_TAG] as string;

                // Advertisements can contain multiple events that were aggregated, each represented by 
                // a BluetoothLEAdvertisementReceivedEventArgs object.
                foreach (var eventArgs in advertisements)
                {
                    // Check if there are any manufacturer-specific sections.
                    // If there is, print the raw data of the first manufacturer section (if there are multiple).
                    string manufacturerDataString = "";
                    var manufacturerSections = eventArgs.Advertisement.ManufacturerData;
                    if (manufacturerSections.Count > 0)
                    {
                        var manufacturerData = manufacturerSections[0];
                        var data = new byte[manufacturerData.Data.Length];
                        using (var reader = DataReader.FromBuffer(manufacturerData.Data))
                        {
                            reader.ReadBytes(data);
                        }
                        manufacturerDataString = System.Text.Encoding.UTF8.GetString(data);

                        if (beaconPrefix != null && beaconPrefix.Length > 0)
                        {
                            // case sensitive comparison
                            if (manufacturerDataString.Substring(0, beaconPrefix.Length) != beaconPrefix)
                                continue;
                        }
                    }

                    // workaround: filtering abnormal -127 events
                    if (!eventList.ContainsKey(manufacturerDataString))
                        eventList.Add(manufacturerDataString, eventArgs);
                    else if (eventArgs.RawSignalStrengthInDBm > -127)
                    {
                        eventList[manufacturerDataString] = eventArgs;
                    }
                }

                foreach (var btevent in eventList)
                {
                    eventMessage += string.Format("{0} {1} {2}\n",
                                        btevent.Key,
                                        btevent.Value.RawSignalStrengthInDBm.ToString(),
                                        btevent.Value.Timestamp.ToString("hh\\:mm\\:ss\\.fff"));
                }

                // Store the message in a local settings indexed by this task's name so that the foreground App
                // can display this message.

                ApplicationData.Current.LocalSettings.Values[taskInstance.Task.Name] = eventMessage.Trim();

                Debug.WriteLine(eventMessage);

                bool emuAzure = false;
                byte emuAzureFlag = 0;
                byte.TryParse(ApplicationData.Current.LocalSettings.Values[CURRENTSCENARIO_EMUAZURE_TAG] as string, out emuAzureFlag);
                if (emuAzureFlag != 0) emuAzure = true;

                if (!emuAzure)
                {
                    // initial a separate task to send beacon message to cloud
                    var ignored = Task.Run(async () =>
                    {
                        // send message to IoT Hub, no retry...
                        IoTHubMessageSender iotHubMsgSender = new IoTHubMessageSender();
                        try
                        {
                            if (iotHubMsgSender.Init())
                            {
                                foreach (var eventArgs in advertisements)
                                {
                                    var manufacturerSections = eventArgs.Advertisement.ManufacturerData;
                                    if (manufacturerSections.Count > 0)
                                    {
                                        var defaultDataSection = manufacturerSections[0];
                                        var data = new byte[defaultDataSection.Data.Length];
                                        using (var reader = DataReader.FromBuffer(defaultDataSection.Data))
                                        {
                                            reader.ReadBytes(data);
                                        }
                                        string senderDeviceId = System.Text.Encoding.UTF8.GetString(data);
                                        await iotHubMsgSender.sendDataAsync(senderDeviceId, (int)eventArgs.RawSignalStrengthInDBm, eventArgs.Timestamp.UtcDateTime);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("BTWatcherTask.senderLoop Exception: " + ex.Message);
                        }
                        _deferral.Complete();
                    });
                }
                else
                {
                    // simulate a cloud to device message if "emuAzure" enabled
                    if (details != null && details.Error == Windows.Devices.Bluetooth.BluetoothError.Success)
                    {
                        if (eventMessage.Trim().Length > 0)
                        {
                            string contentId = ApplicationData.Current.LocalSettings.Values[CONTENTID_TAG] as string;

                            RemoteMessageReceiver _receiver = new RemoteMessageReceiver(contentId);
                            _receiver.OneshotProcess(rssiFilter.InRangeThresholdInDBm, rssiFilter.OutOfRangeThresholdInDBm);
                        }
                    }
                    _deferral.Complete();
                }
            }
            else
            {
                _deferral.Complete();
            }
        }
    }

    // the utility class to manage sending message to IoT hub.
    // this UWP app will act as a IoT device to send/recieve messages
    // hence device identity should be assigned per UWP app installation
    // in this sample, device identity is acquired from backend service, and saved in app settings
    
    class IoTHubMessageSender
    {
        private const string DEVICEID_TAG = "DeviceId";
        private const string IOTHUBRECVCONN_TAG = "IotHubConn";
        private const string IOTHUBKEY_TAG = "IotHubKey";

        public IBackgroundTaskInstance _taskInstance = null;

        private DeviceClient deviceClient = null;
        private string deviceId = null;

        public class SendData
        {
            public string BeaconId { get; set; }
            public string TargetDeviceId { get; set; }
            public int SignalStrength { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public bool Init()
        {
            if (deviceClient == null)
            {
                try
                {
                    var localSettings = ApplicationData.Current.LocalSettings;
                    deviceId = localSettings.Values[DEVICEID_TAG] as string;
                    string deviceKey = localSettings.Values[IOTHUBKEY_TAG] as string;
                    string receiverConnectionString = localSettings.Values[IOTHUBRECVCONN_TAG] as string;
                    //char[] separators = { '=', ';' };
                    //string[] receiverArgs = receiverConnectionString.Split(separators);
                    //string iotHubUri = (receiverArgs.Length >= 2) ? receiverArgs[1] : null;

                    //deviceClient = DeviceClient.Create(iotHubUri,
                    //               AuthenticationMethodFactory.CreateAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey),
                    //               TransportType.Http1);

                    deviceClient = DeviceClient.CreateFromConnectionString(
                                    receiverConnectionString,
                                    deviceId,
                                    TransportType.Amqp_WebSocket_Only);

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("IoTHubMessageSender.Init: " + ex.Message);
                }
                return false;
            }
            else
                return true;
            
        }

        public async Task<bool> sendDataAsync(string senderDeviceID, int signalStrength, DateTime timeStamp)
        {
            if (!Init()) return false;
            try
            {
                var newData = new SendData();
                newData.TargetDeviceId = deviceId;
                newData.BeaconId = senderDeviceID;
                newData.SignalStrength = signalStrength;
                newData.Timestamp = timeStamp;

                string message = JsonConvert.SerializeObject(newData);
                Message azureMsg = new Message(Encoding.ASCII.GetBytes(message));
                await deviceClient.SendEventAsync(azureMsg);
                Debug.WriteLine("sendDataAsync: " + message);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("IoTHubMessageSender.sendDataAsync: " + ex.Message);
            }
            return false;
        }
    }
}