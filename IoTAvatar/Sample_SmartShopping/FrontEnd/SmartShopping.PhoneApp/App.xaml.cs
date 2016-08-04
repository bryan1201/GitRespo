using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmartShopping.PhoneApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private const string CURRENTUSER_TAG = "CurrentUser";
        private const string CURRENTSCENARIO_EMUAZURE_TAG = "EmuAzure";

        public static AppConfiguration configManager { get; private set; }
        public static UserManager userManager { get; private set; }

        public static Scenario curScenario { get; private set; }
        public static ApplicationExecutionState previousExecState { get; private set;  }
        public static string appActivationDesc { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += App_Resuming;
        }

        public static void SetScenario(string scenarioId)
        {
            try
            {
                curScenario = configManager.Scenarios[scenarioId];
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SetScenario: " + ex.Message);
                curScenario = null;
            }
            if (curScenario == null)
            {
                curScenario = configManager.Scenarios.First().Value;
            }
            ApplicationData.Current.LocalSettings.Values[CURRENTSCENARIO_EMUAZURE_TAG] = curScenario.EmuAzure.ToString();
        }

        public static void SetLogin(UserRecord userProfile)
        {
            if (userProfile == null) return;
            if (userManager != null)
            {
                userManager.CurrentUser.Status = USERLOGIN_STATUS.LOGGED_IN;
                userManager.CurrentUser.Profile = userProfile;

                ApplicationData.Current.LocalSettings.Values[CURRENTUSER_TAG] = userProfile.ID;
            }
        }

        public static void SetLogout()
        {
            if (userManager != null)
            {
                userManager.CurrentUser.Status = USERLOGIN_STATUS.LOGGED_OUT;
                userManager.CurrentUser.Profile = null;
            }
            ApplicationData.Current.LocalSettings.Values[CURRENTUSER_TAG] = null;
        }

        public static async Task<bool> InitUserManager()
        {
            if (userManager == null)
            {
                userManager = new UserManager();
                await userManager.LoadDataFromXml(configManager.UserDataFile);

                string userId = ApplicationData.Current.LocalSettings.Values[CURRENTUSER_TAG] as string;
                if (userId != null)
                {
                    UserRecord userProfile = null;
                    try
                    {
                        userProfile = userManager.UserProfiles[userId] as UserRecord;
                    }
                    catch (Exception)
                    { }
                    if (userProfile != null)
                    {
                        userManager.CurrentUser.Status = USERLOGIN_STATUS.LOGGED_IN;
                        userManager.CurrentUser.Profile = userProfile;
                    }
                    else
                    {
                        ApplicationData.Current.LocalSettings.Values[CURRENTUSER_TAG] = null;
                    }
                }
            }
            return true;
        }

        public static async Task<bool> InitConfig()
        {
            if (configManager == null)
            {
                configManager = new AppConfiguration();
                await configManager.LoadDataFromXml(null);

                const string DEVICEID_TAG = "DeviceId";
                const string IOTDEVICESERVICEURL_TAG = "IotDeviceServiceUrl";
                string deviceId = ApplicationData.Current.LocalSettings.Values[DEVICEID_TAG] as string;
                if (deviceId != null && deviceId.Length > 0)
                {
                    string iotDeviceUrl = ApplicationData.Current.LocalSettings.Values[IOTDEVICESERVICEURL_TAG] as string;

                    if (iotDeviceUrl == null || !iotDeviceUrl.Equals(App.configManager.ServiceUrl))
                    {
                        // force renew device ID, force logout
                        RemoteMessageManager.ResetLocalDevice();
                        ApplicationData.Current.LocalSettings.Values[CURRENTUSER_TAG] = null;
                    }
                }
            }
            return true;
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            previousExecState = e.PreviousExecutionState;
            Debug.WriteLine("App OnActivated: previous state = " + e.PreviousExecutionState.ToString());

            appActivationDesc = "App OnActivated: previous state = " + e.PreviousExecutionState.ToString() + "\nkind=" + e.Kind.ToString();

            Type pageType = null;
            Uri uri = null;
            if (e.Kind == ActivationKind.ToastNotification)
            {
                //Get the pre-defined arguments and user inputs from the eventargs;
                ToastNotificationActivatedEventArgs toastArgs = e as ToastNotificationActivatedEventArgs;
                var arguments = toastArgs.Argument;
                pageType = typeof(MainPage);
                uri = new Uri("ms-appx:///?" + arguments);

                appActivationDesc += toastArgs.Argument;
            }

            if (pageType == null)
            {
                Application.Current.Exit();
                return;
            }

            // TODO: Handle URI activation
            // The received URI is eventArgs.Uri.AbsoluteUri
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                rootFrame.Navigate(pageType, uri.Query);

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            else
            {
                MainPage p = rootFrame.Content as MainPage;
                if (p != null && pageType == typeof(MainPage))
                {
                    p.NavigateToPageWithParameter(uri.Query);
                }
                else
                {
                    rootFrame.Navigate(pageType, uri.Query);
                }
            }
            
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            previousExecState = e.PreviousExecutionState;
            Debug.WriteLine("App OnLaunched: previous state = " + e.PreviousExecutionState.ToString());

            appActivationDesc = "App OnLaunched: previous state = " + e.PreviousExecutionState.ToString() + "\nkind=" + e.Kind.ToString();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void App_Resuming(object sender, object e)
        {
            if (appActivationDesc == null)
                appActivationDesc = "";
            else
                appActivationDesc += "\n";
            appActivationDesc += "Resumed";
        }
    }
}
