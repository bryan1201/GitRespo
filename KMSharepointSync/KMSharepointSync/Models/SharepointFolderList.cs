using Jayrock.Json;
using Jayrock.Json.Conversion;
using Microsoft.SharePoint.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Web;

namespace KMSharepointSync.Models
{
    public class SharepointFolderList
    {
        private const string KMPRDEnvironment = "PRD";
        private Web _web;
        private string GlobalCurrentCategoryId = "1";
        private string JsonAcquireDraftFolderResult = string.Empty;
        private string JsonResultAddFolder = string.Empty;
        private string JsonAcquireDraftFolderResultIndent = string.Empty;

        private List<SharepointFolder> SPFolderList { get; set; }
        private List<SharepointFile> SPFileList { get; set; }

        public SharepointFolderList()
        {
            SPFolderList = new List<SharepointFolder>();
            SPFileList = new List<SharepointFile>();
        }

        public SharepointFolderList(Web web, List<ListItem> src_spfolderlist)
        {
            _web = web;
            SPFolderList = new List<SharepointFolder>();
            SPFileList = new List<SharepointFile>();
            UpdateSPFolderList(src_spfolderlist);
            //DAOTmpSharePointFolder(SPFolderList);
        }

        private void UpdateSPFolderList(List<ListItem> splist)
        {
            foreach (ListItem listfolder in splist)
            {
                SharepointFolder fd = new SharepointFolder();
                fd.Url = _web.Url;
                fd.ServerRelativeUrl = _web.ServerRelativeUrl;
                fd.Id = listfolder.FieldValues["UniqueId"].ToString();
                fd.Name = listfolder.FieldValues["FileLeafRef"].ToString();
                fd.Path = listfolder.FieldValues["FileRef"].ToString();
                fd.Parent = listfolder.FieldValues["ParentUniqueId"].ToString();

                SPFolderList.Add(fd);
            }
        }

        private void UpdateSPFileList(List<ListItem> spfilelist)
        {
            foreach (ListItem item in spfilelist)
            {
                SharepointFile sf = new SharepointFile();

                sf.UniqueId = item.FieldValues["UniqueId"].ToString();
                sf.ParentUniqueId = item.FieldValues["ParentUniqueId"].ToString();
                sf.ServerRedirectedEmbedUri = item.ServerRedirectedEmbedUri;
                sf.FileLeafRef = item.FieldValues["FileLeafRef"].ToString();
                sf.FileDirRef = item.FieldValues["FileDirRef"].ToString();
                sf.FileRef = item.FieldValues["FileRef"].ToString();
                sf.Author = ((FieldUserValue)item.FieldValues["Author"]).LookupValue;
                sf.Editor = ((FieldUserValue)item.FieldValues["Editor"]).LookupValue;
                sf.Created = ((DateTime)item.FieldValues["Created"]).ToString("yyyyMMddHHmmss");
                sf.Modified = ((DateTime)item.FieldValues["Modified"]).ToString("yyyyMMddHHmmss");
                SPFileList.Add(sf);

                /*
                    ((FieldUserValue)item.FieldValues["Author"]).LookupValue
                    "Jiang.Joe 蔣鎧駿 IEC1"

                    ((DateTime)item.FieldValues["Created"]).ToString()
                    "2024/2/6 上午 08:29:01"

                    ((DateTime)item.FieldValues["Modified"]).ToString()
                    "2024/2/6 上午 08:29:01"
                */
            }
        }

        public IEnumerable<SharepointKM_FolderPathMapping> AddKMFolder(string parentFolderId, string newKMSubFolderName)
        {

            SharepointKM_FolderPathMapping spkm = new SharepointKM_FolderPathMapping();
            IEnumerable<SharepointKM_FolderPathMapping> spkmMappingList = spkm.GetSharepointKM_FolderPathMapping();

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

            spkmMappingList = spkm.GetSharepointKM_FolderPathMapping();

            return spkmMappingList;
        }

        public string GetJsonAcquireDraftFolderResult()
        {
            return JsonAcquireDraftFolderResult;
        }
        public string GetJsonResultAddFolder()
        {
            return JsonResultAddFolder;
        }

        public string GetJsonAcquireDraftFolderResultIndent()
        {
            return JsonAcquireDraftFolderResultIndent;
        }

        private Newtonsoft.Json.Linq.JObject UpdateKMDraftSubFolder(Newtonsoft.Json.Linq.JObject rss, string displayName)
        {
            string jsonString = string.Empty;
            Newtonsoft.Json.Linq.JObject channel = (Newtonsoft.Json.Linq.JObject)rss["data"];
            channel["FolderId"] = 0;
            channel["DisplayName"] = displayName;

            return rss;
        }

        private string KMServiceAddFolder(Newtonsoft.Json.Linq.JObject rss, string parentFolderId)
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

        public void AddKMFile(SharepointKM_FilePathMapping spkmfilepath)
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

        private string WebCall_GetCategoryInfo(string categoryList)
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

        public void GetSharepointFolder(string url, string kmadm, string kmpwd, string spSharedDocLib = "文件", int kmRootFolderId = 39575)
        {
            //Web web;
            string _url = @"https://inventeccorp.sharepoint.com/sites/msteams_a0f382_290647/";
            string kmadmpassword = kmpwd;//"T@aiPei+km0988";
            string kmadmaccount = kmadm;
            using (ClientContext clientContext = new ClientContext(_url))
            {
                SecureString passWord = new SecureString();
                foreach (char c in kmadmpassword.ToCharArray()) passWord.AppendChar(c);
                clientContext.Credentials = //System.Net.CredentialCache.DefaultCredentials;
                             new SharePointOnlineCredentials(kmadmaccount, passWord);
                _web = clientContext.Web;
                var lists = clientContext.LoadQuery(_web.Lists.Where(l => l.BaseType == BaseType.DocumentLibrary));
                clientContext.ExecuteQuery();
                clientContext.RequestTimeout = -1;

                List list = clientContext.Web.Lists.GetByTitle(spSharedDocLib); //spSharedDocLib = "文件";

                clientContext.Load(list.Fields);

                // We must call ExecuteQuery before enumerate list.Fields.
                clientContext.ExecuteQuery();

                CamlQuery camlQuery = new CamlQuery();
                List<ListItem> listFiles = new List<ListItem>();
                List<ListItem> listFolders = new List<ListItem>();
                int iPage = 0;

                listFiles = GetAllItems(iPage, clientContext, list, camlQuery, listFiles, listFolders);

                clientContext.Load(_web);
                clientContext.Load(_web, x => x.HasUniqueRoleAssignments);
                clientContext.ExecuteQuery();

                UpdateSPFolderList(listFolders);
                UpdateSPFileList(listFiles);

                DAO dbaccess = new DAO(KMPRDEnvironment);

                dbaccess.RenewTmpSharePointFolder(splist: SPFolderList);

                string ParentUniqueId = listFolders[0].FieldValues["ParentUniqueId"].ToString().Replace("{", "").Replace("}", "");
                dbaccess.UpdateSharepointFolderPath(rootspfolderId: ParentUniqueId);
                dbaccess.RenewSharePointFile(spfilelist: SPFileList);
                dbaccess.RenewKMFolderPath(rootkmfolderId: kmRootFolderId); //kmRootFolderId= 39575
            }
            //return _web;
        }

        public List<ListItem> GetAllItems(int page, ClientContext Context, List list, CamlQuery camlQuery, List<ListItem> ListFiles, List<ListItem> ListFolders)
        {
            camlQuery.ViewXml =
            @"< View Scope = 'RecursiveAll'>
                < Query >
                    <Where>
                  </Where>
                    <OrderBy>
                        <FieldRef Name='ID' />
                    </OrderBy>
                </ Query >
            </ View >";

            ListItemCollection AllItems = list.GetItems(camlQuery);
            try
            {
                Context.Load(AllItems);
                Context.ExecuteQuery();
                foreach (ListItem item in AllItems)
                {
                    if (item.FileSystemObjectType == FileSystemObjectType.File)
                    {
                        ListFiles.Add(item);
                        /*
                         * item.ServerRedirectedEmbedUri
                            "https://inventeccorp.sharepoint.com/sites/msteams_a0f382_290647/_layouts/15/Embed.aspx?UniqueId=0b03ef65-8a88-4436-a0f0-c86a6ef93f25"
                         * https://inventeccorp.sharepoint.com/sites/msteams_a0f382_290647/Shared Documents/DCAIC智慧醫材電子系統/081_E_管理審查會議紀錄(機密)/1_ASAICMP004-F002-E112001  管理審查會議紀錄.pdf
                         * item.FieldValues["FileLeafRef"]
                            "1_ASAICMP004-F002-E112001  管理審查會議紀錄.pdf"

                            item.FieldValues["FileDirRef"]
                            "/sites/msteams_a0f382_290647/Shared Documents/DCAIC智慧醫材電子系統/081_E_管理審查會議紀錄(機密)"
                            
                            item.FieldValues["FileRef"]
                            "/sites/msteams_a0f382_290647/Shared Documents/DCAIC智慧醫材電子系統/081_E_管理審查會議紀錄(機密)/1_ASAICMP004-F002-E112001  管理審查會議紀錄.pdf"
                         
                            ((FieldUserValue)item.FieldValues["Author"]).LookupValue
                            "Jiang.Joe 蔣鎧駿 IEC1"

                            ((DateTime)item.FieldValues["Created"]).ToString()
                            "2024/2/6 上午 08:29:01"

                            ((DateTime)item.FieldValues["Modified"]).ToString()
                            "2024/2/6 上午 08:29:01"
                         */
                    }

                    if (item.FileSystemObjectType == FileSystemObjectType.Folder)
                    {
                        if (!ListFolders.Contains(item))
                            ListFolders.Add(item);
                    }

                    if (item.FileSystemObjectType == FileSystemObjectType.Folder)
                    {
                        camlQuery.FolderServerRelativeUrl = item.FieldValues["FileRef"].ToString();

                        if (page < 2000)
                            GetAllItems(page + 1, Context, list, camlQuery, ListFiles, ListFolders);
                    }
                }
            }
            catch
            { //do nothing!
            }

            return ListFiles;
        }
        public List<SharepointFolder> GetSPFolderList()
        {
            return this.SPFolderList;
        }
        public List<SharepointFile> GetSPFileList()
        {
            return this.SPFileList;
        }
        public string GetWebUrl()
        {
            return _web.Url;
        }

        public string GetServerRelativeUrl()
        {
            return _web.ServerRelativeUrl;
        }
        public string GetWebTitle()
        {
            return _web.Title;
        }
        public string GetWebDescription()
        {
            return _web.Description;
        }

    }
}