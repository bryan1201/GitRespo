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
        public IEnumerable<SharepointKM_FilePathMapping> GetSharepointKM_FilePathMapping()
        {
            DAO dbaccess = new DAO(KMPRDEnvironment);
            return dbaccess.GetSharepointKM_FilePathMapping();
        }
    }
}