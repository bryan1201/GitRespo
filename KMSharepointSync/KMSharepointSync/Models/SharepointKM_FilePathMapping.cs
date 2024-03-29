using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KMSharepointSync.Models
{
    public class SharepointKM_FilePathMapping
    {
        private const string KMPRDEnvironment = "PRD";
        public string TaskId { get; set; }
        public string KM_DOCUMENT_ID { get; set; }
        public string KM_FolderId { get; set; }
        public string SP_FileLeafRef { get; set; }
        public string SP_FileRef { get; set; }
        public string KM_Path { get; set; }
        public string SP_Author { get; set; }
        public string SP_Editor { get; set; }
        public string SP_Created { get; set; }
        public string SP_Modified { get; set; }
        public string SP_sortOrder { get; set; }

        public SharepointKM_FilePathMapping()
        {
        }
        public IEnumerable<SharepointKM_FilePathMapping> GetSharepointKM_FilePathMapping(string taskId)
        {
            DAO dbaccess = new DAO(KMPRDEnvironment, taskId);
            return dbaccess.GetSharepointKM_FilePathMapping();
        }

        public IEnumerable<SharepointKM_FilePathMapping> ConvertToTankReading(DataTable dataTable)
        {
            //SP_FileRef, KM_Path, SP_Author, SP_Editor, SP_Created, SP_Modified, SP_sortOrder
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new SharepointKM_FilePathMapping
                {
                    TaskId = Convert.ToString(row["TaskId"]),
                    KM_DOCUMENT_ID = Convert.ToString(row["KM_DOCUMENT_ID"]),
                    KM_FolderId = Convert.ToString(row["KM_FolderId"]),
                    SP_FileLeafRef = Convert.ToString(row["SP_FileLeafRef"]),
                    SP_FileRef = Convert.ToString(row["SP_FileRef"]),
                    KM_Path = Convert.ToString(row["KM_Path"]),
                    SP_Author = Convert.ToString(row["SP_Author"]),
                    SP_Editor = Convert.ToString(row["SP_Editor"]),
                    SP_Created = Convert.ToString(row["SP_Created"]),
                    SP_Modified = Convert.ToString(row["SP_Modified"]),
                    SP_sortOrder = Convert.ToString(row["SP_sortOrder"])
                };
            }
        }

    }
}