using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMSharepointSync.Models
{
    public class SyncTaskInfo
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string SharePointVersion { get; set; }
        public string SharePointUri { get; set; }
        public string AuthId { get; set; }
        public string AuthPassword { get; set; }
        public string SharePointListName { get; set; }
        public string EspDocumentClassGroupId { get; set; }
        public string EspDocumentClassId { get; set; }
        public int EspFolderId { get; set; }
        public bool IsUploadSharePointFile { get; set; }
        public int SchedulePriod { get; set; }
        public string ScheduleStartTime { get; set; }
        public string ScheduleInterval { get; set; }
        public string ScheduleStartDate { get; set; }
        public string ScheduleWeekDay { get; set; }
        public string ScheduleMoth { get; set; }
        public string Creator { get; set; }
        public string LastModifier { get; set; }
        public string ScheduleEndDatetime { get; set; }
        public bool ScheduleTriggerImmediately { get; set; }
        public bool ScheduleImmediateTriggerHaveRunOnce { get; set; }
        public string EspApiTenant { get; set; }
        public string LastExecutionDatetime { get; set; }
        public string Status { get; set; }
        public DateTime Cdt { get; set; }

        public string RunTask()
        {
            string result = string.Format("Successful Task {0}, TaskName {1}", TaskId, TaskName);
            try
            {
                SharepointOnlineFolderList();
                SharepointOnlineSync();
            }
            catch(Exception e)
            {
                result = string.Format("Exception on Task {0}, TaskName {1}, \r\n Message {2}", TaskId, TaskName, e.Message); 
            }
            return result;
        }
        public IEnumerable<SharepointFolder> SharepointOnlineFolderList()
        {
            string _url = SharePointUri;
            string _spSharedDocLib = SharePointListName;
            string _taskId = TaskId;

            List<SharepointFolder> spfolderList = new List<SharepointFolder>();
            SharepointFolderList splist = new SharepointFolderList(taskId: _taskId);
            splist.GetSharepointFolder(url: _url, spSharedDocLib: _spSharedDocLib, kmadm: AuthId, kmpwd: AuthPassword, kmRootFolderId: EspFolderId);
            spfolderList = splist.GetSPFolderList();

            IEnumerable<SharepointFolder> viewspfolderList = spfolderList.OrderBy(x => x.Path);
            return viewspfolderList;
        }

        public IEnumerable<SharepointKM_FilePathMapping> SharepointOnlineSync()
        {
            string _taskId = TaskId;
            SharepointKM_FolderPathMapping spkmfolder = new SharepointKM_FolderPathMapping();
            IEnumerable<SharepointKM_FolderPathMapping> spkmFolderMappingList = spkmfolder.GetSharepointKM_FolderPathMapping(_taskId);
            foreach (SharepointKM_FolderPathMapping item in spkmFolderMappingList)
            {
                if (string.IsNullOrEmpty(item.KM_Id))
                    KMService.AddKMFolder(_taskId, item.KM_ParentId, item.SP_Name);

            }

            SharepointKM_FilePathMapping spkmfile = new SharepointKM_FilePathMapping();
            IEnumerable<SharepointKM_FilePathMapping> spkmFileMappingList = spkmfile.GetSharepointKM_FilePathMapping(_taskId);
            foreach (SharepointKM_FilePathMapping item in spkmFileMappingList)
            {
                if (string.IsNullOrEmpty(item.KM_DOCUMENT_ID))
                    KMService.AddKMFile(spkmfilepath: item);
            }

            IEnumerable<SharepointKM_FilePathMapping> v_SharepointKM_FilePathMapping = spkmfile.GetSharepointKM_FilePathMapping(_taskId).OrderBy(x => x.SP_sortOrder);

            return v_SharepointKM_FilePathMapping;
        }
    }

}