using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Net;
using System.Text;
using Jayrock.Json;
using Jayrock.Json.Conversion;

using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;

/// <summary>
/// Summary description for DeleteDOC
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class DeleteDOC : System.Web.Services.WebService
{

    private string DataFormat = "JSON";                             // (必須)API資料傳輸格式，建議預設使用JSON
    private string userid = ConfigurationManager.AppSettings["userid"];//"IEC940480";                                // (必須)KM系統中有權限讀寫的帳號，建議使用系統管理者帳號
    //private string API_Key = "096481b7b31c419ab91423ea619cd486";    // (必須)KM系統中已註冊並啟用的API Key
    private string API_Key = ConfigurationManager.AppSettings["API_Key"];    // (必須)KM系統中已註冊並啟用的API Key


    private string KMUrl = "http://km.iec.inventec/ESP/api/"; // (必須)KM Server Site的API虛擬目錄URL路徑

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

    private string ErrMessage = "NO DATA";

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

    public DeleteDOC()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string DeleteOldDoc(string FolderID,int Days=30)
    {//默認刪除30天之前
        
        //string sql = "execute [KMDBPSGPRD].dbo.[FolderDocList] @Param=35321 ";
        string sql = "execute [KMDBPSGPRD].dbo.[FolderDocList] @Param="+ FolderID ;
        
        DataSet dsMO_FP = DAO.sqlCmdDataSetSP("KMConnStr", sql);

        string DOCID = "";
        double DiffDays;
        double KeepDays = 0-Days; //檔案要保留幾天,預設30天,可輸入調整
        //把有多少料號筆數抓出來
        int H_cnt = dsMO_FP.Tables[0].Rows.Count;
        for (int i = 0; i < H_cnt; i++)
        {
            //string aa=DateTime.Today.ToString();
            //DiffDays = (Convert.ToDateTime(dsMO_FP.Tables[0].Rows[i]["docCreateDate"].ToString())- Convert.ToDateTime(DateTime.Today.ToString())).TotalDays;
            DateTime DocDate = Convert.ToDateTime(dsMO_FP.Tables[0].Rows[i]["docCreateDate"].ToString());
            DOCID = dsMO_FP.Tables[0].Rows[i]["docId"].ToString();
            DiffDays = DocDate.Subtract(DateTime.Today).TotalDays;

            if (DiffDays > KeepDays)
                continue;//45天內的不處理,跳過
          
            //    string DocDeleteUrl = GetServiceUrl(ServiceType.DeleteDocument) + GetParameterString();
            string ServiceUrl = GetKmUrl() + "document/delete/" + DOCID + "?shell=true&tenant=" + tenant + "&";

            string DocDeleteUrl = ServiceUrl + GetParameterString();

            ErrMessage = "001:Success:"+ DiffDays +";DOCID:"+ DOCID;

            WebRequest request = WebRequest.Create(DocDeleteUrl);
            request.Method = "DELETE";

            try
            {

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader sr = new StreamReader(response.GetResponseStream());

                string result = sr.ReadToEnd();

                JsonObject JOResult = (JsonObject)JsonConvert.Import(result);
                string data = JsonConvert.ExportToString(JOResult);

                //JsonResultTxt.Text = data;

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
        return ErrMessage;
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

}
