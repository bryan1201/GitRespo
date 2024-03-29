using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.SharePoint.Client;
using KMSharepointSync.Models;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using Newtonsoft.Json.Linq;
using System.IO;

namespace KMSharepointSync.Controllers
{
    public class SharepointOnlineController : Controller
    {
        // GET: SharepointOnline

        public ActionResult Index(string TaskId)
        {
            ViewBag.Title = "SharepointOnline Sync Task";
            ViewBag.Message = "Read Folders and Files";
            SyncTaskInfoList stinfo = new SyncTaskInfoList();
            if (!string.IsNullOrEmpty(TaskId))
            {
                APIs.SyncTaskController st = new APIs.SyncTaskController();
                ViewBag.TaskMessage = st.RunByTaskId(TaskId);
            }

            return View(stinfo.GetSyncTaskInfoList());
        }

        public ActionResult SharepointOnlineSync()
        {
            string _taskId = Constant.TaskId;
            SharepointKM_FolderPathMapping spkmfolder = new SharepointKM_FolderPathMapping();
            IEnumerable<SharepointKM_FolderPathMapping> spkmFolderMappingList = spkmfolder.GetSharepointKM_FolderPathMapping(_taskId);
            foreach(SharepointKM_FolderPathMapping item in spkmFolderMappingList)
            {
                if (string.IsNullOrEmpty(item.KM_Id))
                    KMService.AddKMFolder(_taskId, item.KM_ParentId, item.SP_Name);
                
            }

            SharepointKM_FilePathMapping spkmfile = new SharepointKM_FilePathMapping();
            IEnumerable<SharepointKM_FilePathMapping> spkmFileMappingList = spkmfile.GetSharepointKM_FilePathMapping(_taskId);
            foreach(SharepointKM_FilePathMapping item in spkmFileMappingList)
            {
                if (string.IsNullOrEmpty(item.KM_DOCUMENT_ID))
                    KMService.AddKMFile(spkmfilepath: item);                
            }

            IEnumerable<SharepointKM_FilePathMapping> v_SharepointKM_FilePathMapping = spkmfile.GetSharepointKM_FilePathMapping(_taskId).OrderBy(x => x.SP_sortOrder);

            return View(v_SharepointKM_FilePathMapping);
        }
        public ActionResult SharepointOnlineFolderList()
        {
            string _url = Constant.SharepointOnlineSite;
            string _spSharedDocLib = Constant.spSharedDocLib;
            string _taskId = Constant.TaskId;

            List<SharepointFolder> spfolderList = new List<SharepointFolder>();
            SharepointFolderList splist = new SharepointFolderList(taskId:_taskId);
            splist.GetSharepointFolder(url:_url, spSharedDocLib:_spSharedDocLib, kmadm:Constant.KMUseremail, kmpwd:Constant.KMUserPassword, kmRootFolderId:Constant.KMFolderId);
            spfolderList = splist.GetSPFolderList();

            ViewBag.Url = splist.GetWebUrl();// web.Url;
            ViewBag.ServerRelativeUrl = splist.GetServerRelativeUrl();// web.ServerRelativeUrl;
            ViewBag.Title = splist.GetWebTitle();// web.Title;
            ViewBag.Message = splist.GetWebDescription();// web.Description;
            ViewData["spfolderList"] = spfolderList.OrderBy(x=>x.Path);
            IEnumerable<SharepointFolder> viewspfolderList = spfolderList.OrderBy(x => x.Path);
            return View(viewspfolderList);
        }
        public ActionResult SharepointKMFolderMappingList()
        {
            string taskId = Constant.TaskId;
            SharepointKM_FolderPathMapping spkmfolder = new SharepointKM_FolderPathMapping();
            IEnumerable<SharepointKM_FolderPathMapping> v_SharepointKM_FolderPathMapping = spkmfolder.GetSharepointKM_FolderPathMapping(taskId).OrderBy(x=>x.SP_sortOrder);
            return View(v_SharepointKM_FolderPathMapping);
        }
        public ActionResult SharepointKMFileMappingList()
        {
            string taskId = Constant.TaskId;
            SharepointKM_FilePathMapping spkmfile = new SharepointKM_FilePathMapping();
            IEnumerable<SharepointKM_FilePathMapping> v_SharepointKM_FilePathMapping = spkmfile.GetSharepointKM_FilePathMapping(taskId).OrderBy(x => x.SP_sortOrder);
            return View(v_SharepointKM_FilePathMapping);
        }

        public ActionResult AddKMFolder(string parentFolderId, string newKMSubFolderName)
        {
            string taskId = Constant.TaskId;
            SharepointFolderList sfl = new SharepointFolderList(taskId);
            IEnumerable<SharepointKM_FolderPathMapping> spkmMappingList =  KMService.AddKMFolder(taskId, parentFolderId, newKMSubFolderName);
           
            ViewData["theAcquireDraftResult"] = sfl.GetJsonAcquireDraftFolderResult();
            ViewData["JsonStringIndent"] = sfl.GetJsonAcquireDraftFolderResultIndent();
            ViewData["resultAddFolder"] = sfl.GetJsonResultAddFolder();

            return View(spkmMappingList);
        }

        public ActionResult AddKMFile(string UploadFolderId, string DocTitle, string DocRefUrl, string DocDescription, string Author, string CreateDate)
        {
            string taskId = Constant.TaskId;
            if (string.IsNullOrEmpty(UploadFolderId) || string.IsNullOrEmpty(DocTitle) || string.IsNullOrEmpty(DocRefUrl) || string.IsNullOrEmpty(DocDescription) || string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(CreateDate))
            {
                return View();
            }
            else
            {
                SharepointKM_FilePathMapping item = new SharepointKM_FilePathMapping();
                item.TaskId = taskId;
                item.KM_FolderId = UploadFolderId;
                item.SP_FileLeafRef = DocTitle;
                item.SP_FileRef = DocRefUrl;
                item.KM_Path = DocDescription;
                item.SP_Author = Author;
                item.SP_Created = CreateDate;
                
                KMService.AddKMFile(spkmfilepath:item);
            }
            return View();
        }


        public ActionResult UpdateJason()
        {
            string json = @"{
              'channel': {
                'title': 'Star Wars',
                'link': 'http://www.starwars.com',
                'description': 'Star Wars blog.',
                'obsolete': 'Obsolete value',
                'item': []
              }
            }";

            JObject rss = JObject.Parse(json);

            JObject channel = (JObject)rss["channel"];

            channel["title"] = ((string)channel["title"]).ToUpper();
            channel["description"] = ((string)channel["description"]).ToUpper();

            channel.Property("obsolete").Remove();

            channel.Property("description").AddAfterSelf(new JProperty("new", "New value"));

            JArray item = (JArray)channel["item"];
            item.Add("Item 1");
            item.Add("Item 2");

            ViewData["rss"] = rss.ToString();
            return View();
        }
    }
}