using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KMSharepointSync.Models
{
    public class SyncTaskInfoList
    {
        private const string KMPRDEnvironment = "PRD";

        private IEnumerable<SyncTaskInfo> ListSyncTaskInfo { get; set; }

        public SyncTaskInfoList()
        {
            
        }

        public SyncTaskInfo GetSyncTaskInfo(string taskId)
        {
            ListSyncTaskInfo = GetSyncTaskInfoList();
            return ListSyncTaskInfo.Where(x => x.TaskId == taskId).FirstOrDefault();
        }
        public IEnumerable<SyncTaskInfo> GetSyncTaskInfoList()
        {
            DAO dbaccess = new DAO(KMPRDEnvironment, taskId: string.Empty);
            return dbaccess.GetSyncTaskInfoList();
        }
        public IEnumerable<SyncTaskInfo> ConvertToTankReading(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new SyncTaskInfo
                {
                    TaskId = Convert.ToString(row["TaskId"]),
                    TaskName = Convert.ToString(row["TaskName"]),
                    TaskDescription = Convert.ToString(row["TaskDescription"]),
                    SharePointVersion = Convert.ToString(row["SharePointVersion"]),
                    SharePointUri = Convert.ToString(row["SharePointUri"]),
                    AuthId = Convert.ToString(row["AuthId"]),
                    AuthPassword = Convert.ToString(row["AuthPassword"]),
                    SharePointListName = Convert.ToString(row["SharePointListName"]),
                    EspDocumentClassGroupId = Convert.ToString(row["EspDocumentClassGroupId"]),
                    EspDocumentClassId = Convert.ToString(row["EspDocumentClassId"]),
                    EspFolderId = Convert.ToInt32(row["EspFolderId"]),
                    IsUploadSharePointFile = Convert.ToBoolean(row["IsUploadSharePointFile"]),
                    SchedulePriod = Convert.ToInt32(row["SchedulePriod"]),
                    ScheduleStartTime = Convert.ToString(row["ScheduleStartTime"]),
                    ScheduleInterval = Convert.ToString(row["ScheduleInterval"]),
                    ScheduleStartDate = Convert.ToString(row["ScheduleStartDate"]),
                    ScheduleWeekDay = Convert.ToString(row["ScheduleWeekDay"]),
                    ScheduleMoth = Convert.ToString(row["ScheduleMoth"]),
                    Creator = Convert.ToString(row["Creator"]),
                    LastModifier = Convert.ToString(row["LastModifier"]),
                    ScheduleEndDatetime = Convert.ToString(row["ScheduleEndDatetime"]),
                    ScheduleTriggerImmediately = Convert.ToBoolean(row["ScheduleTriggerImmediately"]),
                    ScheduleImmediateTriggerHaveRunOnce = Convert.ToBoolean(row["ScheduleImmediateTriggerHaveRunOnce"]),
                    EspApiTenant = Convert.ToString(row["EspApiTenant"]),
                    LastExecutionDatetime = Convert.ToString(row["LastExecutionDatetime"]),
                    Status = Convert.ToString(row["Status"]),
                    Cdt = Convert.ToDateTime(row["Cdt"])
                };
            }
        }
    }
}