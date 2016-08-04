using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartShopping.PhoneApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentPage : Page
    {
        public ContentPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string url = e.Parameter as string;
            NavigateToPageWithParameter(url, null);
        }

        public void NavigateToPageWithParameter(string parameter, object sender)
        {
            string url = (parameter != null)? parameter.Trim() : null;
            if (url != null && url.Length > 0)
            {
                if (url[0] == '?')
                {
                    url = url.Substring(1); // skip ?
                }
                try
                {
                    Debug.WriteLine("Open ContentPage: url=" + url);
                    webViewer.Source = new Uri(url);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ContentPage: " + ex.Message);
                    MainPage parentPage = sender as MainPage;
                    if (parentPage != null)
                    {
                        parentPage.NotifyUser("Failed to open url: " + parameter, NotifyType.ErrorMessage);
                    }
                }
            }
        }
    }
}