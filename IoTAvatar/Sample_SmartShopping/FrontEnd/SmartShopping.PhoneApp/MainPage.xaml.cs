using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartShopping.PhoneApp
{
    public class UserLoginAcount
    {
        public string Title { get; set; }
        public UserRecord Profile { get; set; }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public const string APP_FULLNAME = "Smart Shopping Demo App";
        public const string APP_SHORTNAME = "Smart Shopping";

        private BTWatcherManager btwatcherManager = null; // new BTWatcherManager(APP_COMPANYID);
        private RemoteMessageManager messageManager = new RemoteMessageManager();

        List<UserLoginAcount> _userLoginAccounts = new List<UserLoginAcount>();

        public MainPage()
        {
            this.InitializeComponent();

            // This is a static public property that allows downstream pages to get a handle to the MainPage instance
            // in order to call methods that are in this class.
            Current = this;
            Header.Text = APP_FULLNAME;
        }

        public void NavigateToPageWithParameter(string parameter)
        {
            ContentPage p = ContentFrame.Content as ContentPage;
            if (p != null)
            {
                p.NavigateToPageWithParameter(parameter, this);
            }
            else
            {
                ContentFrame.Navigate(typeof(ContentPage), parameter);
            }
            UpdateDebugMessage(App.Current, App.appActivationDesc);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await App.InitConfig();
            await App.InitUserManager();

            if (userLoginAccounts.Count == 0)
            {
                int currentUserIndex = -1;
                int i = 0;
                foreach (var userProfile in App.userManager.UserProfiles.Values)
                {
                    userLoginAccounts.Add(new UserLoginAcount() { Title = userProfile.DisplayName, Profile = userProfile });
                    if (userProfile.Equals(App.userManager.CurrentUser.Profile))
                        currentUserIndex = i;
                    i++;
                }
                listUserAccounts.ItemsSource = userLoginAccounts;
                if (currentUserIndex >= 0)
                    listUserAccounts.SelectedIndex = currentUserIndex;
            }

            if (Window.Current.Bounds.Width < 640)
            {
                Splitter.IsPaneOpen = false;
            }
            bool isLoggedIn = (App.userManager.CurrentUser.Status == USERLOGIN_STATUS.LOGGED_IN);
            UpdateLogin(isLoggedIn, false, e.Parameter as  string);
            App.SetScenario((App.userManager.CurrentUser.Profile == null)? null : App.userManager.CurrentUser.Profile.ScenarioID);

            messageManager.Init(OnRemoteNotification, OnRemoteDisconnected, App.configManager.ServiceUrl, App.configManager.ContentId);
            messageManager.EnsureState(isLoggedIn);
            if (btwatcherManager == null) btwatcherManager = new BTWatcherManager(App.configManager.BeaconCompanyId, App.configManager.BeaconPrefix);
            btwatcherManager.Init(App.configManager.BeaconCompanyId, App.configManager.BeaconPrefix, OnBLEMessageReceived);
            btwatcherManager.EnsureState(isLoggedIn);
            UpdateDebugMessage(App.Current, App.appActivationDesc);

            if (App.configManager.DebugLevel > 0)
            {
                DebugPanel.Visibility = Visibility.Visible;
                StatusPanel.Visibility = Visibility.Visible;
                AdvancedInfo.Visibility = Visibility.Visible;

                string btmsg = ApplicationData.Current.LocalSettings.Values[BTWatcherManager.taskName] as string;
                OnBLEMessageReceived((btmsg == null)? "" : btmsg);
            }
        }

        public void UpdateDebugMessage(object sender, string message)
        {
            var ignored = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => 
            {
                if (sender.GetType() == typeof(BTWatcherManager))
                {
                    txtBTWatcherTask.Text = "BT: " + message;
                }
                else if (sender.GetType() == typeof(RemoteMessageManager))
                {
                    txtMessageListenerTask.Text = "Recv: " + message;
                }
                else if (sender.GetType() == typeof(App))
                {
                    txtPreviousExecState.Text = message;
                }
            });
        }


        public List<UserLoginAcount> userLoginAccounts
        {
            get { return this._userLoginAccounts; }
        }

        /// <summary>
        /// Used to display messages to the user
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }
            StatusBlock.Text = strMessage;

            if (App.configManager.DebugLevel == 0) return;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Visibility.Visible;
                StatusPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Visibility.Collapsed;
                StatusPanel.Visibility = Visibility.Collapsed;
            }
        }

        async void Footer_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        private void listUserAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async Task<bool> RequestBackgroundExec()
        {
            BackgroundAccessStatus backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if ((backgroundAccessStatus == BackgroundAccessStatus.Denied) || (backgroundAccessStatus == BackgroundAccessStatus.Unspecified))
            {
                //rootPage.NotifyUser("Not able to run in background. Application must given permission to be added to lock screen.", NotifyType.ErrorMessage);
                return false;
            }
            else
            {
                //rootPage.NotifyUser("Background publisher registered.", NotifyType.StatusMessage);
                return true;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            btwatcherManager.StopBTWatcher();
            messageManager.StopListen();
            App.SetLogout();
            UpdateLogin(false, false, null);
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserRecord targetProfile = null;
            if (listUserAccounts.SelectedItem == null) return;
            UserLoginAcount selectedUserItem = listUserAccounts.SelectedItem as UserLoginAcount;
            if (selectedUserItem != null)
                targetProfile = selectedUserItem.Profile;
            if (targetProfile == null) return;

            btwatcherManager.StopBTWatcher();
            messageManager.StopListen();
            App.SetLogout();

            btnLogin.IsEnabled = false;
            listUserAccounts.IsEnabled = false;
            btnTogglePanel.IsEnabled = false;

            NotifyUser("Logging in ...", NotifyType.StatusMessage);

            Task.Run(async () => 
            {
                bool isSuccess = false;
                UserRecord userProfile = targetProfile;
                App.SetScenario(targetProfile.ScenarioID);
                string deviceId = await messageManager.GetDeviceId();
                if (deviceId != null)
                {
                    await RequestBackgroundExec();
                    messageManager.Init(OnRemoteNotification, OnRemoteDisconnected, App.configManager.ServiceUrl, App.configManager.ContentId);
                    messageManager.StartListen();
                    btwatcherManager.Init(App.configManager.BeaconCompanyId, App.configManager.BeaconPrefix, OnBLEMessageReceived);
                    btwatcherManager.StartBTWatcher();
                    isSuccess = true;
                }
                if (isSuccess)
                    App.SetLogin(targetProfile);
                UpdateLogin(isSuccess, isSuccess, null);
            });
        }

        private void UpdateLogin(bool isLoggedIn, bool hasErr, string parameters)
        {
            var ignored = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                btnTogglePanel.IsEnabled = true;
                if (isLoggedIn)
                {
                    var deviceId = messageManager.PeekDeviceId();
                    if (deviceId != null && deviceId.Length > 0)
                    {
                        txtDeviceId.Text = "DeviceId: " + deviceId; // + "\r\n" + "Serivce: " + App.configManager.ServiceUrl; 
                        DeviceInfo.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        DeviceInfo.Visibility = Visibility.Collapsed;
                    }

                    
                    btnLogout.IsEnabled = true;
                    btnLogout.Visibility = Visibility.Visible;
                    btnLogin.Visibility = Visibility.Collapsed;
                    listUserAccounts.IsEnabled = false;


                    Header.Text = App.userManager.CurrentUser.Profile.DisplayName + " @" + APP_SHORTNAME;

                    btnLogout.IsEnabled = true;
                    Splitter.IsPaneOpen = false;

                    NotifyUser("", NotifyType.StatusMessage);
                    if (parameters == null)
                    {
                        string contentId = App.configManager.ContentId;
                        parameters = "ms-appx-web:///" + contentId + "/promote.html?pcode=welcome";
                    }
                    NavigateToPageWithParameter(parameters);
                }
                else
                {
                    DeviceInfo.Visibility = Visibility.Collapsed;
                    btnLogout.Visibility = Visibility.Collapsed;
                    btnLogin.Visibility = Visibility.Visible;
                    btnLogin.IsEnabled = true;
                    listUserAccounts.IsEnabled = true;

                    Header.Text = APP_FULLNAME;

                    Splitter.IsPaneOpen = true;

                    if (hasErr)
                        NotifyUser("Login Failed!", NotifyType.ErrorMessage);
                    else
                        NotifyUser("", NotifyType.StatusMessage);
                    ContentFrame.Navigate(typeof(BlankPage), parameters);
                }
            });
        }

        private void OnBLEMessageReceived(string message)
        {
            if (App.configManager.DebugLevel <= 0) return;

            var ignored = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                const string CURRENTINRANGE_TAG = "BTCurrentInRange";
                string inrangelist = ApplicationData.Current.LocalSettings.Values[CURRENTINRANGE_TAG] as string;
                string debugmsg = ApplicationData.Current.LocalSettings.Values[BTWatcherManager.taskName + "_Debug"] as string;
                if (debugmsg == null) debugmsg = "";
                else if (debugmsg.Length > 0) debugmsg += "\n";
                if (inrangelist == null) inrangelist = "";
                else if (inrangelist.Length > 0) inrangelist += "\n";
                message = debugmsg + inrangelist + message;
                DebugOutputBlock.Text = message;
            });
        }

        private void OnRemoteNotification(string message)
        {
            var ignored = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                string msg = "New Notification at " + DateTime.Now.ToString() + ((message == null) ? "" : ("\n" + message));
                NotifyUser(msg, NotifyType.StatusMessage);
            });
        }
        private void OnRemoteDisconnected(string message)
        {
            var ignored = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyUser(message, NotifyType.StatusMessage);
            });
        }

        private void StatusBlock_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            NotifyUser("", NotifyType.StatusMessage);
        }
    }

    public enum NotifyType
    {
        StatusMessage,
        ErrorMessage
    };
}
