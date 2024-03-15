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

        public ActionResult Index()
        {
            ViewBag.Title = "Test SharepointOnlineSync";
            ViewBag.Message = "Read Folders and Files";
            return View();
        }

        public ActionResult SharepointOnlineSync()
        {
            SharepointKM_FolderPathMapping spkmfolder = new SharepointKM_FolderPathMapping();
            IEnumerable<SharepointKM_FolderPathMapping> spkmFolderMappingList = spkmfolder.GetSharepointKM_FolderPathMapping();
            foreach(SharepointKM_FolderPathMapping item in spkmFolderMappingList)
            {
                if (string.IsNullOrEmpty(item.KM_Id))
                {
                    SharepointFolderList sfl = new SharepointFolderList();
                    sfl.AddKMFolder(item.KM_ParentId, item.SP_Name);
                }
            }

            SharepointKM_FilePathMapping spkmfile = new SharepointKM_FilePathMapping();
            IEnumerable<SharepointKM_FilePathMapping> spkmFileMappingList = spkmfile.GetSharepointKM_FilePathMapping();
            foreach(SharepointKM_FilePathMapping item in spkmFileMappingList)
            {
                if (string.IsNullOrEmpty(item.KM_DOCUMENT_ID))
                {
                    SharepointFolderList sf2 = new SharepointFolderList();

                    sf2.AddKMFile(spkmfilepath: item);
                }
            }

            IEnumerable<SharepointKM_FilePathMapping> v_SharepointKM_FilePathMapping = spkmfile.GetSharepointKM_FilePathMapping().OrderBy(x => x.SP_sortOrder);

            return View(v_SharepointKM_FilePathMapping);
        }
        public ActionResult SharepointOnlineFolderList()
        {
            string _url = @"https://inventeccorp.sharepoint.com/sites/msteams_a0f382_290647/";
            string _spSharedDocLib = "文件";

            List<SharepointFolder> spfolderList = new List<SharepointFolder>();
            SharepointFolderList splist = new SharepointFolderList();
            splist.GetSharepointFolder(url:_url, spSharedDocLib:_spSharedDocLib, kmadm:Constant.KMUseremail, kmpwd:Constant.KMUserPassword);
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
            SharepointKM_FolderPathMapping spkmfolder = new SharepointKM_FolderPathMapping();
            IEnumerable<SharepointKM_FolderPathMapping> v_SharepointKM_FolderPathMapping = spkmfolder.GetSharepointKM_FolderPathMapping().OrderBy(x=>x.SP_sortOrder);
            return View(v_SharepointKM_FolderPathMapping);
        }
        public ActionResult SharepointKMFileMappingList()
        {
            SharepointKM_FilePathMapping spkmfile = new SharepointKM_FilePathMapping();
            IEnumerable<SharepointKM_FilePathMapping> v_SharepointKM_FilePathMapping = spkmfile.GetSharepointKM_FilePathMapping().OrderBy(x => x.SP_sortOrder);
            return View(v_SharepointKM_FilePathMapping);
        }

        public ActionResult AddKMFolder(string parentFolderId, string newKMSubFolderName)
        {
            SharepointFolderList sfl = new SharepointFolderList();
            IEnumerable<SharepointKM_FolderPathMapping> spkmMappingList = sfl.AddKMFolder(parentFolderId, newKMSubFolderName);
            ViewData["theAcquireDraftResult"] = sfl.GetJsonAcquireDraftFolderResult();
            ViewData["JsonStringIndent"] = sfl.GetJsonAcquireDraftFolderResultIndent();
            ViewData["resultAddFolder"] = sfl.GetJsonResultAddFolder();

            return View(spkmMappingList);
        }

        public ActionResult AddKMFile(string UploadFolderId, string DocTitle, string DocRefUrl, string DocDescription, string Author, string CreateDate)
        {
            if (string.IsNullOrEmpty(UploadFolderId) || string.IsNullOrEmpty(DocTitle) || string.IsNullOrEmpty(DocRefUrl) || string.IsNullOrEmpty(DocDescription) || string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(CreateDate))
            {
                return View();
            }
            else
            {
                SharepointKM_FilePathMapping item = new SharepointKM_FilePathMapping();
                item.KM_FolderId = UploadFolderId;
                item.SP_FileLeafRef = DocTitle;
                item.SP_FileRef = DocRefUrl;
                item.KM_Path = DocDescription;
                item.SP_Author = Author;
                item.SP_Created = CreateDate;
                
                SharepointFolderList sfl = new SharepointFolderList();
                sfl.AddKMFile(spkmfilepath:item);
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