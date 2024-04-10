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
        private string TaskResult { get; set; }
        private string TaskId { get; set; }
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
            SyncTaskInfoLog stinfolog = new SyncTaskInfoLog();
            stinfolog.TaskId = Id;
            stinfolog.StartDateTime = DateTime.Now;

            try
            {
                //Start Task
                TaskId = Id;
                SyncTaskInfoList lststinfo = new SyncTaskInfoList();
                SyncTaskInfo stinfo = lststinfo.GetSyncTaskInfo(taskId: Id);
                TaskResult = stinfo.RunTask();
                //End Task

                stinfolog.StatusCode = "0";
                stinfolog.StatusDescription = "Done with no error.";
                stinfolog.LogMessage = TaskResult;
            }
            catch(Exception ex)
            {
                stinfolog.StatusCode = "2";
                stinfolog.StatusDescription = string.Format("{0}, {1}", "SyncTaskController:API:RunByTaskId", ex.Message);
                stinfolog.LogMessage = string.Format("{0}, {1}", "API Failed", TaskResult);
            }
            stinfolog.UserId = System.Net.Dns.GetHostName();            
            stinfolog.EndDateTime = DateTime.Now;
            stinfolog.AddSyncTaskInfoLog(stinfolog);

            return TaskResult;
        }

        [HttpGet]
        public string GetTaskResult()
        {
            return TaskResult;
        }

        [HttpGet]
        public string GetTaskId()
        {
            return TaskId;
        }

    }
}