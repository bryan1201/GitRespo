using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

//  Install Nuget packages for being as WebApi client:
//  1. Microsoft.AspNet.WebApi.Client
//  2. Newtonsoft.Json

namespace SmartShopping.PhoneApp
{
    class IotHubDevice
    {
        public int DevId { get; set; }

        public string DeviceId { get; set; }

        public string DeviceKey { get; set; }

        public string ConnectionString { get; set; }

        public bool IsUsed { get; set; }

        public bool IsActive { get; set; }

        public DateTime TimeStamp { get; set; }
    }

    class RemoteMessageManager
    {
        public delegate void TASKEVENT_HANLDER(string message);

        private const string RequestUri = "api/DevicesData";

        private const string DEVICEID_TAG = "DeviceId";
        private const string IOTDEVICESERVICEURL_TAG = "IotDeviceServiceUrl";
        private const string IOTHUBRECVCONN_TAG = "IotHubConn";
        private const string IOTHUBKEY_TAG = "IotHubKey";
        private const string NOTIFICATION_TAG = ".Message";
        private const string CURRENTSCENARIO_EMUAZURE_TAG = "EmuAzure";
        private const string CURRENTSCENARIO_EMUNOTIFICATIONINTERVALS_TAG = "EmuAzureNotifIntervals";

        private const string CURRENTINRANGE_TAG = "BTCurrentInRange";

        private const string CONTENTID_TAG = "ContentId";

        private IBackgroundTaskRegistration taskRegistration;
        // A name is given to the task in order for it to be identifiable across context.
        private string taskName = "MessageListenerTask";
        // Entry point for the background task.
        private string taskEntryPoint = "SmartShopping.PhoneApp.BGTask.MessageListenerTask";

        private TASKEVENT_HANLDER onNotificationHandler = null;
        private TASKEVENT_HANLDER onTaskCompletedHandler = null;
        private string ServiceUri = null;
        private string ContentId = null;

        public static void ResetLocalDevice()
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

                localSettings.Values[IOTDEVICESERVICEURL_TAG] = null;
                localSettings.Values[DEVICEID_TAG] = null;
                localSettings.Values[IOTHUBRECVCONN_TAG] = null;
                localSettings.Values[IOTHUBKEY_TAG] = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string PeekDeviceId()
        {
            if (App.curScenario.EmuAzure != 0) return "";

            string deviceId = ApplicationData.Current.LocalSettings.Values[DEVICEID_TAG] as string;
            if (deviceId == null)
            {
                return "";
            }
            return deviceId;
        }


        public async Task<string> GetDeviceId()
        {
            if (App.curScenario.EmuAzure != 0) return "";
            
            var localSettings = ApplicationData.Current.LocalSettings;
            string deviceId = localSettings.Values[DEVICEID_TAG] as string;
            if (deviceId == null)
            {
                IotHubDevice device = await AcquireDeviceId();

                localSettings.Values[IOTDEVICESERVICEURL_TAG] = ServiceUri;
                localSettings.Values[DEVICEID_TAG] = device.DeviceId;
                localSettings.Values[IOTHUBRECVCONN_TAG] = device.ConnectionString;
                localSettings.Values[IOTHUBKEY_TAG] = device.DeviceKey;

                deviceId = device.DeviceId;
            }
            return deviceId;
        }

        private async Task<IotHubDevice> AcquireDeviceId()
        {
            IotHubDevice device = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ServiceUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // HTTP GET
                    HttpResponseMessage response = await client.GetAsync(RequestUri + "/ 1");

                    if (response.IsSuccessStatusCode)
                    {
                        device = await response.Content.ReadAsAsync<IotHubDevice>();
                        Debug.WriteLine("AcquireDeviceId: " + device.DeviceId + ", " + device.DeviceKey + ",\r\n    " + device.ConnectionString);

                        device.IsUsed = true;
                        response = await client.PutAsJsonAsync(RequestUri + "/" + device.DevId.ToString(), device);
                        Debug.WriteLine(response.StatusCode.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return device;
        }

        public void Init(TASKEVENT_HANLDER notificactionHandler, TASKEVENT_HANLDER taskCompleteHandler, string serviceUrl, string contentId)
        {
            onNotificationHandler = notificactionHandler;
            onTaskCompletedHandler = taskCompleteHandler;
            if (serviceUrl == null)
               ServiceUri = App.configManager.ServiceUrl;
            else
               ServiceUri = serviceUrl;
            if (contentId == null)
                ContentId = App.configManager.ContentId;
            else
                ContentId = contentId;
            ApplicationData.Current.LocalSettings.Values[CONTENTID_TAG] = contentId;
        }

        // ensure background task in desired state: start/stop
        public void EnsureState(bool shouldStart)
        {
            if (taskRegistration == null)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        taskRegistration = task.Value;
                        taskRegistration.Completed += OnMessageListenerTaskCompleted;
                        taskRegistration.Progress += OnMessageListenerTaskProgress;
                        Debug.WriteLine("Task found: " + task.Value.Name + " / " + task.Value.TaskId);
                        MainPage.Current.UpdateDebugMessage(this, "Task found: " + task.Value.Name + " / " + task.Value.TaskId);
                        break;
                    }
                }
                if (shouldStart && taskRegistration != null)
                {
                    // the very first time after system boot up
                    if (App.previousExecState == Windows.ApplicationModel.Activation.ApplicationExecutionState.NotRunning)
                    {
                        // clear previous inrange list
                        ApplicationData.Current.LocalSettings.Values[CURRENTINRANGE_TAG] = "";
                        ApplicationData.Current.LocalSettings.Values[BTWatcherManager.taskName] = "";
                    }
                }
            }
            if (shouldStart)
            {
                if (taskRegistration != null)
                { 
                    // trigger background task to start
                    ApplicationTrigger trigger = new ApplicationTrigger();
                    var ignored = trigger.RequestAsync();
                }
                else
                {
                    RegisterListenerTask();
                }
            }
            else
            {
                if (taskRegistration != null)
                {
                    UnregisterrListenerTask();
                }
            }
        }

        private void RegisterListenerTask()
        {
            // Registering a background trigger if it is not already registered. It will start background advertising.
            // First get the existing tasks to see if we already registered for it
            if (taskRegistration == null)
            {
                ApplicationData.Current.LocalSettings.Values[CURRENTINRANGE_TAG] = "";
                ApplicationData.Current.LocalSettings.Values[BTWatcherManager.taskName] = "";

                // At this point we assume we haven't found any existing tasks matching the one we want to register
                // First, configure the task entry point, trigger and name
                ApplicationTrigger trigger = new ApplicationTrigger();
                //SystemTrigger trigger = new SystemTrigger(SystemTriggerType.InternetAvailable, false);

                var builder = new BackgroundTaskBuilder();
                builder.TaskEntryPoint = taskEntryPoint;
                builder.SetTrigger(trigger);
                builder.Name = taskName;

                // Now perform the registration.
                taskRegistration = builder.Register();

                // For this scenario, attach an event handler to display the result processed from the background task
                taskRegistration.Completed += OnMessageListenerTaskCompleted;
                taskRegistration.Progress += OnMessageListenerTaskProgress;

                var ignored = trigger.RequestAsync();
                MainPage.Current.UpdateDebugMessage(this, "RegisterListenerTask: " + taskRegistration.Name + " / " + taskRegistration.TaskId);
            }
        }

        private void OnMessageListenerTaskProgress(BackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
        {
            if (args.Progress == 0)
            {
                //string msg = "Internet available!";
                //onNotificationHandler(msg);
                MainPage.Current.UpdateDebugMessage(this, "first start of task");
                return;
            }
            if (onNotificationHandler != null)
            {
                string msg = ApplicationData.Current.LocalSettings.Values[taskName + NOTIFICATION_TAG] as string;
                // msg = "New Notification at " + DateTime.Now.ToString() + ((msg == null) ? "" : ("\n" + msg));
                onNotificationHandler(msg);
                MainPage.Current.UpdateDebugMessage(this, msg);
            }
        }

        private void UnregisterrListenerTask()
        {
            if (taskRegistration != null)
            {
                Debug.WriteLine("UnregisterListenerTask: unregister " + taskRegistration.Name + " / " + taskRegistration.TaskId);
                MainPage.Current.UpdateDebugMessage(this, "UnregisterListenerTask: unregister inapp " + taskRegistration.Name + " / " + taskRegistration.TaskId);
                taskRegistration.Completed -= OnMessageListenerTaskCompleted;
                taskRegistration.Progress -= OnMessageListenerTaskProgress;
                taskRegistration.Unregister(true);
                taskRegistration = null;
                //rootPage.NotifyUser("Background publisher unregistered.", NotifyType.StatusMessage);
            }
            else
            {
                // At this point we assume we haven't found any existing tasks matching the one we want to unregister
                //rootPage.NotifyUser("No registered background publisher found.", NotifyType.StatusMessage);
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        Debug.WriteLine("UnregisterListenerTask: unregister " + task.Value.Name + " / " + task.Value.TaskId);
                        MainPage.Current.UpdateDebugMessage(this, "UnregisterListenerTask: unregister " + task.Value.Name + " / " + task.Value.TaskId);
                        task.Value.Completed -= OnMessageListenerTaskCompleted;
                        task.Value.Progress -= OnMessageListenerTaskProgress;
                        task.Value.Unregister(true);
                        break;
                    }
                }
            }
            ApplicationData.Current.LocalSettings.Values[taskName + ".Cancel"] = null;
        }


        public void StopListen()
        {
            UnregisterrListenerTask();
        }

        public bool StartListen()
        {
            UnregisterrListenerTask();
            RegisterListenerTask();
            return true;
        }

        private void OnMessageListenerTaskCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            MainPage.Current.UpdateDebugMessage(this, "OnMessageListenerTaskCompleted");
            Debug.WriteLine("OnMessageListenerTaskCompleted");
            //if (onTaskCompletedHandler != null)
            //{
            //    string msg = "No internet!";
            //    onTaskCompletedHandler(msg);
            //}
            if (App.curScenario.EmuAzure == 0)
            {
                EnsureState(true);
            }
        }
    }
}
