using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Radios;
using Windows.Storage;

namespace SmartShopping.PhoneApp
{
    class BTWatcherManager
    {
        public delegate void TASKEVENT_HANLDER(string message);

        private IBackgroundTaskRegistration taskRegistration;
        // A name is given to the task in order for it to be identifiable across context.
        public const string taskName = "BTWatcherTask";
        // Entry point for the background task.
        private const string taskEntryPoint = "SmartShopping.PhoneApp.BGTask.BTWatcherTask";

        private TASKEVENT_HANLDER onEventMessageReceivedHandler = null;

        private ushort companyId = 0xFFFF;
        private string beaconPrefix = "";

        public const string BEACONPREFIX_TAG = "BeaconPrefix";

        public BTWatcherManager(ushort compId, string prefix)
        {
            companyId = compId;
            beaconPrefix = prefix;
        }

        public void Init(ushort compId, string prefix, TASKEVENT_HANLDER eventMsgRecvHandler)
        {
            companyId = compId;
            beaconPrefix = prefix;
            onEventMessageReceivedHandler = eventMsgRecvHandler;
        }

        public void EnsureState(bool shouldStart)
        {
            if (taskRegistration == null)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        taskRegistration = task.Value;
                        Debug.WriteLine("Task found: " + task.Value.Name + " / " + task.Value.TaskId);
                        MainPage.Current.UpdateDebugMessage(this, "Task found: " + task.Value.Name + " / " + task.Value.TaskId);
                        break;
                    }
                }
            }
            if (shouldStart)
            {
                if (taskRegistration == null)
                {
                    var ignored = EnableBT(true);
                    RegisterBTWatcherTask();
                }
                else
                {
                    if (App.previousExecState == Windows.ApplicationModel.Activation.ApplicationExecutionState.NotRunning)
                    {
                        taskRegistration.Unregister(true);
                        taskRegistration = null;
                        var ignored = EnableBT(true);
                        RegisterBTWatcherTask();
                    }
                    else
                    {
                        taskRegistration.Completed += OnBTWatcherTaskCompleted;
                    }
                }
            }
            else
            {
                if (taskRegistration != null)
                {
                    UnregisterBTWatcherTask();
                }
            }
        }

        private BluetoothLEAdvertisementWatcherTrigger SetupTrigger()
        {
            // The watcher trigger used to configure the background task registration
            BluetoothLEAdvertisementWatcherTrigger trigger;

            // Create and initialize a new trigger to configure it.
            trigger = new BluetoothLEAdvertisementWatcherTrigger();

            // We need to add some payload to the advertisement. A publisher without any payload
            // or with invalid ones cannot be started. We only need to configure the payload once
            // for any publisher.

            // Add a manufacturer-specific section:
            // First, create a manufacturer data section
            var manufacturerData = new BluetoothLEManufacturerData();

            // Then, set the company ID for the manufacturer data. Here we picked an unused value: 0xFFFE
            manufacturerData.CompanyId = companyId;

            // Add the manufacturer data to the advertisement publisher:
            trigger.AdvertisementFilter.Advertisement.ManufacturerData.Add(manufacturerData);

            trigger.SignalStrengthFilter.InRangeThresholdInDBm = App.curScenario.BTFilterInRange; // -50; // -60;
            trigger.SignalStrengthFilter.OutOfRangeThresholdInDBm = App.curScenario.BTFilterOutRange;  // -100;
            double timeout = App.curScenario.BTSamplingIntervalms * 2.5;
            if (timeout < App.configManager.BTMinOutRangeTimeoutms) timeout = App.configManager.BTMinOutRangeTimeoutms;
            trigger.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(timeout);
            trigger.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(App.curScenario.BTSamplingIntervalms);

            // Display the information about the published payload
            Debug.WriteLine(string.Format("Watch BLE payload information: CompanyId=0x{0}", manufacturerData.CompanyId.ToString("X")));

            return trigger;
        }

        private void RegisterBTWatcherTask()
        {
            // Registering a background trigger if it is not already registered. It will start background watcher.
            // First get the existing tasks to see if we already registered for it
            if (taskRegistration == null)
            {
                ApplicationData.Current.LocalSettings.Values[taskName] = ""; //reset message
                ApplicationData.Current.LocalSettings.Values[BEACONPREFIX_TAG] = beaconPrefix; //reset message

                // At this point we assume we haven't found any existing tasks matching the one we want to register
                // First, configure the task entry point, trigger and name
                BluetoothLEAdvertisementWatcherTrigger trigger = SetupTrigger();

                var builder = new BackgroundTaskBuilder();
                builder.TaskEntryPoint = taskEntryPoint;
                builder.SetTrigger(trigger);
                builder.Name = taskName;

                // Now perform the registration.
                taskRegistration = builder.Register();

                // For this scenario, attach an event handler to display the result processed from the background task
                taskRegistration.Completed += OnBTWatcherTaskCompleted;

                MainPage.Current.UpdateDebugMessage(this, "RegisterBTWatcherTask: " + taskRegistration.Name + " / " + taskRegistration.TaskId);
            }
        }
        private void UnregisterBTWatcherTask()
        {
            if (taskRegistration != null)
            {
                Debug.WriteLine("UnregisterBTWatcherTask: unregister inapp " + taskRegistration.Name + " / " + taskRegistration.TaskId);
                MainPage.Current.UpdateDebugMessage(this, "UnregisterBTWatcherTask: unregister inapp " + taskRegistration.Name + " / " + taskRegistration.TaskId);
                taskRegistration.Unregister(true);
                taskRegistration = null;
            }
            else
            {
                // At this point we assume we haven't found any existing tasks matching the one we want to unregister
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        Debug.WriteLine("UnregisterBTWatcherTask: unregister " + task.Value.Name + " / " + task.Value.TaskId);
                        MainPage.Current.UpdateDebugMessage(this, "UnregisterBTWatcherTask: unregister " + task.Value.Name + " / " + task.Value.TaskId);
                        task.Value.Unregister(true);
                        break;
                    }
                }
            }
        }

        public bool StartBTWatcher()
        {
            UnregisterBTWatcherTask();

            var ignored = EnableBT(true);
            RegisterBTWatcherTask();
            return true;
        }

        public void StopBTWatcher()
        {
            UnregisterBTWatcherTask();
            var ignored = EnableBT(false);
        }

        /// <summary>
        /// Handle background task completion.
        /// </summary>
        /// <param name="task">The task that is reporting completion.</param>
        /// <param name="eventArgs">Arguments of the completion report.</param>
        private void OnBTWatcherTaskCompleted(BackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs eventArgs)
        {
            // We get the status changed processed by the background task
            string msg = ApplicationData.Current.LocalSettings.Values[taskName] as string;
            if (msg != null && msg.Trim().Length > 0)
            {
                string backgroundMessage = "";
                try
                {
                    string[] events = msg.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (events != null)
                    {
                        foreach (var singleevent in events)
                        {
                            try
                            {
                                string[] fields = singleevent.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                string beaconId = fields[0];
                                short signalStrength = 0;
                                short.TryParse(fields[1], out signalStrength);
                                string timestamp = fields[2];
                                backgroundMessage += string.Format("[{0}] {1}dBm, '{2}'\n",
                                                            timestamp, signalStrength.ToString(), beaconId);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("OnBTWatcherTaskCompleted: " + ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("OnBTWatcherTaskCompleted: " + ex.Message);
                }

                Debug.WriteLine("OnBTWatcherTaskCompleted: msg=" + backgroundMessage);

                if (backgroundMessage.Length > 0 && onEventMessageReceivedHandler != null)
                {
                    // Serialize UI update to the main UI thread
                    var ignore = Task.Run(() =>
                    {
                        onEventMessageReceivedHandler?.Invoke(backgroundMessage);
                    });
                }
            }
        }

        private static RadioState LastSetBTState = RadioState.Unknown;

        private static async Task<bool> EnableBT(bool enable)
        {
            bool isBluetoothOn = false;
            if (!enable && LastSetBTState == RadioState.Unknown) return isBluetoothOn;

            try
            {
                var radios = await Radio.GetRadiosAsync();
                if (radios.Count > 0)
                {
                    foreach (var radio in radios)
                    {
                        if (radio.Kind == RadioKind.Bluetooth)
                        {
                            Debug.WriteLine("[EnableBT] bluetooth state: " + radio.State.ToString());
                            if (enable)
                            {
                                RadioState changeToState = RadioState.Unknown;
                                if (radio.State == RadioState.Off)
                                {
                                    var status = await radio.SetStateAsync(RadioState.On);

                                    if (status == RadioAccessStatus.Allowed)
                                    {
                                        changeToState = RadioState.On;
                                    }

                                    Debug.WriteLine("[EnableBT] bluetooth turned on! " + radio.State.ToString());
                                    if (radio.State == RadioState.On)
                                        isBluetoothOn = true;
                                }
                                else
                                    isBluetoothOn = true;
                                LastSetBTState = changeToState;
                            }
                            else
                            {
                                if (LastSetBTState == RadioState.On && radio.State == RadioState.On)
                                {
                                    var status = await radio.SetStateAsync(RadioState.Off);
                                    Debug.WriteLine("[EnableBT] bluetooth turned on! " + radio.State.ToString());
                                    if (radio.State == RadioState.On)
                                        isBluetoothOn = true;
                                }
                                LastSetBTState = RadioState.Unknown;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[EnableBT] exception: " + ex.Message);
            }
            return isBluetoothOn;
        }
    }
}
