using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
// **********************************************
using System.Configuration;
using Microsoft.ServiceBus.Notifications;
using Newtonsoft.Json;
// **********************************************

namespace VS2012MVC4.Controllers.APIs
{
    public class UserController : ApiController
    {
        private async void SendNotificationAsync()
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            string notificationHubPath = ConfigurationManager.AppSettings["Microsoft.ServiceBus.NotificationHubPath"];

            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, notificationHubPath);
            //var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">Hello from a .NET App!</text></binding></visual></toast>";
            //await hub.SendWindowsNativeNotificationAsync(toast);
            await hub.SendGcmNativeNotificationAsync("{ \"data\" : { \"message\" : \"Hello from Windows Azure!\" } }");
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}