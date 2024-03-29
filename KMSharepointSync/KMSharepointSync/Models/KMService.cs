using Jayrock.Json;
using Jayrock.Json.Conversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace KMSharepointSync.Models
{
    public class KMService
    {
        public static String DataFormat = ConfigurationManager.AppSettings["DataFormat"];
        public static string KMUserId = ConfigurationManager.AppSettings["KMUserId"];
        public static string API_Key = ConfigurationManager.AppSettings["API_Key"];
        public static string TENANT = ConfigurationManager.AppSettings["TENANT"]; //psg
        public static string KMPUrl = ConfigurationManager.AppSettings["KMPUrl"];   // (必須)KM Server Site的API虛擬目錄URL路徑         
        private static string GlobalCurrentDocumentId = "";        // 暫時無作用,不需要去動它
        private static string GlobalCurrentCategoryId = "1";       // 暫時無作用,不需要去動它
        private static string GlobalSearchKeyword = "KM";
        private static string JsonResultAddFolder = string.Empty;
        private static string JsonAcquireDraftFolderResult = string.Empty;
        private static string JsonAcquireDraftFolderResultIndent = string.Empty;

        public static string GetServiceUrl(ServiceType serviceType, string docId, string userId, string tenant)
        {
            StringBuilder sb = new StringBuilder();
            string ServiceUrl = "";

            switch (serviceType)
            {
                case ServiceType.GetRootFolder:
                    ServiceUrl = GetKmUrl() + "folder/root/public?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.UploadFile:
                    ServiceUrl = GetKmUrl() + "upload?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AcquireDocumentDraft:
                    ServiceUrl = GetKmUrl() + "document/acquirenewdocumentdraft?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.SubmitNewDocument:
                    ServiceUrl = GetKmUrl() + "document/new?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AllDocumentClass:
                    ServiceUrl = GetKmUrl() + "documentclass/all/enabled?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetCategoryInfo:
                    ServiceUrl = GetKmUrl() + "category/" + GlobalCurrentCategoryId + "?load_path=False&shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetSearchExBySimple:
                    ServiceUrl = GetKmUrl() + "search/ext/simple/" + GlobalSearchKeyword + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetAdvancedResult:
                    ServiceUrl = GetKmUrl() + "search/advancedresult" + "?shell=true&tid=0&api_key=" + API_Key + "&who=" + userId + "&format=" + DataFormat + "&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetDocument:
                    ServiceUrl = GetKmUrl() + "document/" + GlobalCurrentDocumentId.ToString() + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AddUser:
                    ServiceUrl = GetKmUrl() + "user/add" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetUser:
                    ServiceUrl = GetKmUrl() + "user/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetUserBySubjectId:
                    ServiceUrl = GetKmUrl() + "user/exact/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.UpdateUser:
                    ServiceUrl = GetKmUrl() + "user/update" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.DeleteUser:
                    ServiceUrl = GetKmUrl() + "user/delete/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetAllUser:
                    ServiceUrl = GetKmUrl() + "user/all" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.DownloadFile2:
                    ServiceUrl = GetKmUrl() + "download2/{0}" + "?shell=true&tenant=" + tenant + "&";
                    break;
                case ServiceType.AcquireFolderDraft:
                    ServiceUrl = GetKmUrl() + "folder/draft/{0}" + "?shell=true&tid=0&pi=0&ps=10&api_key=" + KMService.API_Key + "&who=" + userId + "&format=" + DataFormat + "&tenant=" + tenant;
                    break;
                case ServiceType.AddFolder:
                    ServiceUrl = GetKmUrl() + "folder/new/{0}" + "?shell=true&tid=0&api_key=" + API_Key + "&who=" + userId + "&format=" + DataFormat + "&tenant=" + tenant + "&";
                    break;
                case ServiceType.GetDocumentFileById:
                    /*
                     http://km.iec.inventec/ESP/api/document/file/10090?version_number=0&format=json&tid=0&who=IEC891652&tenant=psg&pi=0&ps=10&api_key=154e10710ea44cdaaaec9cb4f7910ddc
                     */
                    sb.Append(GetKmUrl());
                    sb.Append("document/file/{0}?version_number=0");
                    sb.Append(string.Format("&format={0}", DataFormat));
                    sb.Append(string.Format("&tid=0&who={0}", userId));
                    sb.Append(string.Format("&tenant={0}", tenant));
                    sb.Append(string.Format("&api_key={0}", API_Key));
                    ServiceUrl = string.Format(sb.ToString(),docId);
                    break;
                default:
                    break;
            }
            return ServiceUrl;
        }

        private static string GetKmUrl()
        {
            return KMPUrl;
        }

        public static void AddKMFile(SharepointKM_FilePathMapping spkmfilepath)
        {
            string UploadFolderId = spkmfilepath.KM_FolderId;
            string DocTitle = spkmfilepath.SP_FileLeafRef;
            string DocRefUrl = spkmfilepath.SP_FileRef;
            string DocDescription = spkmfilepath.KM_Path;
            string Author = spkmfilepath.SP_Author;
            string CreateDate = spkmfilepath.SP_Created;

            if (string.IsNullOrEmpty(UploadFolderId) || string.IsNullOrEmpty(DocTitle) || string.IsNullOrEmpty(DocRefUrl) || string.IsNullOrEmpty(DocDescription) || string.IsNullOrEmpty(Author) || string.IsNullOrEmpty(CreateDate))
                return;
            //string DocTitle = "IEC預防(風控)內部控制制度文件.url";
            //string DocRefUrl = Constant.SharepointOnlineRoot + @"sites/msteams_a0f382_290647/Shared Documents/智慧醫材電子系統/000_公告區(相關說明請見此資料夾)/IEC預防(風控)內部控制制度文件.url";
            //StringBuilder DocDescription = new StringBuilder();
            //DocDescription.Append("/智慧醫材電子系統/000_公告區(相關說明請見此資料夾)/<br>");
            //DocDescription.Append(@"/sites/msteams_a0f382_290647/Shared Documents/智慧醫材電子系統/000_公告區(相關說明請見此資料夾)<br><br>");
            //DocDescription.Append(@"<a href='" + DocRefUrl + "'>" + DocTitle + "</a><br>");
            //string Author = @"Jiang.Joe 蔣鎧駿 IEC1";
            //CreateDate = DateTime.Parse("2024/3/5 13:26:53").ToString("yyyyMMddHHmmss"); //20240305065600

            StringBuilder sbDocDescription = new StringBuilder();
            sbDocDescription.Append(DocDescription);
            sbDocDescription.Append("<br><br>");
            sbDocDescription.Append(@"<a href='" + DocRefUrl + "'>" + DocTitle + "</a>");
            sbDocDescription.Append("<br>");
            DocDescription = sbDocDescription.ToString();

            string theAcquireDraftResult = string.Empty;
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string targetDraftUrl = KMService.GetServiceUrl(ServiceType.AcquireDocumentDraft, docId: "0", userId: Constant.KMUserId, tenant: Constant.TENANT);

            ArrayList al = new ArrayList();
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("tid", "0");
            string guid = Guid.NewGuid().ToString();
            nvc.Add("guid", guid);                                      //語法產生的Guid值
            nvc.Add("api_key", Constant.API_Key);                       //KM API Key
            nvc.Add("who", Constant.KMUserId);                          //指定文件建立者帳號
            nvc.Add("format", Constant.DataFormat);                     //資料傳輸格式(基本上就是"JSON")

            nvc.Add("source_type", "f");                                //附件型態，在此固定為"f"
            nvc.Add("folder_id", UploadFolderId);                       //文件夾ID
            nvc.Add("document_class_id", Constant.GlobalDocClassId84);  //知識類型ID
            nvc.Add("ActivationDatetime", "552877632000000000");        //ValidDate
            nvc.Add("DeactivationDatetime", "553877632000000000");      //ValidDate

            string uploadData = "";
            foreach (string k in nvc.Keys)
            {
                al.Add(k + "=" + nvc[k]);
            }
            uploadData = string.Join("&", (string[])al.ToArray(typeof(string)));
            //文件草稿傳入用
            byte[] bytesDraftAcquire = Encoding.UTF8.GetBytes(uploadData);
            //文件草稿傳出用
            byte[] bytesDraftAcquireResult = null;

            try
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                bytesDraftAcquireResult = client.UploadData(targetDraftUrl, bytesDraftAcquire);    //執行取得文件草稿API
                theAcquireDraftResult = Encoding.UTF8.GetString(bytesDraftAcquireResult);          //將執行API取回的結果轉成字串
            }
            catch
            {
            }

            string theNewAcquireDraftResult = string.Empty;

            //取得文件草稿JSON當中的"data" Object
            JsonObject JOResult = (JsonObject)JsonConvert.Import(theAcquireDraftResult);
            string data = JsonConvert.ExportToString(JOResult["data"]);

            //取得文件草稿JSON當中的"DocumentAttributes" Object
            JsonObject JOData = (JsonObject)JsonConvert.Import(data);
            string DocumentAttributes = JOData["DocumentAttributes"].ToString();
            JsonArray JADocumentAttributes = (JsonArray)JsonConvert.Import(DocumentAttributes);

            foreach (JsonObject jo in JADocumentAttributes)
            {
                switch (jo["Name"].ToString())
                {
                    case "Title":
                        jo["Value"] = DocTitle;                     //填入meta欄位值 文件標題jo["Value"] + "-" +
                        jo["ValidateEmpty"] = "true";               //20230119
                        break;
                    case "Description":
                        jo["Value"] = DocDescription.ToString();    //填入meta欄位值 文件摘要
                        break;
                    case "Summary":
                        jo["Value"] = DocDescription.ToString();    //填入meta欄位值 文件摘要
                        break;
                    case "Author":
                        jo["Value"] = Author;
                        break;
                    case "CreateDate":
                        jo["Value"] = CreateDate;                   //CreateDate.ToString("yyyy/MM/dd HH:mm:ss");
                        break;
                    default:
                        break;
                }
            }

            JOData["DocumentAttributes"] = JADocumentAttributes;        //將填完值的meta欄位結果指定回給JSON的"DocumentAttributes" Object
            JOResult["data"] = JOData;
            theNewAcquireDraftResult = JsonConvert.ExportToString(JOResult);

            WebClient client3 = new WebClient();
            client3.Encoding = Encoding.UTF8;
            string targetNewDocumentUrl = KMService.GetServiceUrl(ServiceType.SubmitNewDocument, docId: "0", userId: Constant.KMUserId, tenant: Constant.TENANT);

            ArrayList alNewDocument = new ArrayList();
            NameValueCollection nvcNewDocument = new NameValueCollection();


            nvcNewDocument.Add("tid", "0");
            guid = Guid.NewGuid().ToString();
            nvcNewDocument.Add("guid", guid);                                                           //語法產生的Guid值
            nvcNewDocument.Add("api_key", Constant.API_Key);                                            //KM API Key
            nvcNewDocument.Add("who", Constant.KMUserId);                                               //指定文件建立者帳號
            nvcNewDocument.Add("format", Constant.DataFormat);                                          //資料傳輸格式(基本上就是"JSON")
            nvcNewDocument.Add("folder_id", UploadFolderId);                                            //文件夾ID
            nvcNewDocument.Add("documentdetailinfo", HttpUtility.UrlEncode(theNewAcquireDraftResult));  //取回的草稿(draft)填完meta欄位值後的JSON
            nvcNewDocument.Add("privilegeinfo", "[]");                                                  //文件權限JSON (本範例中沒有特別指定)
            if (GlobalCurrentCategoryId != string.Empty)
            {
                nvcNewDocument.Add("categoryinfo", HttpUtility.UrlEncode(WebCall_GetCategoryInfo(UploadFolderId)));   //分類主題JSON
            }
            else
            {
                nvcNewDocument.Add("categoryinfo", "[]");
            }
            nvcNewDocument.Add("tags", "[]");                                                                           //標籤(tag)JSON (本範例中沒有特別指定)

            string uploadDataNewDocument = "";
            foreach (string k in nvcNewDocument.Keys)
            {
                alNewDocument.Add(k + "=" + nvcNewDocument[k]);                                                         //將NameValueCollection集合轉成ArrayList
            }
            uploadDataNewDocument = string.Join("&", (string[])alNewDocument.ToArray(typeof(string)));                  //將ArrayList轉成URL字串


            //新增文件傳入用
            byte[] bytesNewDocument = Encoding.UTF8.GetBytes(uploadDataNewDocument);
            //新增文件傳出用
            byte[] bytesNewDocumentResult = null;
            string theNewDocumentResult = string.Empty;
            try
            {
                client3.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                bytesNewDocumentResult = client3.UploadData(targetNewDocumentUrl, bytesNewDocument);
                theNewDocumentResult = Encoding.UTF8.GetString(bytesNewDocumentResult);

            }
            catch (Exception ex)
            {
                string err = "CreateNewDocument(3.1) " + ex.ToString();
            }
        }

        public static IEnumerable<SharepointKM_FolderPathMapping> AddKMFolder(string taskId, string parentFolderId, string newKMSubFolderName)
        {

            SharepointKM_FolderPathMapping spkm = new SharepointKM_FolderPathMapping();
            IEnumerable<SharepointKM_FolderPathMapping> spkmMappingList = spkm.GetSharepointKM_FolderPathMapping(taskId);

            if (string.IsNullOrEmpty(parentFolderId) || string.IsNullOrEmpty(newKMSubFolderName))
            {
                return spkmMappingList;
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

                JsonAcquireDraftFolderResult = theAcquireDraftResult;
                //ViewData["theAcquireDraftResult"] = theAcquireDraftResult;
                var x = Newtonsoft.Json.JsonConvert.SerializeObject(JOResult["data"], Newtonsoft.Json.Formatting.Indented);
                //ViewData["JsonStringIndent"] = x;
                JsonAcquireDraftFolderResultIndent = x;

                // 建立子文件夾，修改文件夾的草稿，填入子文件夾的名稱
                Newtonsoft.Json.Linq.JObject jobjDraftResult = Newtonsoft.Json.Linq.JObject.Parse(theAcquireDraftResult);
                Newtonsoft.Json.Linq.JObject newKMFolder = UpdateKMDraftSubFolder(jobjDraftResult, newKMSubFolderName);
                string resultAddFolder = KMServiceAddFolder(newKMFolder, parentFolderId);
                JsonResultAddFolder = resultAddFolder;
                //ViewData["resultAddFolder"] = resultAddFolder; // 顯示建子文夾的結果及是否完成，或顯示KMAPI傳回的錯誤訊息

            }

            spkmMappingList = spkm.GetSharepointKM_FolderPathMapping(taskId);

            return spkmMappingList;
        }

        private static Newtonsoft.Json.Linq.JObject UpdateKMDraftSubFolder(Newtonsoft.Json.Linq.JObject rss, string displayName)
        {
            string jsonString = string.Empty;
            Newtonsoft.Json.Linq.JObject channel = (Newtonsoft.Json.Linq.JObject)rss["data"];
            channel["FolderId"] = 0;
            channel["DisplayName"] = displayName;

            return rss;
        }

        private static string KMServiceAddFolder(Newtonsoft.Json.Linq.JObject rss, string parentFolderId)
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
                    // 執行 PUT 動作
                    theAcquireResult = webClient.UploadString(targetNewFolderUrl, WebRequestMethods.Http.Put, json);

                    Jayrock.Json.JsonObject JOResult = (JsonObject)Jayrock.Json.Conversion.JsonConvert.Import(theAcquireResult);
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(JOResult, Newtonsoft.Json.Formatting.Indented);
                }
            }
            catch (WebException we)
            {
                //result = e.Message;
                using (StreamReader sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        private static string WebCall_GetCategoryInfo(string categoryList)
        {
            string[] categories = categoryList.Split(',');
            JsonArray joArray = new JsonArray();

            for (int i = 0; i < categories.Length; i++)
            {
                GlobalCurrentCategoryId = categories[i];

                //使用 WebClient 類別向伺服器發出要求
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                string serviceUrl = KMService.GetServiceUrl(ServiceType.GetCategoryInfo, docId: "0", userId: Constant.KMUserId, tenant: Constant.TENANT);

                //反序列化用
                string result = "";
                string ErrMessage = string.Empty;
                try
                {
                    //接收結果
                    result = client.DownloadString(serviceUrl);
                    JsonObject JOResult = (JsonObject)JsonConvert.Import(result);
                    string data = JsonConvert.ExportToString(JOResult["data"]);
                    JsonObject JOCategoryInfo = (JsonObject)JsonConvert.Import(data);
                    joArray.Add(JOCategoryInfo);
                }
                catch (WebException webEx)
                {
                    ErrMessage = webEx.ToString();
                }
                catch (NotSupportedException nsEx)
                {
                    ErrMessage = nsEx.ToString();
                }
                catch (Exception ex)
                {
                    ErrMessage = ex.ToString();
                }
            }

            return JsonConvert.ExportToString(joArray);
        }


    }
}