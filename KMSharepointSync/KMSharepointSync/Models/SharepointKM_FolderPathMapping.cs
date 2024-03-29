using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KMSharepointSync.Models
{
    public class SharepointKM_FolderPathMapping
    {
        private const string KMPRDEnvironment = "PRD";
        public string TaskId { get; set; }
        public string KM_ParentId { get; set; }
        public string KM_ParentPath { get; set; }
        public string KM_Id { get; set; }
        public string KM_Path { get; set; }
        public string SP_Id { get; set; }
        public string SP_Name { get; set; }
        public string SP_Path { get; set; }
        public string SP_IndentName { get; set; }
        public string SP_sortOrder { get; set; }
        public string SP_Url { get; set; }
        public string SP_ServerRelativeUrl { get; set; }
        public SharepointKM_FolderPathMapping()
        {
            /*
             EXEC KMDBAPIPRD.dbo.sp_UpdateKMFolder @FolderId=39575
            SELECT MA.* FROM KMDBAPIPRD.dbo.v_SharepointKM_FolderPathMapping MA WHERE MA.KM_Id IS NULL
             */
        }

        public IEnumerable<SharepointKM_FolderPathMapping> GetSharepointKM_FolderPathMapping(string taskId)
        {
            DAO dbaccess = new DAO(KMPRDEnvironment, taskId);
            return dbaccess.GetSharepointKM_FolderPathMapping();
        }

        public IEnumerable<SharepointKM_FolderPathMapping> ConvertToTankReading(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new SharepointKM_FolderPathMapping
                {
                    TaskId = Convert.ToString(row["TaskId"]),
                    KM_ParentId = Convert.ToString(row["KM_ParentId"]),
                    KM_ParentPath = Convert.ToString(row["KM_ParentPath"]),
                    KM_Id = Convert.ToString(row["KM_Id"]),
                    KM_Path = Convert.ToString(row["KM_Path"]),
                    SP_Id = Convert.ToString(row["SP_Id"]),
                    SP_Name = Convert.ToString(row["SP_Name"]),
                    SP_Path = Convert.ToString(row["SP_Path"]),
                    SP_IndentName = Convert.ToString(row["SP_IndentName"]),
                    SP_sortOrder = Convert.ToString(row["SP_sortOrder"]),
                    SP_Url = Convert.ToString(row["SP_Url"]),
                    SP_ServerRelativeUrl = Convert.ToString(row["SP_ServerRelativeUrl"])
                };
            }
        }
    }
}