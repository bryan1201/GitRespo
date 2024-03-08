using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Net;
using System.Text;
using Jayrock.Json;
using Jayrock.Json.Conversion;

using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;

using System.Data;

/// <summary>
/// Summary description for CreateNewDoc
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class CreateNewDoc : System.Web.Services.WebService
{


    private string DataFormat = "JSON";                             // (必須)API資料傳輸格式，建議預設使用JSON
    private string userid = ConfigurationManager.AppSettings["userid"];//"IEC940480";                                // (必須)KM系統中有權限讀寫的帳號，建議使用系統管理者帳號
    //private string API_Key = "096481b7b31c419ab91423ea619cd486";    // (必須)KM系統中已註冊並啟用的API Key
    private string API_Key = ConfigurationManager.AppSettings["API_Key"];    // (必須)KM系統中已註冊並啟用的API Key


    private string KMUrl = "http://km.iec.inventec/ESP/api/";
    // "http://vitalskm.gss.com.tw/pdca/api/";              // (必須)KM Server Site的API虛擬目錄URL路徑

    private string tenant = "psg"; //要改PSG使用20230117

    private string GlobalFolderId = "";//"35330";//要改FolderID使用20230117   // 文件夾ID (新增文件時，必須使用傳入的參數之一)
    private string GlobalDocClassId = "1";                          // 知識類型ID (新增文件時，必須使用傳入的參數之一)
    private string UploadCategoryId = "5791257";                     // 分類主題項目ID (新增文件時，使用傳入的參數之一，如有多個請用,隔開)

    private string UploadFileName1 = @"\\10.1.251.134\pu\TEST\BU1_HP_ITO_SA_20221220-172226.xlsx";  //"\\testUpload2.txt";   // 附件檔案名稱1 (新增文件時，使用傳入的參數之一)
    private string UploadFileName2 = "\\testUpload.txt";   // 附件檔案名稱2 (新增文件時，使用傳入的參數之一)

    private string GlobalSearchKeyword = "KM";                      // 「搜尋取回一筆」&「搜尋取回全部結果」測試功能中，若文字方塊空白時，預設進行查詢所使用的關鍵字
    private string GlobalTag = "15433732";                           // 取某文件標籤、取得熱門標籤 兩個測試功能會用到的標籤(Tag)ID

    private string GlobalCurrentDocumentId = "";             // 暫時無作用,不需要去動它
    private string GlobalCurrentCategoryId = "1";                   // 暫時無作用,不需要去動它

    private string ErrMessage = "";

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
        UpdateDocumentFiles
    }

    public CreateNewDoc()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string CreateNew(string FolderID,string SourceFile,string DocName)
    {


        GlobalFolderId = FolderID;
        string thePutFileResult = "";
        string theAcquireDraftResult = "";
        string theNewDocumentResult = "";

        #region (1) PUT檔案
        WebClient client = new WebClient();
        client.Encoding = Encoding.UTF8;
        //注意參數是接在QueryString內
        string targetUrl = GetServiceUrl(ServiceType.UploadFile) + GetParameterString();
        string filename = SourceFile;// UploadFileName1;// Application.StartupPath + UploadFileName1;
                                     //string filename2 = UploadFileName2;//Application.StartupPath +

        //FileInfo fi2 = new FileInfo(filename2);
        FileInfo fi = null;
        DataSet dsMO_FP;
        string sql = "";

        SourceFile = SourceFile.Replace('\'', '`');
        DocName = DocName.Replace('\'', '`');

        string WebUrl = "https://iec1-b2bapp.iec.inventec/WS_Call_KMAPI/CreateNewDoc.asmx/CreateNew?FolderID="+FolderID+"&SourceFile="+SourceFile + "&DocName="+DocName;

       
        //上傳第一個檔案
        try
        {
            fi = new FileInfo(filename);
            client.Headers.Add("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(fi.Name) + "\"");
            client.Headers.Add("Content-Type", "application/octet-stream");
            byte[] result = client.UploadFile(targetUrl, "PUT", filename);
            thePutFileResult = Encoding.UTF8.GetString(result);

            //記錄回傳Log,避免有錯誤描述
            sql = "Insert into [KMDBAPIPRD].dbo.[WS_Call_KMAPILog] ([FolderID],[SourceFile],[DocName],RegionType,WSResultMessage,Flag,WSUri,CDT) VALUES('";

            sql = sql + FolderID + "','" + SourceFile + "','" + DocName + "','SUCCESS_REGION1','" + thePutFileResult.Replace('\'', '`') + "','1','" + WebUrl+"','"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

            dsMO_FP = DAO.sqlCmdDataSetSP("KMConnStr", sql);
        }
        catch (Exception ex)
        {
            string err = "CreateNewDocument(1.1) (" + filename + ")  " + ex.ToString().Replace('\'', '`');

            sql = "Insert into [KMDBAPIPRD].dbo.[WS_Call_KMAPILog] ([FolderID],[SourceFile],[DocName],RegionType,WSResultMessage,Flag,WSUri,CDT) VALUES('";

            sql = sql + FolderID + "','" + SourceFile + "','" + DocName + "','FAIL_REGION1','" + err + "','1','" + WebUrl + "','"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

            dsMO_FP = DAO.sqlCmdDataSetSP("KMConnStr", sql);
            //        return err;
        }
        
        /*上傳第二個檔案
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
            return err;
        }*/

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
        if (filename != "")
        {
            nvc.Add("master_filename", HttpUtility.UrlEncode(fi.Name)); //指定主要附檔名稱
        }
        nvc.Add("source_type", "f");                                //附件型態，在此固定為"f"
        nvc.Add("folder_id", GlobalFolderId);                       //文件夾ID
        nvc.Add("document_class_id", GlobalDocClassId);             //知識類型ID
        nvc.Add("sourcefiles", thePutFileResult);                   //第一步上傳檔案後KM回傳的JSON
        nvc.Add("ActivationDatetime", "552877632000000000");                   //ValidDate
        nvc.Add("DeactivationDatetime", "553877632000000000");                   //ValidDate

        //nvcNewDocument.Add("ActivationDatetime", "1673998342000");
        //nvcNewDocument.Add("DeactivationDatetime", "1703799000000");
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

            sql = "Insert into [KMDBAPIPRD].dbo.[WS_Call_KMAPILog] ([FolderID],[SourceFile],[DocName],RegionType,WSResultMessage,Flag,WSUri,CDT) VALUES('";

            sql = sql + FolderID + "','" + SourceFile + "','" + DocName + "','SUCCESS_REGION2','" + theAcquireDraftResult.Replace('\'', '`') + "','1','" + WebUrl + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

            dsMO_FP = DAO.sqlCmdDataSetSP("KMConnStr", sql);
        }
        catch (Exception ex)
        {
            string err = "CreateNewDocument(2.1) " + ex.ToString();

            sql = "Insert into [KMDBAPIPRD].dbo.[WS_Call_KMAPILog] ([FolderID],[SourceFile],[DocName],RegionType,WSResultMessage,Flag,WSUri,CDT) VALUES('";

            sql = sql + FolderID + "','" + SourceFile + "','" + DocName + "','FAIL_REGION2','" + err.Replace('\'', '`') + "','1','" + WebUrl + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

            dsMO_FP = DAO.sqlCmdDataSetSP("KMConnStr", sql);
            return err;
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

        string fieldTitle = DocName; //"測試API文件-" + DateTime.Now.ToString("MM-dd");
        string fieldDescription = DocName;// "摘要(JSON)";


        foreach (JsonObject jo in JADocumentAttributes)
        {
            if (jo["Name"].ToString() == "Title")
            {
                jo["Value"] =  fieldTitle;         //填入meta欄位值 文件標題jo["Value"] + "-" +
                jo["ValidateEmpty"] = "true";         //20230119
                //jo["ActivationDatetime"] = "552877632000000000";// "2023/01/19";         //20230119
                //jo["DeactivationDatetime"] = "553877632000000000";//"2024/01/19";         //20230119
            }
            else if (jo["Name"].ToString() == "Description")
            {
                jo["Value"] =  fieldDescription;   //填入meta欄位值 文件摘要
                //jo["ValidateEmpty"] = "true";         //20230119                
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

        //20230119
      //  nvcNewDocument.Add("ActivationDatetime", "552877632000000000");
      //  nvcNewDocument.Add("DeactivationDatetime", "553877632000000000");



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
            //     MessageBox.Show("文件新增完成！");
            sql = "Insert into [KMDBAPIPRD].dbo.[WS_Call_KMAPILog] ([FolderID],[SourceFile],[DocName],RegionType,WSResultMessage,Flag,WSUri,CDT) VALUES('";

            sql = sql + FolderID + "','" + SourceFile + "','" + DocName + "','SUCCESS_REGION3','" + theNewDocumentResult.Replace('\'', '`') + "','1','" + WebUrl + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

            dsMO_FP = DAO.sqlCmdDataSetSP("KMConnStr", sql);
        }
        catch (Exception ex)
        {
            string err = "CreateNewDocument(3.1) " + ex.ToString();
            sql = "Insert into [KMDBAPIPRD].dbo.[WS_Call_KMAPILog] ([FolderID],[SourceFile],[DocName],RegionType,WSResultMessage,Flag,WSUri,CDT) VALUES('";

            sql = sql + FolderID + "','" + SourceFile + "','" + DocName + "','FAIL_REGION3','" + err.Replace('\'', '`') + "','1','" + WebUrl + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";

            dsMO_FP = DAO.sqlCmdDataSetSP("KMConnStr", sql);
        }
        #endregion
        #endregion

        return "001:Success！FolderID:" + FolderID +";SourceFile:"+ SourceFile +";DocName:"+ DocName;

    }

    ///相關函數
    ///
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
                ServiceUrl = GetKmUrl() + "search/advancedresult" + "?shell=true&tid=0&api_key=" + API_Key + "&who=" + userid + "&format=" + DataFormat + "&tenant=" + tenant + "&";
                break;
            case ServiceType.DeleteDocument:
                ServiceUrl = GetKmUrl() + "document/delete/";//+ this.txtDeleteDocID.Text.Trim() + "?shell=true&tenant=" + tenant + "&";
                break;
            case ServiceType.GetDocument:
                ServiceUrl = GetKmUrl() + "document/" + GlobalCurrentDocumentId.ToString() + "?shell=true&tenant=" + tenant + "&";
                //document /{ documentId}?version_number ={ versionNumber}
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
            case ServiceType.UpdateDocumentFiles:
                ServiceUrl = GetKmUrl() + "document/files/update/" + GlobalCurrentDocumentId.ToString() + "?shell=true&tenant=" + tenant + "&";
                //新增上傳檔案測試

                //document / files / update /{ documentId}?format = json & tid = 0 & who = IEC940480 & tenant = psg & pi = 0 & ps = 10 & api_key = 096481b7b31c419ab91423ea619cd486
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
            string serviceUrl = GetServiceUrl(ServiceType.GetCategoryInfo) + GetParameterString();

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

}
