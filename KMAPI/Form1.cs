using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jayrock.Json;
using System.Net;
using Jayrock.Json.Conversion;
using System.IO;
using System.Web;
using System.Collections;
using System.Collections.Specialized;

namespace API教育訓練
{
    public partial class Form1 : Form
    {
        private string DataFormat = "JSON";                             // (必須)API資料傳輸格式，建議預設使用JSON
        private string userid = "admin";                                // (必須)KM系統中有權限讀寫的帳號，建議使用系統管理者帳號
        private string API_Key = "1722e63fda0740e4aff711836d9ca0bc";    // (必須)KM系統中已註冊並啟用的API Key
        private string KMUrl = "http://vitalskm.gss.com.tw/pdca/api/";              // (必須)KM Server Site的API虛擬目錄URL路徑

        private string tenant = "default";

        private string GlobalFolderId = "14647590";                            // 文件夾ID (新增文件時，必須使用傳入的參數之一)
        private string GlobalDocClassId = "1";                          // 知識類型ID (新增文件時，必須使用傳入的參數之一)
        private string UploadCategoryId = "5791257";                     // 分類主題項目ID (新增文件時，使用傳入的參數之一，如有多個請用,隔開)
        private string UploadFileName1 = "\\Upload File Test 01.ppt";   // 附件檔案名稱1 (新增文件時，使用傳入的參數之一)
        private string UploadFileName2 = "\\Upload File Test 01.doc";   // 附件檔案名稱2 (新增文件時，使用傳入的參數之一)

        private string GlobalSearchKeyword = "KM";                      // 「搜尋取回一筆」&「搜尋取回全部結果」測試功能中，若文字方塊空白時，預設進行查詢所使用的關鍵字
        private string GlobalTag = "15433732";                           // 取某文件標籤、取得熱門標籤 兩個測試功能會用到的標籤(Tag)ID

        private string GlobalCurrentDocumentId = "";             // 暫時無作用,不需要去動它
        private string GlobalCurrentCategoryId = "1";                   // 暫時無作用,不需要去動它

        private string ErrMessage = "";

        public Form1()
        {
            InitializeComponent();
        }

        //建立新文件
        private void button1_Click(object sender, EventArgs e)
        {
            string thePutFileResult = "";
            string theAcquireDraftResult = "";
            string theNewDocumentResult = "";

            #region (1) PUT檔案
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            //注意參數是接在QueryString內
            string targetUrl = GetServiceUrl(ServiceType.UploadFile) + GetParameterString();
            string filename = Application.StartupPath + UploadFileName1;
            string filename2 = Application.StartupPath + UploadFileName2;
            FileInfo fi = new FileInfo(filename);
            FileInfo fi2 = new FileInfo(filename2);


            //上傳第一個檔案
            try
            {

                client.Headers.Add("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(fi.Name) + "\"");
                client.Headers.Add("Content-Type", "application/octet-stream");
                byte[] result = client.UploadFile(targetUrl, "PUT", filename);
                thePutFileResult = Encoding.UTF8.GetString(result);

            }
            catch (Exception ex)
            {
                string err = "CreateNewDocument(1.1) (" + filename + ")  " + ex.ToString();
                return;
            }
            //上傳第二個檔案
            try
            {
                client.Headers.Add("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(fi2.Name) + "\"");
                client.Headers.Add("Content-Type", "application/octet-stream");
                byte[] result = client.UploadFile(targetUrl, "PUT", filename2);
                thePutFileResult = Encoding.UTF8.GetString(result);
            }
            catch (Exception ex)
            {
                string err = "CreateNewDocument(1.2) (" + filename2 + ")  " + ex.ToString();
                return;
            }

            #endregion

            #region (2) 要求文件草稿 AcquireNewDocumentDraft

            #region 準備參數
            WebClient client2 = new WebClient();
            client2.Encoding = Encoding.UTF8;
            string targetDraftUrl = GetServiceUrl(ServiceType.AcquireDocumentDraft);

            ArrayList al = new ArrayList();
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("tid", "0");
            string guid = Guid.NewGuid().ToString();
            nvc.Add("guid", guid);                                      //語法產生的Guid值
            nvc.Add("api_key", API_Key);                                //KM API Key
            nvc.Add("who", userid);                                     //指定文件建立者帳號
            nvc.Add("format", DataFormat);                              //資料傳輸格式(基本上就是"JSON")
            nvc.Add("master_filename", HttpUtility.UrlEncode(fi.Name)); //指定主要附檔名稱
            nvc.Add("source_type", "f");                                //附件型態，在此固定為"f"
            nvc.Add("folder_id", GlobalFolderId);                       //文件夾ID
            nvc.Add("document_class_id", GlobalDocClassId);             //知識類型ID
            nvc.Add("sourcefiles", thePutFileResult);                   //第一步上傳檔案後KM回傳的JSON

            string uploadData = "";
            foreach (string k in nvc.Keys)
            {
                al.Add(k + "=" + nvc[k]);
            }
            uploadData = string.Join("&", (string[])al.ToArray(typeof(string)));
            #endregion

            #region 執行
            //文件草稿傳入用
            byte[] bytesDraftAcquire = Encoding.UTF8.GetBytes(uploadData);
            //文件草稿傳出用
            byte[] bytesDraftAcquireResult = null;

            try
            {
                client2.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                bytesDraftAcquireResult = client2.UploadData(targetDraftUrl, bytesDraftAcquire);    //執行取得文件草稿API
                theAcquireDraftResult = Encoding.UTF8.GetString(bytesDraftAcquireResult);           //將執行API取回的結果轉成字串
            }
            catch (Exception ex)
            {
                string err = "CreateNewDocument(2.1) " + ex.ToString();
                return;
            }
            #endregion

            #endregion

            #region (3) 新增文件 SubmitNewDocument

            #region 針對指定文件類型欄位進行填值的動作
            string theNewAcquireDraftResult = string.Empty;
           
            //取得文件草稿JSON當中的"data" Object
            JsonObject JOResult = (JsonObject)JsonConvert.Import(theAcquireDraftResult);
            string data = JsonConvert.ExportToString(JOResult["data"]);

            //取得文件草稿JSON當中的"DocumentAttributes" Object
            JsonObject JOData = (JsonObject)JsonConvert.Import(data);
            string DocumentAttributes = JOData["DocumentAttributes"].ToString();
            JsonArray JADocumentAttributes = (JsonArray)JsonConvert.Import(DocumentAttributes);

            string fieldTitle = "測試API文件-標題(JSON)1";
            string fieldDescription = "測試API文件-摘要(JSON)";

            foreach (JsonObject jo in JADocumentAttributes)
            {
                if (jo["Name"].ToString() == "Title")
                {
                    jo["Value"] = jo["Value"] + "-" + fieldTitle;         //填入meta欄位值 文件標題
                }
                else if (jo["Name"].ToString() == "Description")               
                {
                    jo["Value"] = jo["Value"] + "-" + fieldDescription;   //填入meta欄位值 文件摘要
                }
                else
                {
                }
            }

            JOData["DocumentAttributes"] = JADocumentAttributes;        //將填完值的meta欄位結果指定回給JSON的"DocumentAttributes" Object
            JOResult["data"] = JOData;
            theNewAcquireDraftResult = JsonConvert.ExportToString(JOResult);

            
            #endregion

            #region 準備參數
            WebClient client3 = new WebClient();
            client3.Encoding = Encoding.UTF8;
            string targetNewDocumentUrl = GetServiceUrl(ServiceType.SubmitNewDocument);

            ArrayList alNewDocument = new ArrayList();
            NameValueCollection nvcNewDocument = new NameValueCollection();

           
            nvcNewDocument.Add("tid", "0");
            guid = Guid.NewGuid().ToString();
            nvcNewDocument.Add("guid", guid);                                                                           //語法產生的Guid值
            nvcNewDocument.Add("api_key", API_Key);                                                                     //KM API Key
            nvcNewDocument.Add("who", userid);                                                                          //指定文件建立者帳號
            nvcNewDocument.Add("format", DataFormat);                                                                   //資料傳輸格式(基本上就是"JSON")
            nvcNewDocument.Add("folder_id", GlobalFolderId);                                                            //文件夾ID
            nvcNewDocument.Add("documentdetailinfo", HttpUtility.UrlEncode(theNewAcquireDraftResult));                  //取回的草稿(draft)填完meta欄位值後的JSON
            nvcNewDocument.Add("privilegeinfo", "[]");                                                                  //文件權限JSON (本範例中沒有特別指定)
            if (GlobalCurrentCategoryId != string.Empty)
            {
                nvcNewDocument.Add("categoryinfo", HttpUtility.UrlEncode(WebCall_GetCategoryInfo(UploadCategoryId)));   //分類主題JSON
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
            #endregion

            #region 執行
            //新增文件傳入用
            byte[] bytesNewDocument = Encoding.UTF8.GetBytes(uploadDataNewDocument);
            //新增文件傳出用
            byte[] bytesNewDocumentResult = null;

            try
            {
                client3.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                bytesNewDocumentResult = client3.UploadData(targetNewDocumentUrl, bytesNewDocument);
                theNewDocumentResult = Encoding.UTF8.GetString(bytesNewDocumentResult);
                MessageBox.Show("文件新增完成！");
            }
            catch (Exception ex)
            {
                string err = "CreateNewDocument(3.1) " + ex.ToString();
            }
            #endregion

            #endregion
            
        }

        private string GetParameterString()
        {
            string guid = Guid.NewGuid().ToString();
            return "guid=" + guid
                + "&format=" + DataFormat
                + "&tid=0"
                + "&who=" + userid
                + "&api_key=" + API_Key;
        }
        private string WebPost(string requestUrl, string parameters, string uploadData)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            byte[] bytesDraftAcquire = Encoding.UTF8.GetBytes(uploadData);
            byte[] bytesAcquireResult = null;
            try
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                bytesAcquireResult = client.UploadData(requestUrl + parameters, "POST", bytesDraftAcquire);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Encoding.UTF8.GetString(bytesAcquireResult);
        }

        private JsonObject AcquireNewDocumentDraft(string guid, int folderId, int documentClassId,
            string masterFileName, string sourceFiles)
        {
            string requestUrl = "http://vitalskm-demo.gss.com.tw/default/api/document/acquirenewdocumentdraft?";
            string parameters = GetParameterString();
            string uploadData = "guid=" + guid +
                "&master_filename=" + masterFileName +
                "&source_type=f" +
                "&folder_id=" + folderId.ToString() +
                "&document_class_id=" + documentClassId.ToString() +
                "&sourcefiles=" + sourceFiles;
            string result = WebPost(requestUrl, parameters, uploadData);
            JsonObject JOResult = (JsonObject)JsonConvert.Import(result);
            if (JOResult["statuscode"].ToString() != "000")
            {
                throw new Exception(JOResult["data"].ToString());
            }
            else
            {
                return JOResult;
            }
        }

        private string GetServiceUrl(ServiceType serviceType)
        {
            string ServiceUrl = "";
            switch (serviceType)
            {
                case ServiceType.GetRootFolder:
                    ServiceUrl = GetKmUrl() + "folder/root/public?shell=true&tenant=" + tenant + "&" ;
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
                    ServiceUrl = GetKmUrl() + "search/advancedresult" + "?shell=true&tid=0&api_key=" + API_Key + "&who=" + userid + "&format=" + DataFormat + "&tenant=" + tenant + "&" ;
                    break;
                case ServiceType.DeleteDocument:
                    ServiceUrl = GetKmUrl() + "document/delete/" + this.txtDeleteDocID.Text.Trim()  + "?shell=true&tenant=" + tenant + "&";
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
                    ServiceUrl = GetKmUrl() + "folder/newdraft/{0}" + "?shell=true&tid=0&pi=0&ps=10&api_key=" + API_Key + "&who=" + userid + "&format=" + DataFormat + "&tenant=" + tenant + "&";
                    break;
                case ServiceType.AddFolder:
                    ServiceUrl = GetKmUrl() + "folder/new/{0}" + "?shell=true&tid=0&api_key=" + API_Key + "&who=" + userid + "&format=" + DataFormat + "&tenant=" + tenant + "&";
                    break;
                default:
                    break;
            }
            return ServiceUrl;
        }

        private string GetKmUrl()
        {
            return KMUrl;
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
                string serviceUrl = GetServiceUrl(ServiceType.GetCategoryInfo) + GetParameterString() ;

                //反序列化用
                string result = "";

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

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            if (SearchText.Text.Trim() != "")
            {
                GlobalSearchKeyword = SearchText.Text.Trim();
            }

            WebClient searchWeb = new WebClient();
            searchWeb.Encoding = Encoding.UTF8;
            string targetDraftUrl = GetServiceUrl(ServiceType.GetSearchExBySimple) + GetParameterString() + "&pi=0&ps=1";

            //反序列化用
            string result = "";

            try
            {
                //接收結果
                result = searchWeb.DownloadString(targetDraftUrl);
                JsonObject JOResult = (JsonObject)JsonConvert.Import(result);
                string data = JsonConvert.ExportToString(JOResult["data"]);

                JsonResultTxt.Text = data;
                //JsonObject JOCategoryInfo = (JsonObject)JsonConvert.Import(data);
                //joArray.Add(JOCategoryInfo);
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

        private void SearchGetAll_Click(object sender, EventArgs e)
        {
            if (SearchText.Text.Trim() != "")
            {
                GlobalSearchKeyword = SearchText.Text.Trim();
            }

            WebClient searchWeb = new WebClient();
            searchWeb.Encoding = Encoding.UTF8;
            string targetDraftUrl = GetServiceUrl(ServiceType.GetSearchExBySimple) + GetParameterString() ;

            //反序列化用
            string result = "";

            try
            {
                //接收結果
                result = searchWeb.DownloadString(targetDraftUrl);
                JsonObject JOResult = (JsonObject)JsonConvert.Import(result);
                string data = JsonConvert.ExportToString(JOResult["data"]);

                JsonResultTxt.Text = data;
                //JsonObject JOCategoryInfo = (JsonObject)JsonConvert.Import(data);
                //joArray.Add(JOCategoryInfo);
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

        private void GetTag_Click(object sender, EventArgs e)
        {
            if (DocIdOrUserId.Text.Trim() != "")
            {
                GlobalTag = DocIdOrUserId.Text.Trim();
            }

            WebClient searchWeb = new WebClient();
            searchWeb.Encoding = Encoding.UTF8;
            string targetDraftUrl = GetKmUrl() + "tag/document/" + GlobalTag + "?top_n=10" + "&" + GetParameterString();

            //反序列化用
            string result = "";

            try
            {
                //接收結果
                result = searchWeb.DownloadString(targetDraftUrl);
                JsonObject JOResult = (JsonObject)JsonConvert.Import(result);
                string data = JsonConvert.ExportToString(JOResult["data"]);

                JsonResultTxt.Text = data;
                //JsonObject JOCategoryInfo = (JsonObject)JsonConvert.Import(data);
                //joArray.Add(JOCategoryInfo);
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

        private void GetHotTag_Click(object sender, EventArgs e)
        {
            if (DocIdOrUserId.Text.Trim() != "")
            {
                GlobalTag = DocIdOrUserId.Text.Trim();
            }

            WebClient searchWeb = new WebClient();
            searchWeb.Encoding = Encoding.UTF8;
            string targetDraftUrl = GetKmUrl() + "tag/hot/" + GlobalTag + "?" + GetParameterString();

            //反序列化用
            string result = "";

            try
            {
                //接收結果
                result = searchWeb.DownloadString(targetDraftUrl);
                JsonObject JOResult = (JsonObject)JsonConvert.Import(result);
                string data = JsonConvert.ExportToString(JOResult["data"]);

                JsonResultTxt.Text = data;
                //JsonObject JOCategoryInfo = (JsonObject)JsonConvert.Import(data);
                //joArray.Add(JOCategoryInfo);
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

        private void GetAttachment_Click(object sender, EventArgs e)
        {
            if (AttDocID.Text.Trim() == "")
            {
                MessageBox.Show("請輸入欲取得附件的文件編號DocumentID", "",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                return;
            }

            if (AttVerNo.Text.Trim() == "")
            {
                MessageBox.Show("請輸入欲取得附件的文件版本號碼VersionNo", "",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                return;
            }

            if (AttFileName.Text.Trim() == "")
            {
                MessageBox.Show("請輸入欲取得附件的檔案名稱", "",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                return;
            }
            WebClient clientFile = new WebClient();
            clientFile.Encoding = Encoding.UTF8;
            string targetAttUrl = GetKmUrl() + "download2/" + AttDocID.Text.Trim() + "?version_number=" + AttVerNo.Text.Trim() + "&file_name=" + HttpUtility.UrlEncode(AttFileName.Text.Trim()) + "&" + GetParameterString();

            try
            {
                //clientFile.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                //bytesDraftAcquireResult = clientFile.UploadData(targetDraftUrl, bytesDraftAcquire);
                clientFile.DownloadFile(targetAttUrl, Application.StartupPath + "\\1" + AttFileName.Text.Trim());

                MessageBox.Show("下載附檔完成", "",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                //theAcquireDraftResult = Encoding.UTF8.GetString(bytesDraftAcquireResult);
            }
            catch (Exception ex)
            {
                string err = "DownloadAtt(2.1) " + ex.ToString();
                MessageBox.Show(err, "",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                return;
            }
        }


        private void btnAdvSearch_Click(object sender, EventArgs e)
        {
            this.JsonResultTxt.Text = string.Empty; 

            WebClient client2 = new WebClient();
            client2.Encoding = Encoding.UTF8;
            string targetAdvSearchUrl = GetServiceUrl(ServiceType.GetAdvancedResult);

            ArrayList al = new ArrayList();
            NameValueCollection nvc = new NameValueCollection();

            nvc.Add("enabletagsynonyms", "false");
            nvc.Add("enablekeywordsynonyms", "false");
            nvc.Add("containchildcategory", "false");
            nvc.Add("containchildfolder", "false");
            nvc.Add("keyword", this.txtAdvKeyword.Text.Trim());

            if (this.txtAdvFolderID.Text.Trim() != string.Empty )
            {
                nvc.Add("folder", this.txtAdvFolderID.Text.Trim());
            }
                      
            string uploadData = "";
            foreach (string k in nvc.Keys)
            {
                al.Add(k + "=" + nvc[k]);
            }
            uploadData = string.Join("&", (string[])al.ToArray(typeof(string)));


            byte[] bytesAdvSearch = Encoding.UTF8.GetBytes(uploadData);
            byte[] bytesAdvSearchResult = null;
            string stringAdvSearchResult = "";

            try
            {
                client2.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                bytesAdvSearchResult = client2.UploadData(targetAdvSearchUrl, bytesAdvSearch);
                stringAdvSearchResult = Encoding.UTF8.GetString(bytesAdvSearchResult);
            }
            catch (Exception ex)
            {
                string err = "GetAdvancedResult " + ex.ToString();
                return;
            }     
            finally
            {
                this.JsonResultTxt.Text = stringAdvSearchResult;
            }
        }

        private void btnDeleteDoc_Click(object sender, EventArgs e)
        {

            if (txtDeleteDocID.Text.Trim() == string.Empty)
            {
                return;
            }

            string DocDeleteUrl = GetServiceUrl(ServiceType.DeleteDocument) + GetParameterString();

            WebRequest request = WebRequest.Create(DocDeleteUrl);
            request.Method = "DELETE";

            try
            {

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader sr = new StreamReader(response.GetResponseStream());

                string result = sr.ReadToEnd();

                JsonObject JOResult = (JsonObject)JsonConvert.Import(result);
                string data = JsonConvert.ExportToString(JOResult);
                JsonResultTxt.Text = data;

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

        private void btnSearchByID_Click(object sender, EventArgs e)
        {
            if (this.txtSearchByID.Text.Trim() == string.Empty)
            {
                MessageBox.Show("請輸入欲取得文件的編號(DocumentID)", "",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                return;
            }

            //將TextBox中輸入的Document ID填入全域變數GlobalCurrentDocumentId
            GlobalCurrentDocumentId = this.txtSearchByID.Text.Trim();

            WebClient searchWeb = new WebClient();
            searchWeb.Encoding = Encoding.UTF8;
            //組合API呼叫的URI字串
            string targetDraftUrl = GetServiceUrl(ServiceType.GetDocument) + GetParameterString() + "&pi=0&ps=1";

            string result = "";

            try
            {
                //接收結果
                result = searchWeb.DownloadString(targetDraftUrl);
                //利用Jayrock物件的方法將接收結果轉型成JSONObject以便後續的處理
                JsonObject JOResult = (JsonObject)JsonConvert.Import(result);
                //string data = JsonConvert.ExportToString(JOResult["data"]);
                string data = JsonConvert.ExportToString(JOResult);

                JsonResultTxt.Text = data;

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



        private void btnNewFolder_Click_1(object sender, EventArgs e)
        {


            if (this.txtNewFolderParentId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("請輸入父文件夾的Folder ID", "",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                return;
            }

            if (this.txtNewFolderName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("請輸入新文件夾的名稱", "",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                return;
            }


            this.JsonResultTxt.Text = string.Empty;

            #region (1)取得父親文件夾的Draft,準備用來當作新的子文件夾的範本
            WebClient client2 = new WebClient();
            client2.Encoding = Encoding.UTF8;
            //根據父文件夾ID取得新文件夾草稿的API
            string folderDraftUrl = GetServiceUrl(ServiceType.AcquireFolderDraft);
            //要透過webclient的put method上傳到server的資源參數
            byte[] bytesFolderDraft = Encoding.UTF8.GetBytes(string.Empty);
            //接收API執行結果的字串
            string stringFolderDraftResult = "";

            try
            {
                client2.Headers.Add("Content-Type", "application/octet-stream");
                byte[] result = client2.UploadData(string.Format(folderDraftUrl, this.txtNewFolderParentId.Text.Trim()), "PUT", bytesFolderDraft);
                stringFolderDraftResult = Encoding.UTF8.GetString(result);
            }
            catch (Exception ex)
            {
                string err = "GetNewFolderDraftResult ERROR = " + ex.ToString();
                return;
            }
            finally
            {
                this.JsonResultTxt.Text = "GetNewFolderDraftResult OK = " + stringFolderDraftResult;
            }
            #endregion


            #region (2)新的子文件夾範本準備完畢,開始新增文件夾
            //實作Jayrock的JsonObject物件來處理,透過API取回的新文件夾草稿Draft的JSON字串
            JsonObject JOResult = (JsonObject)JsonConvert.Import(stringFolderDraftResult);
            //取出新文件夾草稿Draft的JSON字串中的data區段部分字串
            string data = JsonConvert.ExportToString(JOResult["data"]);
            //把data區段部分的字串轉成JsonObject物件以便處理
            JsonObject JOData = (JsonObject)JsonConvert.Import(data);

            //指定data區段中的新文件夾顯示名稱欄位值
            JOData["DisplayName"] = this.txtNewFolderName.Text.Trim();  // "新文件夾顯示名稱";
            JOResult["data"] = JOData;

            //新文件夾名稱指定完畢,再將JsonObject物件轉回字串
            string newFolderAcquireDraftResult = JsonConvert.ExportToString(JOResult);

            WebClient client3 = new WebClient();
            client3.Encoding = Encoding.UTF8;
            string newFolderUrl = GetServiceUrl(ServiceType.AddFolder);

            //將新文件夾草稿Draft的JSON字串轉成byte[]格式,準備透過webclient的put method上傳到API
            byte[] bytesNewFolder = Encoding.UTF8.GetBytes(newFolderAcquireDraftResult);
            byte[] bytesNewDocumentResult = null;
            string theNewFolderResult = "";

            try
            {
                client3.Headers.Add("Content-Type", "application/octet-stream");
                bytesNewDocumentResult = client3.UploadData(string.Format(newFolderUrl, this.txtNewFolderParentId.Text.Trim()), "PUT", bytesNewFolder);
                theNewFolderResult = Encoding.UTF8.GetString(bytesNewDocumentResult);
            }
            catch (Exception ex)
            {
                string err = "CreateNewFolder ERROR = " + ex.ToString();
            }
            finally
            {
                this.JsonResultTxt.Text = this.JsonResultTxt.Text + "CreateNewFolder OK = " + stringFolderDraftResult;
            }

            #endregion

        }


        
    }
}