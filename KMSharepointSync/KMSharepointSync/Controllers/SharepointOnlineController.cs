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
            List<SharepointFolder> spfolderList = new List<SharepointFolder>();
            SharepointFolderList splist = new SharepointFolderList();
            Web web = splist.GetSharepointFolder();
            spfolderList = splist.SPFolderList;

            ViewBag.Url = web.Url;
            ViewBag.ServerRelativeUrl = web.ServerRelativeUrl;
            ViewBag.Title = web.Title;
            ViewBag.Message = web.Description;
            ViewData["spfolderList"] = spfolderList.OrderBy(x=>x.Path);
            IEnumerable<SharepointFolder> viewspfolderList = spfolderList.OrderBy(x => x.Path);
            return View(viewspfolderList);
        }
        public ActionResult AddKMFolder(string parentFolderId, string newKMSubFolderName)
        {

            SharepointKM_FolderPathMapping spfpm = new SharepointKM_FolderPathMapping();
            IEnumerable<SharepointKM_FolderPathMapping> spkmMappingList = spfpm.GetSharepointKM_FolderPathMappin();

            if (string.IsNullOrEmpty(parentFolderId) || string.IsNullOrEmpty(newKMSubFolderName))
            {
                return View(spkmMappingList);
            }
            else
            {
                string theAcquireDraftResult = "";
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                //注意參數是接在QueryString內
                string targetUrl = KMService.GetServiceUrl(ServiceType.AcquireFolderDraft, docId: "0", userId: Constant.KMUserId, tenant: Constant.TENANT);

                WebClient client2 = new WebClient();
                client2.Encoding = Encoding.UTF8;
                string targetDraftUrl = KMService.GetServiceUrl(ServiceType.AcquireFolderDraft, docId: "0", userId: Constant.KMUserId, tenant: Constant.TENANT);

                //文件草稿傳出用
                byte[] bytesDraftAcquireResult = null;
                targetDraftUrl = string.Format(targetDraftUrl, parentFolderId);
                try
                {
                    client2.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    bytesDraftAcquireResult = client2.DownloadData(targetDraftUrl);                 //執行取得文件草稿API
                    theAcquireDraftResult = Encoding.UTF8.GetString(bytesDraftAcquireResult);       //將執行API取回的結果轉成字串
                }
                catch (Exception e) { theAcquireDraftResult = e.Message; }

                string theNewAcquireDraftResult = string.Empty;

                //取得文件草稿JSON當中的"data" Object
                Jayrock.Json.JsonObject JOResult = (JsonObject)Jayrock.Json.Conversion.JsonConvert.Import(theAcquireDraftResult);
                string data = Jayrock.Json.Conversion.JsonConvert.ExportToString(JOResult["data"]);

                ViewData["theAcquireDraftResult"] = theAcquireDraftResult;
                var x = Newtonsoft.Json.JsonConvert.SerializeObject(JOResult["data"], Newtonsoft.Json.Formatting.Indented);
                ViewData["JsonStringIndent"] = x;

                // 建立子文件夾
                JObject jobjDraftResult = JObject.Parse(theAcquireDraftResult);
                JObject newKMFolder = UpdateKMDraftSubFolder(jobjDraftResult, newKMSubFolderName);
                var y = Newtonsoft.Json.JsonConvert.SerializeObject(newKMFolder, Newtonsoft.Json.Formatting.Indented);
                ViewData["JsonNewKMFolder"] = y;

                string resultAddFolder = KMServiceAddFolder(newKMFolder, parentFolderId);
                ViewData["resultAddFolder"] = resultAddFolder;
            }

            spkmMappingList = spfpm.GetSharepointKM_FolderPathMappin();

            return View(spkmMappingList);
        }

        private JObject UpdateKMDraftSubFolder(JObject rss, string displayName)
        {
            string jsonString = string.Empty;
            JObject channel = (JObject)rss["data"];
            channel["FolderId"] = 0;
            channel["DisplayName"] = displayName;
            //channel["DefaultDocumentPrivileges"] = null;
            //channel["FolderPrivileges"] = null;
            
            return rss;
        }

        private string KMServiceAddFolder(JObject rss, string parentFolderId)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(rss);
            byte[] serializedResult = System.Text.Encoding.UTF8.GetBytes(json);

            string targetNewFolderUrl = KMService.GetServiceUrl(ServiceType.AddFolder, docId: "0", userId: Constant.KMUserId, tenant: Constant.TENANT);

            targetNewFolderUrl = string.Format(targetNewFolderUrl, parentFolderId);

            string theAcquireResult = string.Empty;
            string result = string.Empty;
            // 建立 WebClient
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    // 指定 WebClient 編碼
                    webClient.Encoding = Encoding.UTF8;
                    // 指定 WebClient 的 Content-Type header
                    //webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    //webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    // 指定 WebClient 的 authorization header
                    //webClient.Headers.Add("authorization", "token {apitoken}");
                    // 執行 PUT 動作
                    theAcquireResult = webClient.UploadString(targetNewFolderUrl, WebRequestMethods.Http.Put, json);

                    Jayrock.Json.JsonObject JOResult = (JsonObject)Jayrock.Json.Conversion.JsonConvert.Import(theAcquireResult);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(JOResult, Newtonsoft.Json.Formatting.Indented);
                }
            }
            catch(WebException we)
            {
                //result = e.Message;
                using (StreamReader sr =
                new StreamReader(we.Response.GetResponseStream()))
                {
                    //實務上可將錯誤資訊網頁寫入Log檔備查，
                    //此處只將錯誤訊息完整傳回當示範
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        private string GetParameterString()
        {
            string guid = Guid.NewGuid().ToString();
            return "guid=" + guid
                + "&format=" + Constant.DataFormat
                + "&tid=0"
                + "&who=" + Constant.KMUserId
                + "&api_key=" + Constant.API_Key;
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