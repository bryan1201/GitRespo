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

        public IEnumerable<SharepointKM_FolderPathMapping> GetSharepointKM_FolderPathMapping()
        {
            DAO dbaccess = new DAO(KMPRDEnvironment);
            return dbaccess.GetSharepointKM_FolderPathMapping();
        }
    }
}