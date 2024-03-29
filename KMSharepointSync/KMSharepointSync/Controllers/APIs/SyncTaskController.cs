using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using KMSharepointSync.Models;
using Newtonsoft.Json;

namespace KMSharepointSync.Controllers.APIs
{
    public class SyncTaskController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public HttpResponseMessage RunTest()
        {
            var dateNow = new Dictionary<string, string>()
            {
                {"Date", DateTime.Now.ToShortDateString()},
                {"Time", DateTime.Now.ToShortTimeString()}
            };

            string json = JsonConvert.SerializeObject(dateNow, Formatting.Indented);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(json);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }

        [HttpGet]
        public string RunByTaskId(string Id)
        {
            string result = string.Empty;
            SyncTaskInfoList lststinfo = new SyncTaskInfoList();
            SyncTaskInfo stinfo  = lststinfo.GetSyncTaskInfo(taskId: Id);
            result = stinfo.RunTask();
            return result;
        }

    }
}