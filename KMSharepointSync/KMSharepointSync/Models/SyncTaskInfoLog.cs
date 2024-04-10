using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMSharepointSync.Models
{
    public class SyncTaskInfoLog
    {
        private const string KMPRDEnvironment = "PRD";
        public string LogId { get; set; }
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public string LogMessage { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public string SetDateTimeToString(DateTime dtNow)
        {
            if (dtNow == null)
                dtNow = DateTime.Now;
            return dtNow.ToString("yyyy/MM/dd HH:mm:ss.fff");
        }

        public string SetLogIdToString(string _logId)
        {
            _logId = string.IsNullOrEmpty(_logId)?System.Guid.NewGuid().ToString():_logId;
            return LogId;
        }

        public bool AddSyncTaskInfoLog(SyncTaskInfoLog item)
        {
            try
            {
                DAO dao = new DAO(KMPRDEnvironment, item.TaskId);
                dao.AddSyncTaskInfoLog(item);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return false;
            }
            return true;
        }
    }
}