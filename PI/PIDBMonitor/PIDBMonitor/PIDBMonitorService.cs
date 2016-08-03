using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Net;

namespace PIDBMonitor
{
    public partial class PIDBMonitorService : ServiceBase
    {
        private Timer MyTimer;
        private string ContentType { get; set; }
        private const string _contentType = @"application/json; charset=utf-8";
        private string Url = Constant.LogDBStatusUri;
        //default url == @"http://iec1-b2bapp.iec.inventec/B2BService/Statistic/LogDBStatus";
        private int TimeInterval = Constant.TimeInterval;

        public PIDBMonitorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            MyTimer = new Timer();

            MyTimer.Elapsed += new ElapsedEventHandler(MyTimer_Elapsed);

            MyTimer.Interval = TimeInterval * 1000;

            MyTimer.Start();
        }

        private void MyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GetWebResponse(this.Url);
        }

        protected override void OnStop()
        {
            if (MyTimer != null)
                MyTimer.Stop();
        }

        private string GetWebResponse(string url)
        {
            string response = string.Empty;
            ContentType = _contentType;
            try
            {
                NetworkCredential mycr = new NetworkCredential();
                using (var client = new WebClient())
                {
                    client.Credentials = mycr;
                    client.Headers[HttpRequestHeader.ContentType] = this.ContentType;
                    client.UseDefaultCredentials = true;
                    client.Encoding = System.Text.Encoding.UTF8;
                    response = client.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
    }
}
