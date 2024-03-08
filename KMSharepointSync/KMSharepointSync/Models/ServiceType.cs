using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMSharepointSync.Models
{
    public enum ServiceType
    {
        GetRootFolder,
        UploadFile,
        AcquireDocumentDraft,
        SubmitNewDocument,
        AllDocumentClass,
        GetCategoryInfo,
        GetSearchExBySimple,
        DeleteDocument,
        GetDocument,
        AddUser,
        GetUser,
        GetUserBySubjectId,
        UpdateUser,
        DeleteUser,
        GetAllUser,
        DownloadFile2,
        GetAdvancedResult,
        AcquireFolderDraft,
        AddFolder,
        GetDocumentFileById
    }
}