using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KMSharepointSync.Models
{
    public class SyncTaskInfoLogList
    {
        private const string KMPRDEnvironment = "PRD";
        private IEnumerable<SyncTaskInfoLog> ListSyncTaskInfoLog { get; set; }
        public SyncTaskInfoLog GetSyncTaskInfoLog(string taskId)
        {
            ListSyncTaskInfoLog = GetSyncTaskInfoLogList();
            return ListSyncTaskInfoLog.Where(x => x.TaskId == taskId).FirstOrDefault();
        }
        public IEnumerable<SyncTaskInfoLog> GetSyncTaskInfoLogList()
        {
            DAO dbaccess = new DAO(KMPRDEnvironment, taskId: string.Empty);
            return dbaccess.GetSyncTaskInfoLogList();
        }
        public IEnumerable<SyncTaskInfoLog> ConvertToTankReading(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new SyncTaskInfoLog
                {
                    LogId = Convert.ToString(row["LogId"]),
                    TaskId = Convert.ToString(row["TaskId"]),
                    UserId = Convert.ToString(row["UserId"]),
                    LogMessage = Convert.ToString(row["LogMessage"]),
                    StatusCode = Convert.ToString(row["StatusCode"]),
                    StatusDescription = Convert.ToString(row["StatusDescription"]),
                    StartDateTime = Convert.ToDateTime(row["StartDateTime"]),
                    EndDateTime = Convert.ToDateTime(row["EndDateTime"])
                };
            }
        }

    }
}